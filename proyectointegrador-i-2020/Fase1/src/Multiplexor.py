from ctypes import *
from Packets import * 
from StopAndWait import StopAndWait
import time, os ,queue, random,threading, struct

class Multiplexor:
    SIZE_OF_MULTIPLEXOR = 256
    
    MAX_INT = 4294967295
    MAX_INT16 = 65535
    MAX_INT8 = 255

    OCCUPIED = 1
    SAW_UNIT = 2
    SAW_THREAD = 3

    SENDER = 1
    RECEIVER = 2

    FROM_SANDBOX = 1
    FROM_GREEN = 2

    SUCCESS = True
    FAILURE = False

    SYN_RAISED = 128
    SYN_ACK_RAISED = 192
    ACK_RAISED = 64
    FIN_RAISED = 32
    FIN_ACK_RAISED = 96

    FROM_SENDER = 0
    FROM_RECEIVER = 1

    ESTABLISH_CONS = 0
    SEND_MESSAGE = 1
    CLOSE_CONS = 2
    MESSAGE_FROM_OTHER_NODE = 3

    DEST = 1


    #Function: Initialize class and it's attributes
    #Modifies: It initializes every single one of this object's atrributes.
    #Requirements: instance of AgentPinkSender class (senderToGreen), and logBook
    #Returns: void
    #Author: Luis Sanchez
    def __init__(self, parAgentPinkSender, parLogBook):
        self.agentPinkSender = parAgentPinkSender
        self.logBook = parLogBook
        self.myInfo = NodeInfo(-1,-1,"".encode("utf-8"))
        self.continueLoop = True
        self.multiplexor = []
        self.pairs = []
        self.messagesQueue = queue.Queue()
        self.messages = queue.Queue()
        self.eventNodeInfoReady = threading.Event()
        self.queueForSandbox = queue.Queue()
        self.logBook.writeToLogEvent("Multiplexor:\tThe Multiplexor has been initialized.")


    #Function: It sets the multiplexor's information.
    #Modifies: It modifies the attribute myInfo. 
    #Requirements: It requires the struct NodeInfo as a parameter
    #Returns: void
    #Author: Luis Sanchez
    def setMultiplexorInfo(self, parId):
        self.myInfo = parId
        self.logBook.writeToLogEvent("Multiplexor:\tThe Multiplexor's info has been set.")
        self.eventNodeInfoReady.set()


    #Function: It sets the multiplexor's structure as a list of triplets, where there is a bool indicating whether the unit is in use or not, the instance of stop and wait and the thread of execution.
    #          It sets the list used as an indicator of the pairs of ids and the role of the unit based on the index of the multiplexor, this list is used to access the stop and wait units. 
    #Modifies: It initializes the multiplexor and the list of pairs
    #Requirements: Nothing
    #Returns: void
    #Author: Luis Sanchez
    def setMultiplexor(self,):
        self.eventNodeInfoReady.wait()
        self.eventNodeInfoReady.clear()   

        for index in range(0,self.SIZE_OF_MULTIPLEXOR):
            num = index

            inUse = False

            stopAndWait = StopAndWait(self.agentPinkSender, self.logBook, self.queueForSandbox)
            stopAndWait.setRole(3)
            
            threadStopAndWait = threading.Thread(target=stopAndWait.execute, args=(1,)) 
            threadStopAndWait.start()
            triplet = {1: inUse, 2: stopAndWait, 3: threadStopAndWait}     

            self.multiplexor.append(triplet)

            pair = {1: -1 , 2: -1 , 3: -1}
            self.pairs.append(pair)#coloco esto aca, o lo asigno mejor despues?
        self.logBook.writeToLogEvent("Multiplexor:\tThe Multiplexor and the list of pairs has been set.")



    #Function: It acts as a dispatcher of messages, where it classifies them first based on the pair it gets, if the pair received has a 0,1 or 2, then, it was received from sandbox, where the unit is used to place messages in 
    #           the Stop and Wait unit (if the message[0] is one of them, then, the index can be accessed directly by sandbox and acts as the receiver), where, as if message[0] is 3, then, it comes from the green node and the 
    #           message has to be classified based on its flags, ids and rn and sn. 
    #Modifies: It places elements on the stop and wait queues, either from upper layer or from the green node, it sets new stop and wait units in the multiplexor and gets elements from messageQueue.
    #Requirements: Nothing.
    #Returns: Nothing.
    #Author: Luis Sanchez
    def execute(self, parContinueLoop):
        self.setMultiplexor()
        # self.probar()
        
        while self.continueLoop is True:
            messageRecv = self.messagesQueue.get()
            self.logBook.writeToLogEvent("Multiplexor:\t A new message has been received.")

            #print(f"mensaje es de tipo {messageRecv[0]}")
            if messageRecv[0] is self.ESTABLISH_CONS:

                unit = messageRecv[2]
                self.multiplexor[unit][self.SAW_UNIT] = StopAndWait(self.agentPinkSender, self.logBook, self.queueForSandbox)

                self.multiplexor[unit][self.SAW_UNIT].setNodeInfo(self.myInfo)  
                self.multiplexor[unit][self.SAW_UNIT].setRole(self.SENDER)
                self.multiplexor[unit][self.SAW_UNIT].setPort(unit)
                self.multiplexor[unit][self.SAW_UNIT].setDest(messageRecv[3])

                self.multiplexor[unit][self.SAW_THREAD] = threading.Thread(target=self.multiplexor[unit][self.SAW_UNIT].execute, args=(1,))
                self.multiplexor[unit][self.SAW_THREAD].start()
                self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from Sandbox and its purpose was to stablish a new connection with the node {messageRecv[3]} ")
                #print(f"Multiplexor:\tme llega mensaje de iniciar conexion de capa superior")

            elif messageRecv[0] is self.SEND_MESSAGE:
                unit = messageRecv[2]

                self.multiplexor[unit][self.SAW_UNIT].pushNewPacketFromUpperLayer((0,messageRecv[1]))
                self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from Sandbox and its purpose was to place a new message in the queue of the unti #{unit} ")

            elif messageRecv[0] is self.CLOSE_CONS:
                unit = messageRecv[2]
                # print(f"Multiplexor:\tVoy a finalizar la ejecucion de este")
                self.multiplexor[unit][self.SAW_UNIT].pushNewPacketFromUpperLayer((1,messageRecv[1]))
                self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from Sandbox and its purpose was to close the connection with the node {messageRecv[3]} ")


            elif messageRecv[0] is self.MESSAGE_FROM_OTHER_NODE:#el else corresponde a un mensaje de otro verde.
                msgPacket = messageRecv[1]
                msg_codified = struct.unpack('=HH6BH4096s',msgPacket)
                msg = MessageStopAndWait(msg_codified[0], msg_codified[1], msg_codified[2], msg_codified[3], msg_codified[4], msg_codified[5], msg_codified[6], msg_codified[7], msg_codified[8], msg_codified[9])
                #print(f"recibo flag {msg.FLAGS} de {msg.nodeSrc} con id1 {msg.ID_1} y id2 {msg.ID_2}, se supone que yo soy {msg.nodeDest}")
                self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from the green node. ")

                payloadToSandBox = msg_codified[12:]
                typeMsg = self.typeOfMessage(msg) 
                #print(f"Multiplexor:\t\tMe llega mensaje con simbolo {typeMsg}, de sender es 0 y de receiver es 1, de {msg.nodeSrc} a {msg.nodeDest}")
                if typeMsg is self.FROM_SENDER:
                    
                    if msg.FLAGS == self.SYN_RAISED:#podriamos poner un char rol en pairs 
                        #print(f"Multiplexor:\tme llega mensaje de iniciar conexion de otro nodo")
                        unit = self.assignNewStopAndWaitUnit(self.RECEIVER)

                        #print(f"{unit} es la unidad a la que acceder ")
                        #self.multiplexor[unit][self.SAW_UNIT] = StopAndWait(self.agentPinkSender, self.logBook)
                        self.multiplexor[unit][self.SAW_UNIT] = StopAndWait(self.agentPinkSender, self.logBook, self.queueForSandbox)

                        self.multiplexor[unit][self.SAW_UNIT].setNodeInfo(self.myInfo)  
                        self.pairs[unit][self.SENDER] = msg.ID_1

                        #index = self.getMultiplexorIndex(msg.ID_1, msg.ID_2, self.RECEIVER)
                        #print(f"indice del multiplexor a acceder es con  {self.pairs[unit][1]} y { self.pairs[unit][2]}, soy { self.pairs[unit][2]} donde 1 es sender y 2 es receiver")

                        self.multiplexor[unit][self.SAW_UNIT].setRole(self.RECEIVER)
                        self.multiplexor[unit][self.SAW_UNIT].setPort(unit)
                        self.multiplexor[unit][self.SAW_UNIT].setDest(msg.nodeSrc)

                        self.multiplexor[unit][self.SAW_THREAD] = threading.Thread(target=self.multiplexor[unit][self.SAW_UNIT].execute, args=(1,))
                        self.multiplexor[unit][self.SAW_THREAD].start()
                        self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from the green node with the SYN flag raised, so a new Stop and Wait unit acting as the receiver has been stablished.")

                    elif msg.FLAGS == self.FIN_RAISED:
                        unit = msg.ID_2                        
                        self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from the green node with the FIN flag raised, so the process of closing the connection will start.")
                        #ver que hago en este caso
                    else:
                        unit = msg.ID_2                        
                        self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from the green node carrying data.")


                    index = self.getMultiplexorIndex(msg.ID_1, msg.ID_2, self.RECEIVER)
                    #print(f"indice del multiplexor a acceder es {index} (no uso este, sino el otro)")

                    self.multiplexor[unit][self.SAW_UNIT].pushNewPacket(msgPacket)

                elif typeMsg is self.FROM_RECEIVER:

                    if msg.FLAGS == self.SYN_ACK_RAISED:
                        self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from the green node with the SYN_ACK flag raised, so the receiver successfully received the message to start a new connection.")

                        self.pairs[msg.ID_1][self.RECEIVER] = msg.ID_2
                        unit  = self.getMultiplexorIndex(msg.ID_1, msg.ID_2, self.SENDER)
                        #print(f"unidad a usar es {unit}")

                    else:
                        unit = self.getMultiplexorIndex(msg.ID_1, msg.ID_2, self.SENDER)
                        self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from the green node and is acting as an ACK.")

                        #print(f"unidad a usar es {unit}")
                    self.multiplexor[msg.ID_1][self.SAW_UNIT].pushNewPacket(msgPacket)#  cambio de unit
            else:
                self.logBook.writeToLogEvent(f"Multiplexor:\tThe multiplexor will begin the process of shutting down.")
                self.continueLoop = False
        counter = 0
        for element in self.pairs:
            print(f"Pareja: {counter} es[{element[1]}][{element[2]}], rol = {element[3]} donde 1 es sender y 2 es receiver")
            counter += 1
            if counter == 6:
                break
        for element in self.multiplexor:
            if element[self.SAW_THREAD].is_alive() or element[self.OCCUPIED]:
                #print("entro a hacer join")
                self.logBook.writeToLogEvent(f"Multiplexor:\tA unit of stop and wait that didn't join, will be shutting down.")

                element[self.SAW_UNIT].finalizeComs()
                element[self.SAW_THREAD].join()
                #print("hago join")
        self.queueForSandbox.put((0,1))


    #Function: It places a new packet in the queue for the multiplexor
    #Modifies: It adds a new packet to messagesQueue.
    #Requirements: parPacket as a packet to be placed in the queue. It could be from either, Sandbox or Agent Pink Receiver.
    #Returns: void
    #Author: Luis Sanchez
    def pushNewPacket(self, parPacket):
        self.messagesQueue.put(parPacket)
        self.logBook.writeToLogEvent(f"Multiplexor:\tA new packet was received in the queue of messages.")


    #Function: It classifies a message that is beign process, in case it comes from the Agent Pink Receiver, classifing it as either a message from a sender or from a receiver.
    #Modifies: Nothing
    #Requirements: parMessage as the message to be processed.
    #Returns: A int that defines where the message came from.
    #Author: Luis Sanchez
    def typeOfMessage(self,parMessage):
        if (parMessage.FLAGS == self.SYN_RAISED) or (parMessage.FLAGS == self.FIN_RAISED) or (parMessage.FLAGS == self.ACK_RAISED and parMessage.SN ==0) or (parMessage.SN != 0) or (parMessage.payload_size != 0) :
            self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from the sender.")
            return self.FROM_SENDER
        else:
            self.logBook.writeToLogEvent(f"Multiplexor:\tThe message received has been determined that came from the receiver.")
            return self.FROM_RECEIVER


    #Function: It assigns a new Stop and Wait unit for an upcoming connection, searching for a unit that is neither alive (the thread) or occupied.
    #Modifies: The multiplexor and the list of pairs, in case it finds an available spot, it reserves it.
    #Requirements: Role as the role that this side of the Stop and Wait connection will take, as either receiver or sender. 
    #Returns: A int that indicates the unit that was reserved for the connection.
    #Author: Luis Sanchez
    def assignNewStopAndWaitUnit(self, role):
        unit = -1
        assigned = False
        for element in range (0, self.SIZE_OF_MULTIPLEXOR-1):
            #print(f"Multiplexor:\tno he encontrado una unidad")
            #print(f"Respuesta a esta vivo : {self.multiplexor[element][self.SAW_THREAD].is_alive()} y a si esta ocupado es {self.multiplexor[element][self.OCCUPIED]}")
            if not self.multiplexor[element][self.SAW_UNIT].imInUse() :
                # Se pasa para aca para que se le haga el join y clear antes del start
                self.multiplexor[element][self.SAW_THREAD].join()
                self.clearUnitOut(element)

                # ********************************************************************
                unit = element
                
                self.multiplexor[unit][self.OCCUPIED] = True
                if role == self.SENDER:
                    self.pairs[unit][1] = unit
                    self.pairs[unit][2] = 0
                    self.pairs[unit][3] = self.SENDER
                    self.logBook.writeToLogEvent(f"Multiplexor:\tThe unit #{unit} has been reserved acting as the role of sender")

                else:
                    self.pairs[unit][1] = 0
                    self.pairs[unit][2] = unit
                    self.pairs[unit][3] = self.RECEIVER
                    self.logBook.writeToLogEvent(f"Multiplexor:\tThe unit #{unit} has been reserved acting as the role of receiver")

                assigned = True
                break
        if not assigned:
            self.logBook.writeToLogEvent(f"Multiplexor:\tEvery single one of the stop and wait units is being used, so a new one couldn't be reserved.")
        print(f"ACABO DE ASIGNAR LA UNIDAD {unit} en el multiplexor para la recepcion o emision de mensajes")

        return unit


    #Function: It clears the list of pairs and the multiplexor unit to be used and the Stop and Wait unit in the index indicated.
    #Modifies: The multiplexor and the list of pairs, by reseting the information of the index.
    #Requirements: Unit as the unit of the list of pairs and multiplexor to access.
    #Returns: Nothing.
    #Author: Luis Sanchez
    def clearUnitOut(self,unit):
        self.pairs[unit][1] = 0
        self.pairs[unit][2] = 0

        self.multiplexor[unit][self.OCCUPIED] = False
        self.multiplexor[unit][self.SAW_UNIT].clearUnit()
        self.logBook.writeToLogEvent(f"Multiplexor:\tThe unit #{unit} has been cleared, so its available for a new connection.")


   
    #Function: It gives the index of the multiplexor based on the id_1 and id_2 of a message received, and the role it possesses in the communication.
    #Modifies: Nothing
    #Requirements: The id_1 and id_2 of the message received in the multiplexor, also, the rol that the message is being sent to (to the receiver or sender).
    #Returns: An int indicating the index of the multiplexor to access.
    #Author: Luis Sanchez
    def getMultiplexorIndex(self, paramID1, paramID2, rol):
        index = 1000
        if rol == self.SENDER:
            #print(f"\t\tSOY SENDER Y MIS IDS SON ID_1   {paramID1}Y ID_2   {paramID2}")
            for element in range (0, self.SIZE_OF_MULTIPLEXOR):
                if self.pairs[element][1] == paramID1 and self.pairs[element][2] == paramID2 and self.pairs[element][3] == rol:
                    index = element
                    break
            self.logBook.writeToLogEvent(f"Multiplexor:\tThe index for the communication with the ids {paramID1} and {paramID2} as the role of receiver has been determined to be {index}")

        else:
            #print(f"\t\tSOY RECEIVER Y MIS IDS SON ID_1   {paramID1}Y ID_2   {paramID2}")

            for element in range (0, self.SIZE_OF_MULTIPLEXOR):
                if self.pairs[element][2] == paramID2  and self.pairs[element][1] == paramID1  and self.pairs[element][3] == rol:
                    index = element
                    break
            self.logBook.writeToLogEvent(f"Multiplexor:\tThe index for the communication with the ids {paramID1} and {paramID2} as the role of receiver has been determined to be {index}")

        #print(f"\t \t EL INDICE DE LA COMUNICACION ES {index}")
        return index


    #Function: It lets sandbox get elements from the queue for sandbox.
    #Modifies: It takes out an element from the queue for sandbox.
    #Requirements: Nothing.
    #Returns: A message for sandbox codified, to be used by the sandbox utility.
    #Author: Luis Sanchez
    def getFromQueueForSandbox(self):
        element = self.queueForSandbox.get()
        self.logBook.writeToLogEvent(f"Multiplexor:\tA new element has been received from the multiplexor for sandbox")
        return element

    def pushMessageForSandbox(self, spider_from_blue_node):
        spider = (2,spider_from_blue_node)
        self.queueForSandbox.put(spider)