from enum import Enum
from Packets import *
import queue, struct
import os #struct
import time, threading
from ctypes import *


# Class in charge of sending the message of the diferents functionalities of the pink node to its green 
# Author: Jose Andres Viquez
class AgentPinkSender:

    MAX_INT = 4294967295
    MAX_INT16 = 65535
    MAX_INT8 = 255
    TIMER = 1.0


    def __init__(self, parLogBook):
         
        # Constructor of the AgentPinkSender class 
        # Requirements: The log book to write in it the possible events.
        # Return: Does not return anything
        self.queueMessage = queue.Queue()
        self.logBook = parLogBook
        self.eventNewMessageToSend = threading.Event()
    

    # Function:     Main function in charge of sending messages from the pink node to its green node by the pipe
    # Requirements: A integer indicating the activity of the pink node, and the path of the pipe in which messages 
    # are going to be send to its green node.
    # Modify:       Does not modify anything
    # Return:       Does not return anything
    def execute(self, continueLoop, pathPipePinkToGreen):
        self.logBook.writeToLogEvent("AgentPinkSender:\tBegun its execution")

        pipePinkToGreen = os.open(pathPipePinkToGreen, os.O_WRONLY)
        if pipePinkToGreen < 0 : 
            print("Error opening pipe to send data to green node")
            return

        self.logBook.writeToLogEvent("AgentPinkSender:\tSuccessfully opened pipe")

        while continueLoop:
            try:
                protocol = self.queueMessage.get()
                #print(f"protocolo es {protocol}")
                protocol_packet = struct.pack("!B", protocol)
                os.write(pipePinkToGreen, protocol_packet)
                #print(f"write {} bytes")
                self.queueMessage.task_done()

                self.logBook.writeToLogEvent(f"AgentPinkSender:\tRead a message from my queue with protocol {protocol}")
        
                if protocol is protocolPinkToGreen.protocolForwarding.value:
                    self.logBook.writeToLogEvent(f"AgentPinkSender:\tWrote to pipe forwarding message to the green node's pink layer")
                    listAdy = self.queueMessage.get()
                    for ady in listAdy:
                        os.write(pipePinkToGreen, MessageForwarding(ady[0], ady[1]))
                    os.write(pipePinkToGreen, MessageForwarding(self.MAX_INT16,self.MAX_INT16))
                    self.queueMessage.task_done()
                elif protocol is protocolPinkToGreen.protocolBroadcastTable.value:
                    self.logBook.writeToLogEvent(f"AgentPinkSender:\tWrote to pipe the new broadcast table to the green node's pink layer")
                    listSpanning = self.queueMessage.get()
                    for nodeInSpanning in listSpanning:
                        os.write(pipePinkToGreen,MessageBroadcastTable(nodeInSpanning))
                    os.write(pipePinkToGreen, MessageBroadcastTable(self.MAX_INT16))
                    self.queueMessage.task_done()
                elif protocol is protocolPinkToGreen.protocolBroadcast.value:
                    self.logBook.writeToLogEvent(f"AgentPinkSender:\tWrote to pipe the new broadcast message to the green node's pink layer")
                    message = self.queueMessage.get()
                    os.write(pipePinkToGreen, message)
                    self.queueMessage.task_done()
                elif protocol is protocolPinkToGreen.protocolSpanningQuestion.value:
                    self.logBook.writeToLogEvent(f"AgentPinkSender:\tWrote to pipe the new spanning tree question to the green node's pink layer")
                    message = self.queueMessage.get()
                    os.write(pipePinkToGreen, SpanningTreeQuestion(message.question, message.nodeSrc, message.nodeDest, message.SN, message.RN))
                    self.queueMessage.task_done()
                elif protocol is protocolPinkToGreen.protocolHeartBeatQuestion.value:
                    self.logBook.writeToLogEvent(f"AgentPinkSender:\tWrote to pipe the new heart beat question to the green node's pink layer")
                    message = self.queueMessage.get()
                    os.write(pipePinkToGreen, HeartBeatQuestion(message.nodeSrc, message.nodeDest, message.code, message.secNum, message.resNum))
                    self.queueMessage.task_done()
                elif protocol is protocolPinkToGreen.protocolStopAndWait.value:
                    message = self.queueMessage.get()
                    #print(f"AgentPinkSender:\tvoy a enviar mensaje a {message.nodeDest} desde {message.nodeSrc} de Stop&Wait")
                    # print(f"El mensaje a enviar en el sender es el {message[12:]}")

                    os.write(pipePinkToGreen, message)
                    self.queueMessage.task_done()
                elif protocol is protocolPinkToGreen.protocolSandboxToBlue.value:
                    message = self.queueMessage.get()
                    os.write(pipePinkToGreen, MessageToSandbox(message.luggagePath))
                    self.queueMessage.task_done()
                elif protocol >= protocolPinkToGreen.protocolPacketsFromSrcToSrc.value and protocol <= protocolPinkToGreen.protocolPacketsForwardingBroadcast.value:
                    #print("Llego aca")
                    request = self.queueMessage.get()
                    #print(f"Request is {request}")
                    self.queueMessage.task_done()
                else:
                    self.logBook.writeToLogEvent(f"AgentPinkSender:\tStopping its execution")
                    continueLoop = False
            except BrokenPipeError:
                self.logBook.writeToLogEvent(f"AgentPinkSender:\tStopping its execution because of broken pipe")
                continueLoop = False

    # If there are, use the pipe to send them

    # Function:     Enqueue a message to be sended to the green node.
    # Requirements: The message to send and the protocol of that message.
    # Modify:       Enqueue a message to the private queue
    # Return:       Does not return anything
    def pushMessage(self, message, protocol):
        self.queueMessage.put(protocol)
        self.queueMessage.put(message)
        self.eventNewMessageToSend.set()