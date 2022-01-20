from Packets import *
from ctypes import *
import queue
import os
import sys
import datetime
import time
import struct


class AgentPinkReceiver:
    MAX_INT = 4294967295
    MAX_INT16 = 65535 
    MAX_INT8 = 255

    SENDER = 1
    RECEIVER = 2

    def __init__(self, parSpanningTree, parHeartBeat, parRouting, parLogBook, parStatisticsControlPlane, parMultiplexor, parSandBox):        
        self.spanningTree = parSpanningTree
        self.heartBeat = parHeartBeat
        self.routing = parRouting
        self.logBook = parLogBook
        self.statisticsControlPlane = parStatisticsControlPlane
        self.multiplexor = parMultiplexor
        self.sandbox = parSandBox

    #Function: to receive messages coming from the pink node and, depending on their protocol, places the messages in their corresponding queues. 
    #Modifies: constantly reading from the pipe and sending that information to the corresponding queues. 
    #Requirements: continueLoop, which is an int pointer, which determines when the program stops executing, and pathPipeGreenToPink, which is the name of the pipe used as a method of communication.
    #Return: this function does not return anything. 
    def execute(self, continueLoop, pathPipeGreenToPink):
        self.logBook.writeToLogEvent("AgentPinkReceiver:\tBegun its execution")
        pipeGreenToPink = os.open(pathPipeGreenToPink, os.O_RDONLY)
        if(pipeGreenToPink < 0):
            print("Error opening pipe to send data to green node")
            return

        self.logBook.writeToLogEvent("AgentPinkReceiver:\tSuccessfully opened pipe")
        
        # Receive message of green node from pipe.
        # call the proper push depending on the protocol of the packet
        while continueLoop:
            try:
                recv = os.read(pipeGreenToPink, sizeof(c_int8))
                protocol = int.from_bytes(recv, "big")
                # print(f"recibo protocolo {protocol}")
                if protocol == protocolGreenToPink.protocolStopAndWait.value:
                    self.logBook.writeToLogEvent(f"AgentPinkReceiver:\tReceived a message with protocol {protocol}")
                
                if protocol == protocolGreenToPink.protocolGreenInfo.value:
                    self.logBook.writeToLogEvent(f"AgentPinkReceiver:\tReceived my green node's information")
                    
                    msgRecv = os.read(pipeGreenToPink, sizeof(NodeInfo))
                    nodeGreen = NodeInfo.from_buffer_copy(msgRecv)                

                    # calls every respective set so all of the pink node's funcionalities have the green node's info

                    self.spanningTree.setNodeInfo(nodeGreen)
                    self.heartBeat.setNodeInfo(nodeGreen)
                    self.routing.setNodeInfo(nodeGreen)
                    self.multiplexor.setMultiplexorInfo(nodeGreen)
                    self.sandbox.setNodeInfo(nodeGreen.nodeNum)


                elif protocol == protocolGreenToPink.protocolAdyacentsInfo.value:
                    self.logBook.writeToLogEvent(f"AgentPinkReceiver:\tThe message contained my green node's adjacent nodes information")
                    
                    listAdy = []
                    continueReading = True
                    while continueReading:  
                        msgRecv = os.read(pipeGreenToPink, sizeof(NodeInfo))
                        nodeGreen = NodeInfo.from_buffer_copy(msgRecv)            
                        if nodeGreen.portUDP == self.MAX_INT:
                            continueReading = False
                        else:
                            listAdy.append(nodeGreen)
                    # calls the spanning tree's and heart beat's set so it knows all of the adjacent nodes' info
                    
                    self.spanningTree.setAdyInfo(listAdy)
                    self.heartBeat.setAdyInfo(listAdy)       
                elif protocol == protocolGreenToPink.protocolSpanningTree.value:
                    self.logBook.writeToLogEvent("AgentPinkReceiver:\tThe message contained a spanning tree question and will be pushed into its queue")
                    
                    msgRecv = os.read(pipeGreenToPink, sizeof(SpanningTreeQuestion))
                    spanningTreeMessage = SpanningTreeQuestion.from_buffer_copy(msgRecv)
                    self.spanningTree.pushMessageForSpanningTree(spanningTreeMessage)  

                elif protocol == protocolGreenToPink.protocolDjikstra.value:
                    self.logBook.writeToLogEvent("AgentPinkReceiver:\tThe message contained information regarding djikstra's algorithm and will be pushed into its queue")

                    msgRecv = os.read(pipeGreenToPink, sizeof(MessageDijkstra))
                    dijkstraMessage = MessageDijkstra.from_buffer_copy(msgRecv)

                    self.routing.pushMessageToRouting(dijkstraMessage)   
                    
                elif protocol == protocolGreenToPink.protocolHeartBeat.value:
                    self.logBook.writeToLogEvent("AgentPinkReceiver:\tThe message contained a heartbeat question and will be pushed into its queue")

                    msgRecv = os.read(pipeGreenToPink, sizeof(HeartBeatQuestion))
                    heartBeatMessage = HeartBeatQuestion.from_buffer_copy(msgRecv)
                    self.heartBeat.pushMessageToHeartBeat(heartBeatMessage) 
                elif protocol == protocolGreenToPink.protocolStopAndWait.value:
                    #print("AgentPintReceiver:\tme llega un mensaje de Stop&Wait de otro nodo y lo meto en Stop&Wait.")
                    msgRecv = os.read(pipeGreenToPink, sizeof(MessageStopAndWait))
                    # msgStopAndWait = struct.unpack('HH6BH2048s',msgRecv )
                    msgStopAndWait = msgRecv
                    #print(f"message in the receiver is {msgStopAndWait[12:]}")
                    # msgStopAndWait = MessageStopAndWait.from_buffer_copy(msgRecv)
                    #self.stopAndWait.pushNewPacket(msgStopAndWait)
                    msg = (3, msgStopAndWait)
                    self.multiplexor.pushNewPacket(msg)
                    #print("\tLLEGA NUEVO MENSAJE DE STOP AND WAIT")
                elif protocol == protocolGreenToPink.protocolPacketsFromSrcToSrc.value:
                    self.logBook.writeToLogEvent("AgentPinkReceiver:\tThe packet contained statistics about the counter of packets from one node to another")
                    
                    listStatisticsPacketsRecvFrom = []
                    continueReading = True
                    while continueReading:
                        dataRecv = os.read(pipeGreenToPink, sizeof(PairOfStatisticsElements))
                        pair = PairOfStatisticsElements.from_buffer_copy(dataRecv)
                        if pair.node == self.MAX_INT16:
                            continueReading = False
                        else:
                            listStatisticsPacketsRecvFrom.append((pair.node, pair.counter))
                    
                    self.statisticsControlPlane.pushResponseToRequest(listStatisticsPacketsRecvFrom)

                elif protocol == protocolGreenToPink.protocolPacketsSentToNeighbor.value:
                    self.logBook.writeToLogEvent("AgentPinkReceiver:\tThe packet contained statistics about the counter of packets sent by each of my neighbors")
                    
                    listStatisticsPacketsSentTo = []
                    continueReading = True
                    while continueReading:
                        dataRecv = os.read(pipeGreenToPink, sizeof(PairOfStatisticsElements))
                        pair = PairOfStatisticsElements.from_buffer_copy(dataRecv)
                        if pair.node == self.MAX_INT16:
                            continueReading = False
                        else:
                            listStatisticsPacketsSentTo.append((pair.node, pair.counter))
                    
                    self.statisticsControlPlane.pushResponseToRequest(listStatisticsPacketsSentTo)
                elif protocol == protocolGreenToPink.protocolPacketsRecvFromNeighbor.value:
                    self.logBook.writeToLogEvent("AgentPinkReceiver:\tThe packet contained statistics about the counter of packets received from nodes by whicj I'm neighbor")
                    
                    listStatisticsPacketsRecvFrom = []
                    continueReading = True
                    while continueReading:
                        dataRecv = os.read(pipeGreenToPink, sizeof(PairOfStatisticsElements))
                        pair = PairOfStatisticsElements.from_buffer_copy(dataRecv)
                        if pair.node == self.MAX_INT16:
                            continueReading = False
                        else:
                            listStatisticsPacketsRecvFrom.append((pair.node, pair.counter))
                    
                    self.statisticsControlPlane.pushResponseToRequest(listStatisticsPacketsRecvFrom)
                elif protocol == protocolGreenToPink.protocolPacketsForwardingBroadcast.value:
                    self.logBook.writeToLogEvent("AgentPinkReceiver:\tThe packet contained statistics about the counter of forwarding and broadcast packets")
                    
                    dataRecv = os.read(pipeGreenToPink, sizeof(PairOfStatisticsElements))
                    counterPackets = PairOfStatisticsElements.from_buffer_copy(dataRecv)
                    self.statisticsControlPlane.pushResponseToRequest((counterPackets.node, counterPackets.counter))

                elif protocol == protocolGreenToPink.protocolSandboxFromBlue.value:
                    self.logBook.writeToLogEvent("AgentPinkReceiver:\tThe sandbox packet will be sent to sandbox")
                    #print(sizeof(MessageToSandbox))
                    msgRecv = os.read(pipeGreenToPink, sizeof(MessageToSandbox))
                    msgSandbox = MessageToSandbox.from_buffer_copy(msgRecv)
                    #print(f"Estoy en agent pink receiver, luggage es: {msgSandbox.luggagePath}")
                    self.sandbox.pushMessageFromBlue(msgSandbox.luggagePath)

                else:
                    self.logBook.writeToLogEvent("AgentPinkReceiver:\tStopping its execution")
                    continueLoop = False
                    msgR = ""
                    msgNull = (self.MAX_INT16, struct.pack("!BBHBH4089s",0,0,0,0,0,"".encode("utf-8")), 0 , 0)
                    #self.stopAndWait.pushNewPacket(msgNull)
                    self.multiplexor.pushNewPacket(msgNull)
                    msgNull = MessageDijkstra(self.MAX_INT16, self.MAX_INT16, (AdjacentListInfo * 256)())
                    self.routing.pushMessageToRouting(msgNull)
                    msgNull = HeartBeatQuestion(self.MAX_INT16,self.MAX_INT16,self.MAX_INT8,self.MAX_INT16,self.MAX_INT16)
                    self.heartBeat.pushMessageToHeartBeat(msgNull)
                    msgNull = SpanningTreeQuestion(self.MAX_INT8,self.MAX_INT16,self.MAX_INT16)
                    self.spanningTree.pushMessageForSpanningTree(msgNull)
                    msgNull = ""
                    self.sandbox.pushMessageFromBlue(msgNull)                    
                    msgNull = (self.MAX_INT16, self.MAX_INT8)
                    self.statisticsControlPlane.pushRequest(msgNull)
            except BrokenPipeError:
                continueLoop = False

