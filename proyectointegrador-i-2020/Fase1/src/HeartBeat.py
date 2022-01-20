from SpanningTree import SpanningTree
from LogBook import LogBook 
from Packets import *
import os, threading, sys, time, queue, random

# Author:           Luis Andres Sanchez, Jose Andres Viquez, Daniela Vargas, Fernando Morales
# Definition:       This is the class in charge of the HeartBeat protocol, which is used to send the 
#                   question of spanning tree only to the green nodes that are active in the network.
class HeartBeat:

    TRIES = 5
    IS_ALIVE = 0
    IM_ALIVE = 1
    DEAD = 2
    ALIVE = 3
    INVALID_MESSAGE_FOR_TIMER = 0
    MAX_INT16 = 65535

    # Function:     It initiates the object and its attributes. 
    # Modifies:     It sets some of the attributes of this object to a valid state. It also sets the queues. 
    # Requirements: It requires logBook, agent pink sender and spanning tree objects.
    # Return:       This function does not return anything. 
    def __init__(self, logBook, agentPinkSender, spanningTree):
        self.statusOfAdyacents = []
        self.listAdyacents = []
        self.queueMessagesForHeartBeat = queue.Queue()
        self.agentPinkSender = agentPinkSender
        self.spanningTree = spanningTree
        self.logBook = logBook
        self.nodeID = NodeInfo()
        self.queueForResponses = queue.Queue()
        self.queueForQuestions = queue.Queue()
        self.continueLoop = True
        self.eventReadyToStart = threading.Event()

    def __delattr__(self):
        self.statusOfAdyacents = []
        self.queueMessagesForHeartBeat = []


    # Function:     This function is used to notify that the timer ends.
    # Modifies:     Enqueue an invalid message in the queue of responses to notify that the timer end and no message were received.
    # Requirements: Does not requires any parameters.
    # Return:       Does not return anything.
    def queue_invalid_answer(self):
        invalid_message = HeartBeatQuestion(self.INVALID_MESSAGE_FOR_TIMER, self.INVALID_MESSAGE_FOR_TIMER, self.INVALID_MESSAGE_FOR_TIMER, self.INVALID_MESSAGE_FOR_TIMER, self.INVALID_MESSAGE_FOR_TIMER)
        self.queueForResponses.put(invalid_message)
        self.logBook.writeToLogWarning("Heartbeat:\t\tTimer to receive answer run out")
        return


    # Function:     This function is used has a dispatcher of the messages incoming to the HeartBeat protocol. It enqueue the
    #               message to the queueForQuestions if the message received is a question of are you alive?. Otherwise, if the 
    #               message is about a response about a response to the question of live, enqueue the message in the 
    #               queueForResponses. In both cases, there are one thread in charge of process the message.
    # Modify:       Only enqueue the message to the correspondent queue to be process later.
    # Requirements: A boolean indicating if the process is still alive.
    # Return:       Does not modify anything
    def recvMessageToHeartBeat(self,continueLoop):
        while continueLoop:
            # receive packets directed to HeartBeat and send to the proper mechanism to process it
            message = self.queueMessagesForHeartBeat.get()
            self.logBook.writeToLogEvent("Heartbeat:\t\tA message for HeartBeat functionality received")
            if message.nodeDest == self.MAX_INT16 and message.nodeSrc == self.MAX_INT16:
                # Used to end the functionality
                self.logBook.writeToLogEvent("Heartbeat:\t\tThe HeartBeat functionality is going to be finished")
                continueLoop = False
                self.queueForQuestions.put(message)
                self.continueLoop = False
                self.queueForResponses.put(message)
            else:
                if message.code is self.IS_ALIVE:
                    self.logBook.writeToLogEvent("Heartbeat:\t\tThe message received is a question of life")
                    # if it is asking the question isAlive, send to the process responseQuestionOfLife
                    self.queueForQuestions.put(message)
                else:
                    self.logBook.writeToLogEvent("Heartbeat:\t\tThe message received is a response of life")
                    # if it responding the question isAlive, send to askForLifeOfAdyacent
                    self.queueForResponses.put(message)


    # Function:     Constantly checks the queue of response messages to set whether a adjacent node is alive or not
    # Modifies:     queue "queueForQuestions"
    # Requirements: the "continueLoop" variable, that defines whether the program is still running, or it should end 
    #               its execution
    # Return:       void
    def responseQuestionOfLife(self,continueLoop):
        # get the packet, change the RN and resend to message to the source of the packet
        while continueLoop:
            messageRecv = self.queueForQuestions.get()
            if messageRecv.nodeDest == self.MAX_INT16 and messageRecv.nodeSrc == self.MAX_INT16:
                continueLoop = False
            else:
                messageRecv.nodeDest = messageRecv.nodeSrc
                messageRecv.nodeSrc = self.nodeID.nodeNum
                messageRecv.resNum = messageRecv.secNum
                messageRecv.code = self.IM_ALIVE
                self.logBook.writeToLogEvent("Heartbeat:\t\tA question of life answered")
                self.agentPinkSender.pushMessage(messageRecv, protocolPinkToGreen.protocolHeartBeatQuestion.value)               


    # Function: This function constantly iterates through every adjacent of the node and sends a message to each adjacent asking 
    #           if they are alive. It sets a timer to wait for a response. If the adjacent node sent a response of "IM ALIVE", 
    #           the timer is cancelled and the respective node is set as alive. Otherwise, if the timer ends, an invalid response 
    #           is added to the queue of responses, and then discarded. While the response from the adyacent node hasnt been received,
    #           this process of asking and waiting for a response is repeated "TRIES" times (5 times), before the function moves on 
    #           to another adjacent node. 
    # Modifies: queue "queueForResponses" and list "statusOfAdyacents"
    # Requirements: The "continueLoop" variable, that defines whether the program is still running, or it should end its execution
    # Return: 
    def askForLifeOfAdyacent(self,continueLoop):
        # Send the packets to see if the adyacent node is alive
        # Wait to see responses
        # If it have a response, mark the adyacent has life.
        # Go to the next adyacent to ask
        continueAsking = True
        counterNeighborAlives = 0
        while (self.continueLoop and continueAsking):
            for ady in self.statusOfAdyacents:
                if continueAsking and self.continueLoop and ady[1] is self.DEAD:
                    tries = self.TRIES
                    self.logBook.writeToLogEvent(f"Heartbeat:\t\tA question of life is going to be send to the adjacent node {ady[0]}")
                    messageHeartBeat = HeartBeatQuestion()
                    messageHeartBeat.nodeSrc = self.nodeID.nodeNum
                    messageHeartBeat.nodeDest = ady[0]
                    messageHeartBeat.code = self.IS_ALIVE
                    localSecNum = random.randint(1, 32768)
                    messageHeartBeat.secNum = localSecNum
                    messageHeartBeat.resNum = 0
                    while tries != 0 and ady[1] is self.DEAD and self.continueLoop == True:
                        self.logBook.writeToLogEvent(f"Heartbeat:\t\tThe tried {tries} is going to be sent to {ady[0]}")

                        self.agentPinkSender.pushMessage(messageHeartBeat, protocolPinkToGreen.protocolHeartBeatQuestion.value)

                        timerToWaitAnswer = threading.Timer(3.0, self.queue_invalid_answer)
                        
                        timerToWaitAnswer.start()
                        
                        continueWaiting = True
                        while continueWaiting == True and continueAsking == True and self.continueLoop == True:

                            messageRecv = self.queueForResponses.get()
                            if messageRecv.nodeDest == self.nodeID.nodeNum and messageRecv.code == self.IM_ALIVE:
                                if messageRecv.resNum == localSecNum:
                                    self.logBook.writeToLogEvent(f"Heartbeat:\t\tThe question has been answered. Node {ady[0]} is alive")
                                    timerToWaitAnswer.cancel()
                                    ady[1] = self.ALIVE
                                    self.spanningTree.setAlive(ady[0])
                                    counterNeighborAlives += 1
                                    continueWaiting = False
                            elif (messageRecv.nodeSrc == self.INVALID_MESSAGE_FOR_TIMER) and (messageRecv.nodeDest == self.INVALID_MESSAGE_FOR_TIMER):
                                self.logBook.writeToLogEvent("Heartbeat:\t\tThe question has not been answered")
                                continueWaiting = False
                                tries -= 1
                            else:
                                continueLoop = False

                            if counterNeighborAlives == len(self.statusOfAdyacents):
                                self.logBook.writeToLogEvent("Heartbeat:\t\tAll the neighbors are alive")
                                continueAsking = False  
     
     
    # Function:     This is the main function in charge of all the functionality of the heartBeat protocol. In case
    #               of call it, it wait until the identity is established to create three threads. One thread used
    #               as dispatcher of the messages, other in charge of asking the adjacent nodes if they are alive, 
    #               and the last one in charge of response question of life. 
    # Modifies:     Create three threads, that each one modify the specific queues of messages.
    # Requirements: A boolean value indicating if the node is still alive.
    # Return:       Does not return anything.
    def execute(self, continueLoop):
        
        # Three threads
        # One with recvMessageToHeartBeat
        # Other with responseQuestionOfLife
        # Other with askForLifeOfAdyacent

        self.eventReadyToStart.wait()
        self.eventReadyToStart.clear()

        for ady in self.listAdyacents:
            adyStatus = [ady.nodeNum, self.DEAD]
            self.statusOfAdyacents.append(adyStatus)
        
        
        self.logBook.writeToLogEvent("Heartbeat:\t\tThe heart beat functionality has started")
        threadAskForLifeOfAdyacent = threading.Thread(target=self.askForLifeOfAdyacent,args=(continueLoop,))           
        threadRecvMessageToHeartBeat = threading.Thread(target=self.recvMessageToHeartBeat,args=(continueLoop,))           
        threadResponseQuestionOfLife = threading.Thread(target=self.responseQuestionOfLife,args=(continueLoop,))           


        threadAskForLifeOfAdyacent.start()
        threadResponseQuestionOfLife.start()
        threadRecvMessageToHeartBeat.start()

        threadRecvMessageToHeartBeat.join()
        threadResponseQuestionOfLife.join()
        threadAskForLifeOfAdyacent.join()
        self.logBook.writeToLogEvent("Heartbeat:\t\tThe heart beat functionality has finished")
        return
    

    # Function:     This function add a message to be processed by the dispatcher thread.
    # Modifies:     The attribute of the class queue, in which a message is going to be added.
    # Requirements: The message that is going to be added to the queue.
    # Return:       Does not return anything
    def pushMessageToHeartBeat(self, message):
        self.logBook.writeToLogEvent("Heartbeat:\t\tA new message is enqueue to the queue of messages to be process later")
        self.queueMessagesForHeartBeat.put(message)


    # Function:     This function set the identity of the node.
    # Modifies:     The attribute of the class nodeID, in which the identity established in parameters is going to be assigned.
    # Requirements: The identity of the green node.
    # Return:       Does not return anything
    def setNodeInfo(self, nodeGreen):
        self.logBook.writeToLogEvent("Heartbeat:\t\tThe information of the green node has been established")
        self.nodeID = nodeGreen


    # Function:     This function set the identity of the adjacent nodes.
    # Modifies:     The attribute of the class listAdyacents, in which the identity of all the adjacents nodes established in parameters is going to be assigned.
    # Requirements: The identity of the all the adjacent nodes.
    # Return:       Does not return anything
    def setAdyInfo(self, listAdyacents):
        self.logBook.writeToLogEvent("Heartbeat:\t\tThe information of the adjacent nodes has been established")
        self.listAdyacents = listAdyacents
        self.eventReadyToStart.set()