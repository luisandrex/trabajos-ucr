from AgentPinkSender import AgentPinkSender
from Packets import *
from collections import defaultdict
from threading import Lock, Event
import queue

# Author: Jose Andres Viquez
# Function: This class is used to store all the statistics about the control plane in the network.
class StatisticsControlPlane:

    MAX_INT8 = 255
    MAX_INT16 = 65535

    FORWARDING_TABLE_REQUEST = 1
    NODES_IN_SPANNNING_TREE_REQUEST = 2
    GRAPH_REQUEST = 3
    PACKETS_FROM_SRC_TO_SRC_REQUEST = 4
    PACKETS_SENT_TO_NEIGHBOR_REQUEST = 5
    PACKETS_RECV_FROM_NEIGHBOR_REQUEST = 6
    PACKETS_FORWARDING_BROADCAST_REQUEST = 7

    def __init__(self, parLogBook, parAgentSender):
        self.logBook = parLogBook
        self.agentSender = parAgentSender
        self.forwardingTable = []
        self.nodesInSpanningTree = []
        self.graph = defaultdict(list)
        self.mutex = Lock()
        self.queueRequest = queue.Queue()
        self.queueResponseToRequest = queue.Queue()
        self.queueResponsesToSandBox = queue.Queue()

    #Function: Update the forwarding table of the node. The routing class call it with the appropiate 
    #           forwarding table each time that the table change.
    #Modifies: The attribute of the class containing the forwarding table.
    #Requirements: The new forwarding table to set in the class.
    #Returns: void
    #Author: Jose Andres Viquez
    def updateForwardingTable(self, parForwardingTable):
        self.mutex.acquire()
        self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe forwarding table has been setted")
        self.forwardingTable = parForwardingTable
        self.mutex.release()
    
    #Function: Update the nodes in the spanning tree of the node. The spanning tree class call it with
    #           the appropiate list of nodes each time that it change.
    #Modifies: The attribute of the class containing the list of nodes in the spannig tree.
    #Requirements: The new list of nodes to set up in the class.
    #Returns: void
    #Author: Jose Andres Viquez
    def updateNodesInSpanningTree(self, parNodesInSpanningTree):
        self.mutex.acquire()
        self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe nodes in the spanning tree has been setted")
        self.nodesInSpanningTree = parNodesInSpanningTree
        self.mutex.release()

    #Function: Update the graph of the node. The routing class call it with the appropiate graph 
    #           each time that the graph change.
    #Modifies: The attribute of the class containing the graph.
    #Requirements: The new graph to set in the class.
    #Returns: void
    #Author: Jose Andres Viquez
    def updateGraph(self, parGraph):
        self.mutex.acquire()
        self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe graph has been setted")
        self.graph = parGraph
        self.mutex.release()

    #Function: Get the forwarding table stored in the class.
    #Modifies: Obtain the mutex to doesn't read while someone is writing.
    #Requirements: Nothing.
    #Returns: The forwarding table stored by the class.
    #Author: Jose Andres Viquez
    def getForwardingTable(self):
        self.mutex.acquire()
        self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe forwarding table has been gotten")
        retorno = self.forwardingTable
        self.mutex.release()
        return retorno
    

    #Function: Get the list of nodes in the spanning tree stored in the class.
    #Modifies: Obtain the mutex to doesn't read while someone is writing.
    #Requirements: Nothing.
    #Returns: The list of nodes in the spanning tree stored by the class.
    #Author: Jose Andres Viquez
    def getNodesInSpanningTree(self):
        self.mutex.acquire()
        self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe nodes in the spanning tree has been gotten")
        retorno = self.nodesInSpanningTree
        self.mutex.release()
        return retorno

    #Function: Get the graph stored in the class.
    #Modifies: Obtain the mutex to doesn't read while someone is writing.
    #Requirements: Nothing.
    #Returns: The graph stored by the class.
    #Author: Jose Andres Viquez
    def getGraph(self):
        self.mutex.acquire()
        self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe graph has been gotten")
        retorno = self.graph
        self.mutex.release()
        return retorno

    #Function: It's used to take the requests of the sandbox. If it's a request about control plane, this 
    #           class could manage it, if not, it has to passed it, by the agentPinkSender and by the pipe,
    #           to the statistics data plane. It only handles one by one request
    #Modifies: Nothing.
    #Requirements: Nothing.
    #Returns: The list of nodes in the spanning tree stored by the class.
    #Author: Jose Andres Viquez
    def takeRequest(self, parContinueLoop):
        while parContinueLoop:  
            request = self.queueRequest.get()
            #print(request)
            if request[1] == self.MAX_INT8:
                parContinueLoop = False
            else:
                self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tA new request is going to be processed")
                if request[1] == self.FORWARDING_TABLE_REQUEST:
                    self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe request was the forwarding table of the actual node")
                    forwarding = self.getForwardingTable()
                    response = str('')
                    for pair in forwarding:
                        response += str(f"{pair[0]},{pair[1]};")
                    response += '-'
                    self.queueResponsesToSandBox.put((request[0], response))

                elif request[1] == self.NODES_IN_SPANNNING_TREE_REQUEST:
                    self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe request was the nodes in the spanning tree of the actual node")
                    nodesInSpanningTree = self.getNodesInSpanningTree() 
                    response = str('')
                    for node in nodesInSpanningTree:
                        response += str(f"{node},")
                    response += '-'
                    self.queueResponsesToSandBox.put((request[0], response))


                elif request[1] == self.GRAPH_REQUEST:
                    self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe request was the graph of the node")
                    graph = self.getGraph()
                    response = str('')
                    for node in graph:
                        response += str(f"{node},")
                        for ady in graph[node]:
                            response += str(f"{ady[1]},")
                        response = response[:-1]
                        response += ';'
                    response += '-'
                    self.queueResponsesToSandBox.put((request[0], response))

                elif request[1] == self.PACKETS_FROM_SRC_TO_SRC_REQUEST:
                    self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe request was the counter of packets from source to destination")
                    requestForStatisticsDataPlane = request[1]
                    self.agentSender.pushMessage(requestForStatisticsDataPlane, protocolPinkToGreen.protocolPacketsFromSrcToSrc.value)
                    responseToRequest = self.queueResponseToRequest.get()
                    response = str('')
                    
                    for node in self.getGraph():
                        found = False
                        for item in responseToRequest:
                            if node == item[0]:
                                response += str(f"{node},{item[1]}")
                                found = True
                        if not found:
                            response += str(f"{node},0")
                        response += ';'                        
                    
                    response += '-'
                    self.queueResponseToRequest.task_done()
                    
                    self.queueResponsesToSandBox.put((request[0], response))

                elif request[1] == self.PACKETS_SENT_TO_NEIGHBOR_REQUEST:
                    self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe request was the counter of packets sent by each adjacent node")
                    requestForStatisticsDataPlane = request[1]
                    self.agentSender.pushMessage(requestForStatisticsDataPlane, protocolPinkToGreen.protocolPacketsSentToNeighbor.value)
                    responseToRequest = self.queueResponseToRequest.get()
                    response = str('')
                    for item in responseToRequest:
                        response += str(f"{item[0]},{item[1]};")
                    response += '-'
                    self.queueResponseToRequest.task_done()
                    
                    self.queueResponsesToSandBox.put((request[0], response))

                elif request[1] == self.PACKETS_RECV_FROM_NEIGHBOR_REQUEST:                
                    self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe request was the counter of packets received from each node by which I'm a neighbor")
                    requestForStatisticsDataPlane = request[1]
                    self.agentSender.pushMessage(requestForStatisticsDataPlane, protocolPinkToGreen.protocolPacketsRecvFromNeighbor.value)
                    
                    responseToRequest = self.queueResponseToRequest.get()
                    response = str('')

                    for nodes in self.getForwardingTable():
                        found = False
                        # its adjacent
                        if nodes[0] == nodes[1]:
                            for item in responseToRequest:
                                if nodes[0] == item[0]:
                                    response += str(f"{nodes[0]},{item[1]}")
                                    found = True
                            if not found:
                                response += str(f"{nodes[0]},0")
                            response += ';'                        

                    response += '-'

                    self.queueResponseToRequest.task_done()
                    
                    self.queueResponsesToSandBox.put((request[0], response))

                else: # request is self.PACKETS_FORWARDING_BROADCAST_REQUEST
                    self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe request was the counter of packets the forwarding and broadcast packets")                    
                    requestForStatisticsDataPlane = request[1]
                    self.agentSender.pushMessage(requestForStatisticsDataPlane, protocolPinkToGreen.protocolPacketsForwardingBroadcast.value)
                    responseToRequest = self.queueResponseToRequest.get()

                    response = str(f"{responseToRequest[0]},{responseToRequest[1]},-")
                    self.queueResponseToRequest.task_done()

                    self.queueResponsesToSandBox.put((request[0], response))

            self.queueRequest.task_done()

    #Function: It's used to add to the queue of responses to request a new response to the request made.
    #Modifies: Add to the queue a new response to the request of the data plane.
    #Requirements: The response to the request.
    #Returns: Nothing
    #Author: Jose Andres Viquez
    def pushResponseToRequest(self, response):
        self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tThe response to one request to the data plane has been answered")        
        self.queueResponseToRequest.put(response)

    #Function: It's used by the sandbox to get each of the response to the requests made.
    #Modifies: Only extract a response of the queue os responses ready to be send to the sandbox.
    #Requirements: Nothing
    #Returns: The response to the last request made.
    #Author: Jose Andres Viquez
    def getResponseToRequest(self):
        # while self.queueResponsesToSandBox.empty():
        #     pass
        response = self.queueResponsesToSandBox.get()
        self.queueResponsesToSandBox.task_done()
        return response

    #Function: It's used by the sandbox to request a new statistics.
    #Modifies: Only add one request to the queue of requests.
    #Requirements: The request to attend.
    #Returns: Nothing.
    #Author: Jose Andres Viquez
    def pushRequest(self, request):
        self.logBook.writeToLogEvent("StatisticsControlPlane:\t\tA new request from Spider has been added")
        self.queueRequest.put(request)
     
    #Function: It's used to start the thread in charge of resolve the requests made by the sandbox
    #Modifies: Doesn't modify anything.
    #Requirements: A variable indicating the permanency of the node.
    #Returns: Nothing.
    #Author: Jose Andres Viquez
    def execute(self, continueLoop):
        self.takeRequest(continueLoop)