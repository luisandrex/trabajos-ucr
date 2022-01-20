import os, sys, stat, shutil , queue, threading, subprocess
import py_compile
import posix_ipc
import time, struct
from ctypes import *
from Packets import *
from shutil import copyfile

# Author: Fernando Morales
# Function: this class is used to solve all spider requests and to communicate with the multiplexor to send spiders through the network
class Sandbox:
    SPIDER_FILE = "Spider.py"
    SPIDER_EXEC = "./spider"
    SPIDER_OLD_COMPILED_NAME = './__pycache__/Spider.cpython-36.pyc'
    SPIDER_NEW_COMPILED_NAME = './__pycache__/spider'
    INVALID_MESSAGE_FOR_SANDBOX = 0 
    CHUNK_SIZE = 4089
    PROTOCOL_SANDBOX_TO_BLUE = 10
    MAX_INT16 = 65535
    SENDER = 1


    def __init__(self, parLogBook, parStatisticsControlPlane, parAgentPinkSender, parMultiplexor):

        self.logBook = parLogBook
        self.statisticsControlPlane = parStatisticsControlPlane
        self.agentPinkSender = parAgentPinkSender
        self.multiplexor = parMultiplexor

        self.send_spiders = queue.Queue()
        self.receive_spiders = queue.Queue()
        self.responses_to_spider = queue.Queue()
        self.blue_requests = queue.Queue()
        self.spider_attributes = []
        self.spider_mailboxes = []

        self.my_spider_nums = 0
        self.internal_spider_numbers = 0
        self.node_num = 0
        self.sandbox_mailbox_name = f"/mainsandbox{os.getpid()}"
        self.sandbox_mailbox = posix_ipc.MessageQueue(self.sandbox_mailbox_name, posix_ipc.O_CREAT) 

        self.eventNodeInfoReady = threading.Event()

        self.mutex_for_spider_nums = threading.Lock()

    #Function: Set the node number for the sandbox and signaling the sandbox to begin its execution
    #Modifies: The attribute of the class containing the node number.
    #Requirements: The node number.
    #Returns: void
    #Author: Fernando Morales
    def setNodeInfo(self, nodeNum):
        self.node_num = nodeNum
        self.eventNodeInfoReady.set()

    #Function: Thread which is going to be constantly receiving sandbox packets from the multiplexor
    #Modifies: The attribute of the class containing all the spiders' info(spider_attributes)
    #Requirements: The sandbox to be fully initialized
    #Returns: void
    def receive_spiders_thread(self):
        continue_loop = True
        while continue_loop:
            self.getMessagesFromMultiplexor()
            received_new_spider = self.receive_spiders.get() 
            spider_identifier = received_new_spider[0] 
            message_sandbox = received_new_spider[1]
            if spider_identifier != self.INVALID_MESSAGE_FOR_SANDBOX: 
                if spider_identifier == 1:
                    message_type = struct.unpack("!BB", message_sandbox[:2])[1]
                    
                    message_origin_node = struct.unpack("!BBH", message_sandbox[:4])[2]
                    
                    spider_id = struct.unpack("!BBHB", message_sandbox[:5])[3]
                    
                    payload_size = struct.unpack("!BBHBH", message_sandbox[:7])[4]

                    payload = struct.unpack(f"!4089s", message_sandbox[7:])[0]

                    payload = payload[:payload_size]
                    if message_type == 0:
                        self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has received the first packet of a new spider")
                        self.mutex_for_spider_nums.acquire()

                        executable_name = "./" + str(message_origin_node) + str(self.internal_spider_numbers)
                        luggage_name = "luggage" + str(message_origin_node) + str(self.internal_spider_numbers) + ".txt"
                        mailbox_name = "/mailbox" + str(message_origin_node) + str(self.internal_spider_numbers)

                        new_spider_data = SpiderData(executable_name.encode("utf-8"), spider_id, message_origin_node, payload,luggage_name.encode("utf-8"), mailbox_name.encode("utf-8"), 0, self.internal_spider_numbers)                    

                        mailbox_for_spider = posix_ipc.MessageQueue(mailbox_name, posix_ipc.O_CREAT) 
                        mailbox_data = (self.internal_spider_numbers, mailbox_for_spider)
                        self.spider_mailboxes.append(mailbox_data)

                        self.internal_spider_numbers += 1
        
                        self.spider_attributes.append(new_spider_data)

                        self.mutex_for_spider_nums.release()

                    else:
                        for spiders in self.spider_attributes:
                            if message_origin_node == spiders.origin_node and spider_id == spiders.spider_num:
                                if message_type == 1:
                                    self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has received an executable chunk")
                                    with open(spiders.executable_name, "a+b") as executable:                                    
                                        executable.write(payload)
                                        os.chmod(spiders.executable_name,stat.S_IRWXU)
                                        executable.close()

                                elif message_type == 2:   
                                    self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has received a luggage chunk")
                                    payload_to_write = payload.decode("utf-8")
                                    with open(spiders.luggage_name, "a+") as luggage:
                                        luggage.write(payload_to_write)
                                        luggage.flush()
                                        os.fsync(luggage.fileno())
                                        luggage.close()

                                elif message_type == 3:
                                    print("Voy a empezar a ejecutar araña")
                                    if message_origin_node != self.node_num:
                                        self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has received a type-3 packet, so a spider is going to be executed")                                
                                        childpid = os.fork()
                                        if childpid == 0:
                                            os.execlp(spiders.executable_name.decode("utf-8"), spiders.executable_name.decode("utf-8"), str(spiders.internal_spider_num), str(self.node_num), spiders.luggage_name.decode("utf-8"), self.sandbox_mailbox_name, spiders.mailbox_name.decode("utf-8"))                                           
                                        else:
                                            spiders.PID = childpid
                                            # new_list_of_nodes = spiders.list_of_nodes.decode("utf-8").split(",")
                                            # if len(new_list_of_nodes) != 1:
                                            #     string_list = (new_list_of_nodes[1:])
                                            #     new_list_of_nodes_new = ""
                                            #     for string in string_list:
                                            #         if string != '-':
                                            #             new_list_of_nodes_new += (string + ",")
                                            #     new_list_of_nodes_new += "-"
                                            #     spiders.list_of_nodes = new_list_of_nodes_new.encode("utf-8")
                                    else:
                                        # mandar mi luggage al nodo azul
                                        self.logBook.writeToLogEvent(f"Sandbox:\t\tSandbox has received the luggage from my spider num {spider_id}")
                                        message_to_send = MessageToSandbox(spiders.luggage_name)
                                        self.agentPinkSender.pushMessage(message_to_send, self.PROTOCOL_SANDBOX_TO_BLUE)
                else:
                    # recibo araña de mi nodo
                    self.logBook.writeToLogEvent("Sandbox:\t\tSandbox is going to execute a spider coming from its node")
                    my_new_spider = received_new_spider[1]
                    self.mutex_for_spider_nums.acquire()
                    mailbox_for_spider = posix_ipc.MessageQueue(my_new_spider.mailbox_name.decode("UTF-8"), posix_ipc.O_CREAT) 
                    print("SANDBOX")
                    print(my_new_spider.mailbox_name.decode("UTF-8"))
                    mailbox_data = (my_new_spider.internal_spider_num, mailbox_for_spider)
                    self.spider_mailboxes.append(mailbox_data)
                    self.spider_attributes.append(my_new_spider)
                    spiders = self.spider_attributes[len(self.spider_attributes) - 1]
                    print(spiders.internal_spider_num)
                    print(spiders.mailbox_name.decode("UTF-8"))
                    exec_name = spiders.executable_name.decode('UTF-8')
                    spider_exec = exec_name
                    print(f"ejecutable es {spider_exec}")
                    self.mutex_for_spider_nums.release()
                    childpid = os.fork()
                    if childpid == 0:
                        #os.execlp(spiders.executable_name.decode("utf-8"), spiders.executable_name.decode("utf-8"), str(spiders.internal_spider_num), str(self.node_num), spiders.luggage_name.decode("utf-8"), self.sandbox_mailbox_name, spiders.mailbox_name.decode("utf-8"))                                           
                        os.execlp(spider_exec, spider_exec, str(spiders.internal_spider_num), str(self.node_num), spiders.luggage_name.decode("utf-8"), self.sandbox_mailbox_name, spiders.mailbox_name.decode("utf-8"))                                           
                    else:
                        spiders.PID = childpid

            else:
                continue_loop = False
                break

    #Function: Thread which sends the spiders across the network and checks if the spiders that are going to be sent
    #          were created in this instance of a node                
    #Modifies: constantly reads from the queue
    #Requirements: The sandbox to be fully initialized
    #Returns: void
    def send_spiders_thread(self):        
        continue_loop = True
        while continue_loop:
            send_spider = self.send_spiders.get() 
            self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has received a spider to send to a node")
            nodes_decoded = send_spider.list_of_nodes.decode("utf-8")
            nodes_left_to_visit = nodes_decoded.split(",")
            to_send = True
            destination = 0
            going_to_send_to_origin = False
            print(f"nodes left to visit {nodes_decoded}")
            if len(nodes_decoded) == 1:
                destination = send_spider.origin_node
                going_to_send_to_origin = True
                print(f"Arana va para el origen {destination}")
            elif len(nodes_decoded) == 0:
                self.logBook.writeToLogEvent("Sandbox:\t\tThread send_spiders is going to finish its execution")
                continue_loop = False
                to_send = False
                break
            else:
                destination = int(nodes_left_to_visit[0])
                list_nodes_left_to_visit = ''
                for node in nodes_left_to_visit:                
                    if str(node) != "-" and int(node) != destination:
                        list_nodes_left_to_visit += str(node) + ","
                list_nodes_left_to_visit += '-'
                print(f"Envio la araña a {destination} con lista {list_nodes_left_to_visit}")
                send_spider.list_of_nodes = list_nodes_left_to_visit.encode("utf-8")
            if to_send == True:            
                executable_chunked = self.chunk_file_binary(send_spider.executable_name)
                luggage_chunked = self.chunk_file_string(send_spider.luggage_name)

                unit_multiplexor = self.multiplexor.assignNewStopAndWaitUnit(self.SENDER)
                
                message_to_start_connection = (0, struct.pack("=BBHBH1s",0,0,0,0,0,"".encode("utf-8")) , unit_multiplexor , destination)
                self.multiplexor.pushNewPacket(message_to_start_connection)
                
                if going_to_send_to_origin == False:
                    message_to_multiplexor_node_list = struct.pack(f"!BBHBH{len(send_spider.list_of_nodes)}s", 6, 0, send_spider.origin_node, send_spider.spider_num, len(send_spider.list_of_nodes), send_spider.list_of_nodes)
                
                    packet_to_multiplexor_node_list = (1, message_to_multiplexor_node_list, unit_multiplexor , destination)
                    self.multiplexor.pushNewPacket(packet_to_multiplexor_node_list)

                    self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has sent a list_of_nodes")

                    for executable_part in executable_chunked:                   
                                            
                        packet = struct.pack(f"!BBHBH{len(executable_part)}s", 6, 1,send_spider.origin_node,send_spider.spider_num, len(executable_part), executable_part)
                        message_to_multiplexor_executable_part = (1, packet, unit_multiplexor , destination)
                        self.multiplexor.pushNewPacket(message_to_multiplexor_executable_part)
                        
                        self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has sent an executable chunk")
                    
                    self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has finished sending an executable")
                
                print(f"ejecutable es {send_spider.executable_name}")
                os.remove(send_spider.executable_name)

                for luggage_part in luggage_chunked:
                    
                    message_to_multiplexor = struct.pack(f"!BBHBH{len(luggage_part)}s", 6, 2,send_spider.origin_node,send_spider.spider_num, len(luggage_part), luggage_part.encode("utf-8"))
                    message_to_multiplexor_luggage_part = (1, message_to_multiplexor, unit_multiplexor , destination)
                    self.multiplexor.pushNewPacket(message_to_multiplexor_luggage_part)
                    
                    if going_to_send_to_origin == False:
                        self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has sent a luggage chunk")
                    else:
                        self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has sent a luggage chunk to its origin node")
                    
                print("Mande valija")   
                os.remove(send_spider.luggage_name)
                self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has finished sending a luggage")
                message_to_multiplexor_finish_packet = struct.pack(f"!BBHBH1s", 6, 3,send_spider.origin_node,send_spider.spider_num, 1, "".encode("utf-8"))
                message_to_multiplexor_finish = (1, message_to_multiplexor_finish_packet, unit_multiplexor , destination)
                self.multiplexor.pushNewPacket(message_to_multiplexor_finish)

                self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has sent a type-3 packet")
                    
                message_to_finish_connection = (2, struct.pack("=BBHBH1s",0,0,0,0,0,"".encode("utf-8")) , unit_multiplexor , destination)
                self.multiplexor.pushNewPacket(message_to_finish_connection)

    #Function: Chunking the executable file and returning it as a tuple
    #Modifies: file_chunked, which is going to be returned
    #Requirements: The file name, which has to be in binary for it to properly chunk and read the file
    #Returns: The executable file chunked as a tuple
    def chunk_file_binary(self, file_name):
        file_chunked = []
        with open(file_name, "r+b") as infile:
            while True:
                chunk = infile.read(self.CHUNK_SIZE)
                if not chunk:
                    break

                file_chunked.append(chunk)
            infile.close()

        return file_chunked

    #Function: Chunking the luggage file and returning it as a tuple
    #Modifies: file_chunked, which is going to be returned
    #Requirements: The file name, which has to not be in binary for it to properly chunk and read the file
    #Returns: The luggage file chunked as a tuple
    def chunk_file_string(self, file_name):
        file_chunked = []
        with open(file_name, "r+") as infile:
            while True:
                chunk = infile.read(self.CHUNK_SIZE)
                if not chunk:
                    break

                file_chunked.append(chunk)
            infile.close()

        return file_chunked

    #Function: Thread which receives new spider requests from the blue node
    #Modifies: constantly reads from the queue
    #Requirements: The sandbox to be fully initialized
    #Returns: void
    def receive_blue_requests(self):
        continue_loop = True
        while continue_loop == True:
            new_spider_luggage_name = self.blue_requests.get()
            self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has received a new spider request from the blue node")
            if len(new_spider_luggage_name) != 0:
                self.my_spider_nums += 1
                executable_name = './' + str(self.node_num) + str(self.internal_spider_numbers)
                shutil.copyfile(self.SPIDER_FILE, executable_name)
                os.chmod(executable_name, stat.S_IRWXU)
                #self.compile_spider(self.node_num)
                forwarding_table = self.statisticsControlPlane.getForwardingTable()
                nodes_left_to_visit = ""
                for nodes in forwarding_table:
                    if nodes[1] != 0 and nodes[0] != self.node_num:
                        nodes_left_to_visit += (str(nodes[0]) + ",")
                nodes_left_to_visit += '-'

                print(nodes_left_to_visit)
                self.mutex_for_spider_nums.acquire()

                mailbox_name = "/" + str(self.node_num) + str(self.internal_spider_numbers)
                print(f"Ejecutable es {executable_name}")
                my_spider_data = SpiderData(str.encode(executable_name), self.my_spider_nums, self.node_num, str.encode(nodes_left_to_visit), new_spider_luggage_name,mailbox_name.encode("utf-8"),0,self.internal_spider_numbers)

                self.internal_spider_numbers += 1
                self.mutex_for_spider_nums.release()
                
                self.multiplexor.pushMessageForSandbox(my_spider_data)   

                # self.send_spiders.put(my_spider_data)
            else:
                self.logBook.writeToLogEvent("Sandbox:\t\tSandbox is going to stop its execution")
                continue_loop = False
                end_mailbox = [0,8]
                self.sandbox_mailbox.send(bytes(end_mailbox))
                null_spider = SpiderData("".encode("utf-8"),0,0,"".encode("utf-8"),"".encode("utf-8"),"".encode("utf-8"),0,0)
                self.send_spiders.put(null_spider)
                invalid_tuple = self.INVALID_MESSAGE_FOR_SANDBOX
                invalid_sandbox_message = struct.pack(f"!BBHBH1s",0,0,0,0,0,"".encode("utf-8"))
                invalid_message = [invalid_tuple, invalid_sandbox_message]
                self.receive_spiders.put(invalid_message) 
                               
    #Function: Thread which solves the spider requests and writes them via mailbox
    #Modifies: constantly reads from its mailbox
    #Requirements: The sandbox to be fully initialized
    #Returns: void
    def solve_sandbox_requests(self):
        continue_loop = True
        while continue_loop == True:
            request = self.sandbox_mailbox.receive()
            self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has received a spider request")
            int_values = [x for x in request[0]]
            request_spider_id = int_values[0]
            request_number = int_values[1]
            if request_number == 0:
                self.logBook.writeToLogEvent(f"Sandbox:\tSpider#{request_spider_id} has finished its execution")
                self.my_spider_nums += 1
                for spider in self.spider_attributes:
                    if spider.internal_spider_num == request_spider_id:
                        for mailbox_data in self.spider_mailboxes:
                            if mailbox_data[0] == request_spider_id:
                                mailbox = mailbox_data[1]
                                mailbox.close()
                                mailbox.unlink()
                                self.send_spiders.put(spider)
            elif request_number == 8:
                # Internal protocol to stop its execution
                continue_loop = False
            else:
                self.logBook.writeToLogEvent(f"Sandbox:\tReceived a request from Spider#{request_spider_id} with number {request_number}")
                request = (request_spider_id,request_number)
                self.send_request(request)
                respuesta = self.receiver()
                response_bytes = str.encode(respuesta[1])
                for x in self.spider_attributes:
                    if x.internal_spider_num == request_spider_id:
                        for y in self.spider_mailboxes:
                            if y[0] == request_spider_id:
                                this_mailbox_to_send = y[1]
                                self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has solved a request from a spider")
                                this_mailbox_to_send.send(response_bytes)          

    #Function: Send a certain request to the statistics control plane instance
    #Modifies: Nothing
    #Requirements: The request which is going to be sent to the statistics control plane
    #Returns: void
    def send_request(self,request):
        self.statisticsControlPlane.pushRequest(request)

    #Function: Receive a certain request from the statistics control plane instance
    #Modifies: Nothing
    #Requirements: Nothing
    #Returns: The solved request which was received from the statistics control planes
    def receiver(self):
        return self.statisticsControlPlane.getResponseToRequest()

    #Function: Push a new message coming from the blue node(new spider request)
    #Modifies: Nothing
    #Requirements: The message from the blue node to be pushed to the corresponding queue
    #Returns: void
    def pushMessageFromBlue(self, msgSandbox):
        self.blue_requests.put(msgSandbox)

    #Function: Compile the Spider.py file into a .pyc and rename it accordingly
    #Modifies: The new spider executable
    #Requirements: The spider number to whom this spider belongs
    #Returns: The new path for the spider executable
    def compile_spider(self,spider_num):
        py_compile.compile(self.SPIDER_FILE)
        spider_num_str = str(spider_num)
        spider_path = self.SPIDER_NEW_COMPILED_NAME + spider_num_str
        os.rename(self.SPIDER_OLD_COMPILED_NAME,spider_path)
        new_spider_path = self.SPIDER_EXEC + str(spider_num)
        shutil.move(spider_path,new_spider_path)
        os.chmod(new_spider_path, stat.S_IRWXU)
        return new_spider_path

    #Function: Begin the sandbox execution, all of its threads and, afterwards, waiting for the 
    #          multiple spider processes that are left alone and closing the main server mailbox
    #Modifies: Starts all of the threads 
    #Requirements: continueLoop, which is used to know when the execution halted
    #Returns: void
    def execute(self, continueLoop):
        
        self.eventNodeInfoReady.wait()
        self.eventNodeInfoReady.clear()
        
        self.logBook.writeToLogEvent("Sandbox:\tSandbox has been initialized")              

        thread_receive_spiders = threading.Thread(target=self.receive_spiders_thread)
        thread_send_spiders = threading.Thread(target=self.send_spiders_thread)
        thread_sandbox_requests = threading.Thread(target=self.solve_sandbox_requests)
        thread_blue_requests = threading.Thread(target=self.receive_blue_requests)

        thread_receive_spiders.start()
        thread_send_spiders.start()
        thread_sandbox_requests.start()
        thread_blue_requests.start()

        thread_blue_requests.join()
        thread_sandbox_requests.join()
        thread_send_spiders.join()
        thread_receive_spiders.join()

        for spider_info in self.spider_attributes:
            os.waitpid(spider_info.PID,0)
        

        self.sandbox_mailbox.close()
        self.sandbox_mailbox.unlink()

    #Function: Checks the multiplexor's queue for any packets for the sandbox
    #Modifies: Puts the corresponding packet into this sandbox's corresponding queue
    #Requirements: Multiplexor to be initialized properly
    #Returns: void
    def getMessagesFromMultiplexor(self):
        message_for_sandbox = self.multiplexor.getFromQueueForSandbox()
        self.logBook.writeToLogEvent("Sandbox:\t\tSandbox has received a stop&wait packet")
        self.receive_spiders.put(message_for_sandbox)
