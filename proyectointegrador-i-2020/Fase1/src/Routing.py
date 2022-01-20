from AgentPinkSender import AgentPinkSender
from Packets import *
from collections import defaultdict
from heapq import *
import queue, time
import os

class Routing:

    MAX_INT16 = 65535
    PROTOCOL_DIJKSTRA = 3
    TTL_VALUE = 7

    #Function: Initialize class and it's attributes
    #Modifies: senderToGreen, nodeID, forwardingTable, graph, queueMessageDijkstra, logBook
    #Requirements: instance of AgentPinkSender class (senderToGreen), and logBook
    #Returns: void
    #Author: Daniela Vargas
    def __init__(self, senderToGreen, parLogBook, parStatisticsControlPlane):
        self.senderToGreen = senderToGreen 
        self.statisticsControlPlane = parStatisticsControlPlane
        self.nodeID = NodeInfo()
        self.lastCounter = 0
        self.changesInGraph = False
        self.forwardingTable = []
        self.graph = []
        self.queueMessageDijkstra = queue.Queue()
        self.logBook = parLogBook
        self.listAdy = []


    #Function: Empties forwardingTable, queueMessajeDijkstra and graph 
    #Modifies: forwardingTable, queueMessajeDijkstra and graph
    #Requirements: initialized class
    #Returns: void
    #Author: Daniela Vargas
    def __delattr__(self):
        self.forwardingTable = []
        self.graph = []
        self.logBook.writeToLogEvent("Routing:\t\tforwardingTable, queueMessageDijkstra and graph lists have been emptied in Forwarding functionality")

    #Function: executes dijkstra’s algorithm to find the shortest path from all the other existing nodes to the source node 
    #Modifies: nothing
    #Requirements: initialized class
    #Returns: new forwarding table 
    #Author: Daniela Vargas
    def dijkstra(self):
        g = defaultdict(list)
        for src, dest, cost in self.graph:
            g[src].append((cost, dest))
        counter = 0
        for src in g:
            g[src] = list(set(g[src]))
            counter += len(g[src])
        if counter > self.lastCounter:
            self.lastCounter = counter
            self.changesInGraph = True

        self.statisticsControlPlane.updateGraph(g)

        edges, visited, mins = [(0,self.nodeID.nodeNum,())], set(), {self.nodeID.nodeNum: (0, ())}
        while edges:
            (cost,vertex_origin,path) = heappop(edges)
            if vertex_origin not in visited:
                visited.add(vertex_origin)
                if vertex_origin != self.nodeID.nodeNum and not(path):
                    path = vertex_origin

                for cost_ady, vertex_dest in g.get(vertex_origin, ()):
                    if vertex_dest in visited: 
                        continue
                    prev_cost = mins.get(vertex_dest, None)
                    next_cost = cost + cost_ady
                    if prev_cost is None or next_cost < prev_cost[0]:
                        mins[vertex_dest] = (next_cost, path)
                        heappush(edges, (next_cost, vertex_dest, path))

        temp_forwarding_table = []

        for item in mins:
            cell = mins.get(item)
            temp_forwarding_table.append((item, cell[0], cell[1]))
        
        self.logBook.writeToLogEvent("Routing:\t\tDijkstras algorithm has been executed succesfully")
        return temp_forwarding_table


    #Function: updates the list “graph” with the upcoming information (adds several structures, where each contains the node source, 
    #           one of it’s adyacent nodes and their respective distance)
    #Modifies: the graph
    #Requirements: message of type MessageDijkstra and initialized class
    #Returns: void
    #Author: Daniela Vargas
    def addGraph(self, message):
        self.logBook.writeToLogEvent(f"Routing:\t\tAdyacent nodes info of node {message.nodeSrc} will be added to graph")
        for adyNode in range (message.numAdyNodes):
            if message.pairAdyDistance[adyNode].nodeID != 0 and message.pairAdyDistance[adyNode].distance != 0:
                self.graph.append( (message.nodeSrc, message.pairAdyDistance[adyNode].nodeID, message.pairAdyDistance[adyNode].distance) )
        
            
    #Function: constantly checks the queue of messages in order to update the graph with new information, run dijkstra, update the 
    #           forwarding table and fill a forwarding list to be sent with the forwarding protocol
    #Modifies: the forwarding table
    #Requirements: initialized class, and a variable that determines whether the cycle should continue or not (continueLoop)
    #Returns: void
    #Author: Daniela Vargas
    def execute(self, continueLoop):
        while(continueLoop): 
            message = self.queueMessageDijkstra.get()
            self.logBook.writeToLogEvent(f"Routing:\t\tMessageDijkstra of node {message.nodeSrc} will be processed ")
            if message.nodeSrc != self.MAX_INT16:
                self.addGraph(message)
                forwardingTable = self.dijkstra()
                if self.changesInGraph == True:
                    self.forwardingTable = forwardingTable
                    self.changesInGraph = False
                    # Transmito mi lista de adyacencia 
                    myBroadcastInfo = MessageBroadcast()
                    myBroadcastInfo.nodeSrc = self.nodeID.nodeNum 
                    myBroadcastInfo.TTL = self.TTL_VALUE

                    myBroadcastInfo.msgDijkstra.nodeSrc = self.nodeID.nodeNum
                    myBroadcastInfo.msgDijkstra.numAdyNodes = (int)(len(self.listAdy))
                    myBroadcastInfo.msgDijkstra.pairAdyDistance = (AdjacentListInfo * 256)(*self.listAdy)

                    self.senderToGreen.pushMessage(myBroadcastInfo, protocolPinkToGreen.protocolBroadcast.value)
                    self.logBook.writeToLogEvent(f"Routing:\t\tThe SpanningTree has been sent to the agent pink sender")


                    #  mando la nueva tabla
                    self.logBook.writeToLogEvent("Routing:\t\tNew forwarding table has been created")
                    listForwarding = []
                    node_inter = 0
                    for item in self.forwardingTable:
                        if not isinstance(item[2], tuple) or item[0] != self.nodeID.nodeNum:
                            if isinstance(item[2], tuple):
                                node_inter = item[0]
                            else:
                                node_inter = int(item[2])
                        listForwarding.append( (item[0], node_inter) )
                    self.senderToGreen.pushMessage(listForwarding, protocolPinkToGreen.protocolForwarding.value)
                    self.statisticsControlPlane.updateForwardingTable(listForwarding)
                    self.logBook.writeToLogEvent("Routing:\t\tForwarding table has been sent")
            else:
                continueLoop = False
        return

    def setNodeInfo(self, nodeInfo):
        self.nodeID = nodeInfo
        self.logBook.writeToLogEvent("Routing:\t\tRouting has received the nodeInfo ")


    #Function: adds a message to the queue “queueMessageDijkstra” 
    #Modifies: the que “queueMessajeDijkstra”
    #Requirements: message of type MessageDijkstra
    #Returns: void
    #Author: Daniela Vargas
    def pushMessageToRouting(self, message):
        self.queueMessageDijkstra.put(message)
        self.logBook.writeToLogEvent("Routing:\t\tUpcoming message will be added to routing queue")

    def setAdyList(self, listAdy):
        self.listAdy = listAdy
        self.logBook.writeToLogEvent("Routing:\t\tA new list of adjacent nodes established")