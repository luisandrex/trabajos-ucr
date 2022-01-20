from ctypes import *
from Packets import * 
import threading, queue
from AgentPinkSender import AgentPinkSender
from Routing import Routing
import time, os ,queue, random,threading


# Author:           Luis Andres Sanchez, Jose Andres Viquez, Daniela Vargas, Fernando Morales
# Definition: This class' function is to create the Spanning Tree needed for broadcast messages 
class SpanningTree:

    MAX_INT8 = 255
    MAX_INT16 = 65535
    CAN_JOIN_QUESTION = 0
    JOIN = 1
    CANT_JOIN = 2   
    STILL_JOIN = 3
    IN_TREE = 4
    PARTIALLY_IN_TREE = 5
    NOT_IN_TREE = 6
    ALIVE = 7
    NOT_ALIVE = 8
    NODE_NOT_INITIALIZED = 65535

    # Function: It initiates the object and its attributes. 
    # Modifies: It sets some of the attributes of this object, which are routing, the agent pink sender, isInTree and the logbook. It also sets the queues. 
    # Requirements: It requires routing, agent pink sender and logbook objects.
    # Return: this function does not return anything.  
    def __init__(self, parRouting, parAgentPinkSender, parLogBook, parStatisticsControlPlane):
        self.myInfo = NodeInfo(self.MAX_INT16,self.MAX_INT16,"".encode("utf-8"))
        
        self.routing = parRouting
        self.agentPinkSender = parAgentPinkSender
        self.logBook = parLogBook
        self.statisticsControlPlane = parStatisticsControlPlane

        self.isInTree = False
        self.continueLoop = True
        self.queueOfMessage = queue.Queue()
        self.queueQuestionMessages = queue.Queue()
        self.queueResponseMessages = queue.Queue()

        self.eventReadyToStart = threading.Event()

        self.adyListToSend = []
        self.listOfAdyacents = []
        self.listNodesInSpanning = []   
        self.logBook.writeToLogEvent("SpanningTree:\tThe SpanningTree functionality has been initiated")


    def __delattr__(self):
        self.listOfAdyacents = []


    # Function: Sets the attribute corresponding to the information of this node.
    # Modifies: It sets the attribute corresponding to the information of this node.
    # Requirements: It requires a NodeInfo structure as a parameter.
    # Return: this function does not return anything.
    def setNodeInfo(self, parId):
        self.myInfo = parId
        self.logBook.writeToLogEvent("SpanningTree:\tThis node's info has been set")


    # Function: Sets the attribute corresponding to the information of the adjacent nodes of this node.
    # Modifies: It sets the attribute corresponding to the information of the adjacent nodes of this node.
    # Requirements: It requires an array of NodeInfo structures as a parameter.
    # Return: this function does not return anything.
    def setAdyInfo(self, parAdy):
        for adyInfo in parAdy:
            self.listOfAdyacents.append([adyInfo, self.NOT_ALIVE, self.NOT_IN_TREE])
            adyacentLinkInfo = AdjacentListInfo(adyInfo.nodeNum, 2)
            if self.myInfo.ip == adyInfo.ip:
                adyacentLinkInfo.distance = 1
            self.adyListToSend.append(adyacentLinkInfo)
        self.logBook.writeToLogEvent("SpanningTree:\tThe information of this nodes adjacents has been set")
        self.eventReadyToStart.set()

    # Function: Dispatches the messages that are received by the queueOfMessages. Depending on the message's question attribute.
    #          the message will be sent to one queue or another. If the message's question is an invalid message, this means that
    #          the program has finished its execution.
    # Modifies: It modifies the queues used in the class, queueQuestionMessages or queueResponseMessages, by adding a message to
    #          the correct queue.
    # Requirements: It requires both queues initialized and the variable continueLoop to be received as an attribute.
    # Return:   This function does not return anything.
    def dispatcherMessages(self, continueLoop):
        while self.continueLoop:
            message = self.queueOfMessage.get()

            if message.question == self.MAX_INT8:
                self.continueLoop = False
                continueLoop = False
                self.queueResponseMessages.put(message)
                self.queueQuestionMessages.put(message)
                self.logBook.writeToLogEvent("SpanningTree:\tThe SpanningTree functionality will be shutting down")
            elif message.question == self.CAN_JOIN_QUESTION or message.question == self.STILL_JOIN:
                # mensajes con la pregunta de puedo unirme
                self.queueQuestionMessages.put(message)
                self.logBook.writeToLogEvent("SpanningTree:\tA message has been placed in queueQuestionMessages")
            else:
                # Mensajes con respuesta de la pregunta
                self.queueResponseMessages.put(message)
                self.logBook.writeToLogEvent("SpanningTree:\tA message has been placed in queueResponseMessages")


    # Function:  It generates and places an invalid message in queueResponseMessages. It does it when the timer is set so, it knows if the 
    #          adyacent node that is in the SpanningTree responded in the time set.
    # Modifies: This method modifies queueResponseMessages, by placing a new element in it.
    # Requirements: This method only requires the queueResponseMessages to be initialized.
    # Return:   This function does not return anything.
    def waitToAnswer(self):
        message = SpanningTreeQuestion(self.MAX_INT8, 0, 0)
        self.queueResponseMessages.put(message)
        self.logBook.writeToLogEvent("SpanningTree:\tAn invalid message has been placed in queueQuestionMessages")


    # Function:  It generates and places an invalid message in queueQuestionMessages. It does it when the timer is set so, it knows if the
    #           adyacent node responded in the time set.
    # Modifies: This method modifies queueQuestionMessages, by placing a new element in it.
    # Requirements: This method only requires the queueQuestionAnswer to be initialized.
    # Return:   This function does not return anything.
    def waitToConfirmation(self):
        message = SpanningTreeQuestion(self.MAX_INT8, 0, 0)
        self.queueQuestionMessages.put(message)
        self.logBook.writeToLogEvent("SpanningTree:\tAn invalid message has been placed in queueQuestionMessages")


    # Function: Creates this node's Spanning Tree.
    # Modifies: It sets the Spanning Tree, which is the main function of this class.
    # Requirements: It requires the attributes corresponding to myInfo and listOfAdyacents to be assigned.
    # Return: this function does not return anything.
    def sendSpanningTree(self):

        myDijkstraInfo = MessageDijkstra()
        myDijkstraInfo.nodeSrc = self.myInfo.nodeNum

        myDijkstraInfo.numAdyNodes = len(self.adyListToSend)

        self.routing.setAdyList(self.adyListToSend)
        myDijkstraInfo.pairAdyDistance = (AdjacentListInfo * 256)(*self.adyListToSend)
        self.logBook.writeToLogEvent("SpanningTree:\tThe SpanningTree has been created successfully")

        self.routing.pushMessageToRouting(myDijkstraInfo)
        self.logBook.writeToLogEvent("SpanningTree:\tThe djikstra structure in SpanningTree has been sent to the Routing functionality")


    # Function: The node asks its adjacent nodes (that are alive) if they are in the Spanning Tree. when it finds a node that is in the tree, it asks
    #          it if this node could join the Spanning Tree, in case its answer is yes, this node sends a confirmation that it will
    #          join the Spanning Tree via this adjacent node, if the answer is no (it didn't receive the confirmation in the time set)
    #          then it will continue asking its other neighbours until it joins the Spanning Tree.
    #          If this node's num is 1, then it will join the Spanning Tree instantly.
    # Modifies: It modifies the queueResponseMessages by popping messages from it, it modifies the Spanning Tree, because once it's able to join it, the 
    #           Spanning tree will grow in size, and it also modifies its attribute isInTree, because once it joins the tree, it will be set to True
    # Requirements: It requires the queues to be initialized.
    # Return:   This function does not return anything.
    def askToJoinSpanning(self, continueLoop):
        continueAsking = True
        if self.myInfo.nodeNum == 1:
            self.logBook.writeToLogEvent("SpanningTree:\tThe node 1 is in the Spanning Tree")
            self.isInTree = True
            continueAsking = False
        while continueAsking == True and self.continueLoop == True:
            for ady in self.listOfAdyacents:
                if continueAsking == True and self.continueLoop == True:
                    if ady[1] == self.ALIVE:
                        localSecNum = random.randint(1, self.MAX_INT16)
                        self.logBook.writeToLogEvent(f"SpanningTree:\tNode {self.myInfo.nodeNum} send a question to node {ady[0].nodeNum}")
                        messageSpanningTree = SpanningTreeQuestion(self.CAN_JOIN_QUESTION, self.myInfo.nodeNum, ady[0].nodeNum, localSecNum, 0)
                        self.agentPinkSender.pushMessage(messageSpanningTree, protocolPinkToGreen.protocolSpanningQuestion.value)

                        timerToWaitAnswer = threading.Timer(3.0, self.waitToAnswer)
                        timerToWaitAnswer.start()
                        continueWaiting = True
                        while continueWaiting == True and continueAsking == True and self.continueLoop == True:
                            
                            messageRecv = self.queueResponseMessages.get()
                            if messageRecv.question != self.MAX_INT8:
                                if messageRecv.nodeDest == self.myInfo.nodeNum and messageRecv.RN == ((localSecNum + 1) % (2**16)):
                                    self.logBook.writeToLogEvent(f"SpanningTree:\tA answer from {messageRecv.nodeSrc} received")
                                    timerToWaitAnswer.cancel()
                                    continueWaiting = False

                                    if messageRecv.question == self.JOIN:
                                        if (self.isInTree == True):
                                            pass
                                        else:
                                            self.logBook.writeToLogEvent(f"SpanningTree:\tNode {self.myInfo.nodeNum} send the confirmation to join to {messageRecv.nodeSrc}")
                                            messageRecv.nodeDest = messageRecv.nodeSrc
                                            messageRecv.nodeSrc = self.myInfo.nodeNum
                                            messageRecv.question = self.STILL_JOIN
                                            messageRecv.SN = messageRecv.RN
                                            self.isInTree = True
                                            continueAsking = False
                                            self.agentPinkSender.pushMessage(messageRecv, protocolPinkToGreen.protocolSpanningQuestion.value)
                                            
                                            ady[2] = self.IN_TREE
                                            self.listNodesInSpanning.append(messageRecv.nodeDest)
                                            self.logBook.writeToLogEvent(f"SpanningTree:\tNode {messageRecv.nodeSrc} is now in the spanning tree")
                                        
                                            self.agentPinkSender.pushMessage(self.listNodesInSpanning, protocolPinkToGreen.protocolBroadcastTable.value)
                                            self.statisticsControlPlane.updateNodesInSpanningTree(self.listNodesInSpanning)
                                            self.sendSpanningTree()
                            else:
                                self.logBook.writeToLogEvent(f"SpanningTree:\tNo question received")
                                continueWaiting = False
        self.logBook.writeToLogEvent(f"SpanningTree:\tNode {self.myInfo.nodeNum} has managed to become part of the Spanning Tree")

   
    # Function:  The node receives questions from its adjacent nodes of they can join the SpanningTree. If this node is already part of the Spanning Tree, then
    #           it will respond to them with a yes and set a timer so it receives an answer in the time set (the third part of the three way handshake). If the 
    #           adjacent node responds in time, then it will add him to self.listNodesInSpanning and send this new Spanning Tree to its other neighbours that are
    #           part of the Spanning Tree to let them know that it has been updated.
    # Modifies: It modifies the SpanningTree by adding new members to it if they are able to join through me, it also modifies the queues by popping messages from them. 
    # Requirements: It requires the queues to be initialized.
    # Return:   This function does not return anything.   
    def responseToJoinSpanning(self,continueLoop):
        while self.continueLoop:
            messageRecv = self.queueQuestionMessages.get()
            self.logBook.writeToLogEvent("SpanningTree:\tA message from queueQuestionMessages has been popped")
            if messageRecv.question == self.MAX_INT8:
                self.continueLoop = False

                self.logBook.writeToLogEvent(f"SpanningTree:\tNode {self.myInfo.nodeNum} will stop receiving questions from other nodes to join the spanning tree")
            elif messageRecv.nodeDest == self.myInfo.nodeNum:
                respondMessage = SpanningTreeQuestion(self.JOIN, self.myInfo.nodeNum, messageRecv.nodeSrc, messageRecv.SN, (messageRecv.SN+1) % (2**16))
                
                if self.isInTree == True:
                    respondMessage.question = self.JOIN
                    lastRNRecv = respondMessage.RN
                    
                    self.logBook.writeToLogEvent(f"SpanningTree:\tNode {self.myInfo.nodeNum} has received a request from an adjacent node to join the spanning tree and since i'm in it, I will allow it")
                    
                    self.agentPinkSender.pushMessage(respondMessage, protocolPinkToGreen.protocolSpanningQuestion.value)

                    ady = [adyNode for adyNode in self.listOfAdyacents if adyNode[0].nodeNum == messageRecv.nodeSrc][0]
                    ady[2] = self.PARTIALLY_IN_TREE
                    
                    self.logBook.writeToLogEvent(f"SpanningTree:\tNode {self.myInfo.nodeNum} has set my adjacents info to PARTIALLY IN TREE")
                    # esperar confirmacion
                    timerToWaitConfirmation = threading.Timer(3.0, self.waitToConfirmation)
                    timerToWaitConfirmation.start()
                    continueWaiting = True
                    while continueWaiting == True:
                        message = self.queueQuestionMessages.get()
                        if message.question == self.MAX_INT8:
                            continueWaiting = False
                            ady[2] = self.NOT_IN_TREE
                        elif message.nodeDest == self.myInfo.nodeNum and message.question == self.STILL_JOIN and message.SN == lastRNRecv:
                            continueWaiting = False
                            timerToWaitConfirmation.cancel()
                            ady[2] = self.IN_TREE

                            self.listNodesInSpanning.append(message.nodeSrc)
                            self.logBook.writeToLogEvent(f"SpanningTree:\tA new node has been added to the spanning tree")

                            self.agentPinkSender.pushMessage(self.listNodesInSpanning, protocolPinkToGreen.protocolBroadcastTable.value)
                            self.statisticsControlPlane.updateNodesInSpanningTree(self.listNodesInSpanning)
                            self.logBook.writeToLogEvent(f"SpanningTree:\tNode {self.myInfo.nodeNum} is sending the new broadcast table to the broadcast functionality")

                            self.sendSpanningTree()
                            self.logBook.writeToLogEvent(f"SpanningTree:\tNode {self.myInfo.nodeNum} is sending the updated spanning tree to its adjacent nodes currently in the Spanning Tree")
                
                else:
                    respondMessage.question = self.CANT_JOIN
                    self.agentPinkSender.pushMessage(respondMessage, protocolPinkToGreen.protocolSpanningQuestion.value)
                    self.logBook.writeToLogWarning(f"SpanningTree:\tNode {self.myInfo.nodeNum} wont accept the request to join the spanning tree.(The node itself isn't in it)")
                

    # Function: It creates three diferent threads, one for each of the main methods of this class, which are askToJoinSpanning, responseToJoinSpanning and the dispatcher.
    # Modifies: It doesnt modify anything, it just creates threads.
    # Requirements: It requires the variable continueLoop, which is shared between various threads. 
    # Return: this function does not return anything.     
    def execute(self, continueLoop):

        self.eventReadyToStart.wait()
        self.eventReadyToStart.clear()
        
        threadAskToJoinSpanning = threading.Thread(target=self.askToJoinSpanning,args=(continueLoop,))
        threadResponseToJoinSpanning = threading.Thread(target=self.responseToJoinSpanning,args=(continueLoop,))
        threadDispatcher = threading.Thread(target=self.dispatcherMessages, args=(continueLoop,))
        self.logBook.writeToLogEvent("SpanningTree:\tThe threads used in this functionality have been created")

        threadDispatcher.start()
        threadAskToJoinSpanning.start()
        threadResponseToJoinSpanning.start()

        threadResponseToJoinSpanning.join()
        threadAskToJoinSpanning.join()
        threadDispatcher.join()
        
    # Function: Receives spanning tree messages from the AgentPinkReceiver, to be processed.
    # Modifies: Adds an element to the queue of messages to be processed in createSpanningTree
    # Requirements: It requires a broadcast message, which it will add to the queue.
    # Return: this function does not return anything.
    def pushMessageForSpanningTree(self, message):
        self.queueOfMessage.put(message)
        self.logBook.writeToLogEvent("SpanningTree:\tA message has been added to the queue that belongs to Spanning Tree")


    #Function: This method sets which of this node's neighbours have been awakened.
    #Modifies: It modifies listOfAdyacents, once it finds an adjacent node that has been awakened, it sets it to the state ALIVE
    #Requirements: It requires the nodeNum of the node to be set to the state ALIVE
    #Return:   This function does not return anything.
    def setAlive(self, nodeNum):
        for adyInfo in self.listOfAdyacents:
            if adyInfo[0].nodeNum == nodeNum:
                adyInfo[1] = self.ALIVE
                self.logBook.writeToLogEvent(f"SpanningTree:\t The node {nodeNum} has been set to the ALIVE status")
