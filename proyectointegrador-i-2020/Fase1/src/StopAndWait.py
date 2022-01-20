from ctypes import *
from Packets import * 
from AgentPinkSender import AgentPinkSender
import time, os ,queue, random,threading, sys, struct

class StopAndWait:
    
    TIMEOUT = 1.0
    # TIMER = 3.0
    TIMER_FOR_LOST_CONNECTION = 600.0
    MAX_TRIES = 10

    MAX_INT = 4294967295
    MAX_INT16 = 65535
    MAX_INT8 = 255
    MAX_COUNT = 5
    SENDER = 1
    RECEIVER = 2
    FIRST_INIT = 3
    SYN_RAISED = 128
    SYN_ACK_RAISED = 192
    ACK_RAISED = 64
    FIN_RAISED = 32
    FIN_ACK_RAISED = 96
    PAYLOAD_SIZE = 4096

    SUCCESS = True
    FAILURE = False

    MESSAGE_FOR_SENDER = 1
    MESSAGE_FOR_RECEIVER = 2
    INVALID_MESSAGE = 3


    #Function: Initialize class and it's attributes
    #Modifies: It initializes every single one of this object's atrributes.
    #Requirements: instance of AgentPinkSender class (senderToGreen), and logBook
    #Returns: void
    #Author: Luis Sanchez
    def __init__(self, parAgentPinkSender, parLogBook, parQueueForSandbox):

        self.agentPinkSender = parAgentPinkSender
        self.logBook = parLogBook
        self.myInfo = NodeInfo(-1,-1,"".encode("utf-8"))
        self.logBook.writeToLogEvent("Stop&Wait: A new Stop&Wait has been initialized.")

        self.messagesQueue = queue.Queue() #Cola de mensajes normales.
        self.ackQueue = queue.Queue()      #Cola de mensajes de confirmacion.
        self.incomingQueue = queue.Queue() #Cola de todos los mensajes.
        self.messagesFromUpperLayer = queue.Queue()
        self.role = 0

        self.SN = 0
        self.RN = 0

        self.closing = False
        self.stoppedReceivingMessages = False

        self.id_1 = 0
        self.id_2 = 0
        self.inUse = True

        self.destIdentifier = 0

        self.nodeInComms = 0
        self.previousRTT = 0
        self.eventReadyToStart = threading.Event()
        self.queueForSandbox = parQueueForSandbox  

    def __delattr__(self):
        self.logBook.writeToLogEvent("Stop&Wait:\t This instance of Stop&Wait will be ended.")


    #Function: It sets this instance's information.
    #Modifies: It modifies the attribute myInfo. 
    #Requirements: It requires the struct NodeInfo as a parameter
    #Returns: void
    #Author: Luis Sanchez
    def setNodeInfo(self, parId):
        self.myInfo = parId
        self.eventReadyToStart.set()


    #Function: It sets this instance's role as a sender or receiver of data.
    #Modifies: It modifies the attribute role. 
    #Requirements: It requires a number as a parameter, which indicates what will this instance of S&W will act as. 
    #Returns: void
    #Author: Luis Sanchez
    def setRole(self, parRole):
        if parRole == self.SENDER:
            self.logBook.writeToLogEvent("Stop&Wait: This instance of Stop&Wait has been assigned a role as a sender.")
            # print("Stop&Wait:\tAsigno rol de emisor")
            self.role = self.SENDER
        elif parRole == self.RECEIVER:
            self.logBook.writeToLogEvent("Stop&Wait: This instance of Stop&Wait has been assigned a role as a receiver.")
            # print("Stop&Wait:\tAsigno rol de receptor")
            self.role = self.RECEIVER
        else:
            self.role = self.FIRST_INIT
            #print("Rol invalido asignado")
#-------------------------------------------------------------------------------------------------------------------------Siguen los metodos de funcion, ya sea Receiver o Sender. 


    #Function: It places a new message in the main queue of the S&W instance. 
    #Modifies: It modifies the queue incomingQueue by placing a new message in it. 
    #Requirements: It requires a message as a parameter, which is the new message to place in the queue.  
    #Returns: void
    #Author: Luis Sanchez
    def pushNewPacket(self, parNewPacket):
        #if(self.incomingQueue.empty()):
        self.logBook.writeToLogEvent("Stop&Wait: This instance of Stop&Wait has received a new packet from the multiplexor.")
        # print(f"me colocan mensaje en la cola con flags {parNewPacket.FLAGS}" )
        self.incomingQueue.put(parNewPacket)
        # else:
        #     print("Stop&Wait:\t NO ESTABA VACIA, MENSAJE NO ENTRA")
        #     message = self.incomingQueue.get()
        #     print(f"MESSAGE = id_1 = {message.ID_1} id_2 = {message.ID_2} SN = {message.SN} RN = {message.RN} FLAGS = {message.FLAGS}")


    #Function: It places a new message in the correct queue, which will depend on this instance's role (sender or receiver). 
    #Modifies: It modifies the queue incomingQueue by removing an element from it and modifies either the ackQueue or messageQueue, depending on the role of the instance. 
    #Requirements: It requires the parameter continueLoop, which will be modified once it receives a message of type  INVALID_MESSAGE
    #Returns: void
    #Author: Luis Sanchez
    def dispatcher(self, parContinueLoop):
        while parContinueLoop:
            #print("Stop&Wait\t\trecibi mensaje en el despachador de Stop&Wait")
            msgFromqueue = self.incomingQueue.get()
            msg_codified = struct.unpack(f"=HH6BH{self.PAYLOAD_SIZE}s", msgFromqueue)
            messageRecv = MessageStopAndWait(msg_codified[0], msg_codified[1], msg_codified[2], msg_codified[3], msg_codified[4], msg_codified[5], msg_codified[6], msg_codified[7], msg_codified[8], msg_codified[9])
            messageType = self.typeOfMessage(messageRecv)
            if  messageType is self.MESSAGE_FOR_SENDER:
                #print(f"Stop&Wait:\t {self.id_1} COLOCO MENSAJE PARA EMISOR")
                self.ackQueue.put(msgFromqueue)
                self.logBook.writeToLogEvent("Stop&Wait: This instance of Stop&Wait has placed the message received in the sender queue (an ACK).")

            elif messageType is self.MESSAGE_FOR_RECEIVER:
                #print(f"Stop&Wait:\t {self.id_2}COLOCO MENSAJE PARA RECEPTOR")
                self.messagesQueue.put(msgFromqueue)
                self.logBook.writeToLogEvent("Stop&Wait: This instance of Stop&Wait has placed the message received in the receiver queue.")


            else:
                self.logBook.writeToLogEvent("Stop&Wait: This instance of Stop&Wait has received a message indicating the finalization of this node.")

                #print("Stop&Wait:\tColoco mensaje de finalizacion forzosa")
                self.ackQueue.put(msgFromqueue)
                self.messagesQueue.put(msgFromqueue)
                parContinueLoop = False



    #Function: It places a new message for the sender from Sandbox to be sent to the receiver.
    #Modifies: It places a new element in the messagesFromUpperLayer queue.
    #Requirements: It requires a packet placed by the multiplexor.
    #Returns: Nothing.
    def pushNewPacketFromUpperLayer(self, parNewPacket):
        self.logBook.writeToLogEvent("Stop&Wait: This instance of Stop&Wait has received a new packet from the upper layer.")
        self.messagesFromUpperLayer.put(parNewPacket)


    #Function: It sets the attribute id_1 or id_2 based on the role of this stop and wait unit, if its role is Sender, then, the id it sets is the 1, otherwise, its the 2.
    #Modifies: It modifies the port number used in the multiplexor (multiplexor unit)
    #Requirements: It requires a numPort provided by the multiplexor.
    #Returns: Nothing.
    def setPort(self, numPort):
        self.logBook.writeToLogEvent("Stop&Wait: This instance of Stop&Wait has been assigned its id.")
        #print("asigno puerto")
        if self.role == self.SENDER:
            self.id_1 = numPort
        else:
            self.id_2 = numPort

    
    #Function: It sets the attribute destIdentifier which indicates the node number of the node in communication whith this one.
    #Modifies: It modifies the destIdentifier to indicate the node in communication with this one.
    #Requirements: It requires a numDest provided by the multiplexor indicating the node in communication with this one.
    #Returns: Nothing.
    def setDest(self, numDest):
        #Author: Luis Sanchez  
        self.logBook.writeToLogEvent("Stop&Wait:v This instance of Stop&Wait has been assigned its destination node.")
        self.destIdentifier = numDest          
      

    #Function: It sets the attribute closing to true, indicating that Sandbox has sent its last message, so it needs to begin the two way close
    #Modifies: Sets the closing attribute to true.
    #Requirements: Nothing.
    #Returns: Nothing.
    #Author: Luis Sanchez  
    def finalizeComs(self):
        self.logBook.writeToLogEvent("Stop&Wait:\t This instance of Stop&Wait will finalize its execution.")
        self.closing = True


    #Function: It clears the information of the Stop and Wait unit once it has been utilized for a secure connection between sandbox units in different nodes.
    #Modifies: Sets every attribute of the class to 0, and empties the queues.
    #Requirements: Nothing.
    #Returns: Nothing.
    #Author: Luis Sanchez
    def clearUnit(self):
        self.logBook.writeToLogEvent("Stop&Wait:\t This instance of Stop&Wait will be cleared so it can be used in another communication channel.")
        # print("limpio unidad")

        self.role = 0
        self.SN = 0
        self.RN = 0
        self.closing = False
        self.stoppedReceivingMessages = False
        self.id_1 = 0
        self.id_2 = 0
        self.destIdentifier = 0
        self.nodeInComms = 0
        self.previousRTT = 0
        #existe la posibilidad de haber paquetes en la cola por los paquetes de finalizacion
        #como que llegue un FIN_ACK e inmediatamente llegue una señal de finalizar ejecucion
        while not self.ackQueue.empty():
            msg = self.ackQueue.get()
        while not self.messagesQueue.empty():
            msg = self.messagesQueue.get()
        while not self.incomingQueue.empty():
            msg = self.incomingQueue.get()


    #Function: It determines whether the message received will be handled by the receiver (and should be placed in the messagesQueue), the sender(and should be placed in the ackQueue).
    #Modifies: Nothing
    #Requirements: It requires an Stop&Wait message as parameter.
    #Returns: It returns an int that determines the type of message received.
    #Author: Luis Sanchez
    def typeOfMessage(self, parReceivedMessage):
            #               Mensaje inicial                                 Mensaje de datos                            Mensaje de finalizacion
            if (parReceivedMessage.FLAGS == self.SYN_ACK_RAISED) or (parReceivedMessage.RN == (self.SN+1)%256) or (parReceivedMessage.FLAGS == self.FIN_ACK_RAISED):
                return self.MESSAGE_FOR_SENDER
            elif (parReceivedMessage.FLAGS == self.SYN_RAISED) or (parReceivedMessage.FLAGS == self.ACK_RAISED) or (parReceivedMessage.SN+1 == self.RN) or (parReceivedMessage.SN == self.RN) or  (parReceivedMessage.FLAGS == self.FIN_RAISED):
                return self.MESSAGE_FOR_RECEIVER
            else:
                return self.INVALID_MESSAGE
   
   
    #Function: It indicates that the other end of the connection has stopped working and this one should stop to make room for another connection in the multiplexor. 
    #Modifies: It modifies the attribute stoppedReceivingMessages
    #Requirements: Nothing.
    #Returns: Void.
    #Author: Luis Sanchez
    def stoppedReceivingMessagesFromOtherEnd(self):
        self.stoppedReceivingMessages = True
        self.logBook.writeToLogEvent(f"Stop&Wait:\t This instance of Stop&Wait hasn't received any new messages coming from {self.destIdentifier} in {self.TIMER_FOR_LOST_CONNECTION} so it will be shut down.")
        if self.role == self.SENDER:
            # print(f"Stop&Wait:\t {self.id_1}El otro nodo tuvo que morir porque no recibi mensajes nuevos en 10 segundos, por lo que voy a habilitar esta instancia de stop and wait")# esto es mas pensando en el multiplexor. 
            pass
        else:
            # print(f"Stop&Wait:\t {self.id_2}El otro nodo tuvo que morir porque no recibi mensajes nuevos en 10 segundos, por lo que voy a habilitar esta instancia de stop and wait")# esto es mas pensando en el multiplexor. 
            pass
        self.forcedClosure()


    #Function: It indicates that the message that was spected to be received wasn't received in the time given.
    #Modifies: It places a new message in the queue. 
    #Requirements: Nothing.
    #Returns: Void.
    #Author: Luis Sanchez
    def isLost(self):
        self.logBook.writeToLogEvent(f"Stop&Wait:\t This instance of Stop&Wait didn't receive the ACK in {self.TIMEOUT}s, so it will try again.")

        if self.role == self.SENDER:
            #print("Stop&Wait\t\tNo llega el mensaje esperado en el Emisor.")
            msg = ""
            # messageTrash = MessageStopAndWait(0,0,0,0,0,0,0,0,0,msg.encode("utf-8"))
            messageTrash = struct.pack(f"=HH6BH{self.PAYLOAD_SIZE}s",0,0,0,0,0,0,0,0,0,msg.encode("utf-8"))
            self.ackQueue.put(messageTrash)

        else:
            #print("Stop&Wait\t\tNo llega el mensaje esperado en el Recibidor.")
            msg = ""
            # messageTrash = MessageStopAndWait(0,0,0,0,0,0,0,0,0,msg.encode("utf-8"))
            messageTrash = struct.pack(f"=HH6BH{self.PAYLOAD_SIZE}s",0,0,0,0,0,0,0,0,0,msg.encode("utf-8"))
            self.messagesQueue.put(messageTrash)


    #Function: It places a message in the queue that indicates that the Stop&Wait instance will stop its execution. 
    #Modifies: It places a new message in the main queue (incomingQueue). 
    #Requirements: Nothing.
    #Returns: Void.
    #Author: Luis Sanchez
    def forcedClosure(self):
        self.logBook.writeToLogEvent("Stop&Wait:\t This instance of Stop&Wait is being shut down.")

        # print("Stop&Wait\tEsta instancia de Stop&Wait va a finalzar su ejecucion.")   
        msgR = ""
        messageTrash = struct.pack(f"=HH6BH{self.PAYLOAD_SIZE}s",self.MAX_INT16, self.MAX_INT16, self.MAX_INT8, self.MAX_INT8, self.MAX_INT8, self.MAX_INT8, self.MAX_INT8, self.MAX_INT8, self.MAX_INT16, msgR.encode("utf-8"))              
        self.incomingQueue.put(messageTrash)


    #Function: It receives messages from the sender and sends them to the upper layer, until it receives a message with the FIN flag raised, in which case, 
    #          it will begin the process of ending the execution correctly. 
    #Modifies: stoppedReceivingMessages, closing, RN.   
    #Requirements: Nothing.
    #Returns: Void.
    #Author: Luis Sanchez
    def receivingMessages(self, parContinueLoop):
        while parContinueLoop and not self.closing:
            if self.stoppedReceivingMessages == True:
                parContinueLoop = False
                break
            timerLostConnection = threading.Timer(self.TIMER_FOR_LOST_CONNECTION, self.stoppedReceivingMessagesFromOtherEnd)#Este es el timer en caso de que el sender se muera cuando estaba comunicandose con él
            self.logBook.writeToLogEvent("Stop&Wait:\t Timer for lost connection starting.")

            timerLostConnection.start()
            msgPacket = self.messagesQueue.get()
            msg_codified = struct.unpack(f'=HH6BH{self.PAYLOAD_SIZE}s',msgPacket)
            messageRecv = MessageStopAndWait(msg_codified[0], msg_codified[1], msg_codified[2], msg_codified[3], msg_codified[4], msg_codified[5], msg_codified[6], msg_codified[7], msg_codified[8], msg_codified[9])

            timerLostConnection.cancel()#Si me llega un mensaje, indica que no ha muerto 
            self.logBook.writeToLogEvent("Stop&Wait:\t Timer for lost connection cancelled.")

            if messageRecv.FLAGS == self.FIN_RAISED:
                self.closing = True
                self.logBook.writeToLogEvent("Stop&Wait:\t A message with the FIN flag raised has been received, so there won't be any more data messages.")
                # print(f"Stop&Wait\t{self.id_2}::ME LLEGA MENSAJE DE FINALIZAR CONEXION (FIN RAISED)")
                break

            else:
                if messageRecv.SN == self.RN:
                    self.RN =  (self.RN+1)%256
                    self.logBook.writeToLogEvent("Stop&Wait:\t A new message has been received from the sender, it will be sent to the upper layer.")
                    # print(f"MENSAJE RECIBIDO EN S&W: {msgPacket[12:]}")
                    self.queueForSandbox.put((1,msgPacket[12:])) 
                    #print(f"Mensaje:\t:numnodo: {msg.nodeID}distancia{msg.distance}")
                    #print(f"Stop&Wait:\t{self.id_2} ENVIO MENSAJE A LA CAPA SUPERIOR")
                    #print(f"Stop&Wait\t\tMensaje nuevo recibido de Stop&Wait, mi RN = {self.RN} ")
                    #print("------------------------------------------------------------------------------------------------")

                else:#Este else no va, es solo para mantenerme informado de lo que esta pasando. 
                    self.logBook.writeToLogEvent("Stop&Wait:\t A message was received with a SN different from the one expected, so it's a repeated message, it will be ignored.")

                    #print("No entro al if, el ACK tuvo que haberse perdido.")
                #esto es el envio del ACK.
                #print(f"Stop&Wait:\t {self.id_2}Mensaje:{messageRecv.nodeSrc},{messageRecv.nodeDest}, ID={messageRecv.ID_1}, {messageRecv.ID_2}||,{messageRecv.SN},{messageRecv.RN},{messageRecv.FLAGS}")
                self.id_1 = messageRecv.ID_1
                msg = ""
                messageResponse = struct.pack(f"=HH6BH{self.PAYLOAD_SIZE}s",self.myInfo.nodeNum, self.nodeInComms, self.id_1, self.id_2,0, self.RN, 0, 0, 0, msg.encode("utf-8") )
                print(f"Stop&Wait:\t {self.id_2}Respuesta:{self.myInfo.nodeNum},{self.nodeInComms}, ID= ( {self.id_1}, {self.id_2} ),rn = {self.RN}")

                self.logBook.writeToLogEvent(f"Stop&Wait:\t {self.id_2}::The ACK for the message received is being sent.")

                self.agentPinkSender.pushMessage(messageResponse, protocolPinkToGreen.protocolStopAndWait.value)
                #print(f"Stop&Wait\t\t{messageResponse.SN}, {messageResponse.RN}, va desde{messageResponse.nodeSrc},va para{messageResponse.nodeDest}")
                if messageRecv.nodeDest == self.MAX_INT16 and messageRecv.nodeSrc == self.MAX_INT16:
                    self.stoppedReceivingMessages = True
                    timerLostConnection.cancel()
                    parContinueLoop = False
                    # print(f"Stop&Wait:\t {self.id_2}\tRECEIVER VA A MORIR, LLEGA MENSAJE DE FINALIZAR")
        timerToClose = threading.Timer(self.TIMER_FOR_LOST_CONNECTION, self.stoppedReceivingMessagesFromOtherEnd)
        timerToClose.start()
        
        if self.closing is True:
            self.logBook.writeToLogEvent("Stop&Wait\t: The two way close is being executed.")
            # print(f"Stop&Wait:\t {self.id_2}VOY A INICIAR A TRATAR DE CERRAR MI COMUNICACION")
            keepTrying = True

            while keepTrying:
                #finalMessage = MessageStopAndWait(self.myInfo.nodeNum, self.nodeInComms, self.id_1, self.id_2, 0, 0,self.FIN_ACK_RAISED, self.destIdentifier,0, msg.encode("utf-8") )
                finalMessage = struct.pack(f"=HH6BH{self.PAYLOAD_SIZE}s",self.myInfo.nodeNum, self.nodeInComms, self.id_1, self.id_2, 0, 0,self.FIN_ACK_RAISED, 0,0, msg.encode("utf-8") )

                self.logBook.writeToLogEvent("Stop&Wait:\tA message with the FIN_ACK flag is being sent.")
                self.agentPinkSender.pushMessage(finalMessage, protocolPinkToGreen.protocolStopAndWait.value)
                time.sleep(0.5)

                if self.messagesQueue.empty():
                    self.logBook.writeToLogEvent("Stop&Wait:\tThe timer has been concluded, and no messages were received in the time given, so, the two way close was successfull.")
                    # print(f"\t\tStop&Wait:\t {self.id_2} FINALIZA DE FORMA EXITOSA")
                    keepTrying = False
                    timerToClose.cancel()

                else:
                    uselessMessagePacket = self.messagesQueue.get()
                    useless_msg_codified = struct.unpack(f'=HH6BH{self.PAYLOAD_SIZE}s',msgPacket)
                    uselessMessage = MessageStopAndWait(useless_msg_codified[0], useless_msg_codified[1], useless_msg_codified[2], useless_msg_codified[3], useless_msg_codified[4], useless_msg_codified[5], useless_msg_codified[6], useless_msg_codified[7], useless_msg_codified[8], useless_msg_codified[9])

                    # print(f"{uselessMessage.nodeSrc},{uselessMessage.nodeDest},{uselessMessage.SN},{uselessMessage.RN},{uselessMessage.FLAGS}")
                    if uselessMessage.FLAGS != self.FIN_RAISED:
                        keepTrying = False
                        self.logBook.writeToLogEvent("Stop&Wait:\tThe timer has been concluded, and no messages were received in the time given, so, the two way close was successfull.")

                        # print(f"\t\tStop&Wait:\t {self.id_2} FINALIZA DE FORMA EXITOSA")
                        break
                    if uselessMessage.FLAGS == 0:
                        self.logBook.writeToLogEvent("Stop&Wait:\tThe timer has been concluded, and no messages were received in the time given, so, the two way close was successfull.")
                        break

            if keepTrying == True:
                self.logBook.writeToLogWarning("Stop&Wait:\tThe unit didn't close correctly, since the sender continued sending messages with the FYN flag raised")
                # print(f"Stop&Wait:\t {self.id_2}Nunca me llega mensaje de cierre.")
                pass
        
        elif self.stoppedReceivingMessages == True:
            self.logBook.writeToLogEvent("Stop&Wait:\tThe unit has stopped receiving messages from the sender.")
            
        else:
            self.logBook.writeToLogWarning("Stop&Wait:\tThe unit has stopped its execution because the user pressed enter in the blue node.")

            
    #Function: It sends messages comming from the upper layer to the other end (receiver), until it receives the signal that the connection will end. 
    #Modifies: stoppedReceivingMessages, closing, SN, previousRTT.   
    #Requirements: Nothing.
    #Returns: Void.
    #Author: Luis Sanchez
    def sendingMessages(self, parContinueLoop):
        firstMsg = True
        pastRTT = 0
        tries = 0
        counter = 0
        while(not self.closing):
            continueAsking = True

            if self.stoppedReceivingMessages == True:
                # print(f"Stop&Wait:\t{self.id_1}::Dejo de recibir mensajes porque pierdo la comunicacion con el nodo destino.")
                break

               
            else:
                #esto corresponderia a obtener el mensaje desde la capa superior 
                pair = self.messagesFromUpperLayer.get()
                msg = pair[1]
                self.logBook.writeToLogEvent("Stop&Wait:\tA new message from the upper layer was received.")
                #print(f"Stop&Wait:\t{self.id_1}::recibo mensaje de la capa superior")
                
                if pair[0] == 1:
                    self.finalizeComs()
                    #print(f"Stop&Wait:\t{self.id_1}::soy el ultimo mensaje")
                    break
                size = len(msg)
                #print(f"El tamanyo del mensaje a enviar en s&w es {size}")
                print("voy a mandar ")
                if firstMsg == True:#el primer mensaje tiene la cualidad de que tiene la bandera ACK levantada. 
                    self.logBook.writeToLogEvent("Stop&Wait:\tSince this is the fist message containing data, it will be sent with the ACK flag raised.")
                    messageSAW = struct.pack(f'=HH6BH{self.PAYLOAD_SIZE}s',self.myInfo.nodeNum, self.destIdentifier, self.id_1, self.id_2, self.SN, self.RN, self.ACK_RAISED, 0,size , msg)
                    # print(f"El mensaje a enviar es el {messageSAW[12:]}")
                    firstMsg = False
                    # print(f"Stop&Wait:\t{self.id_1}::Envio primer mensaje")

                else:

                    messageSAW = struct.pack(f'=HH6BH{self.PAYLOAD_SIZE}s',self.myInfo.nodeNum, self.destIdentifier, self.id_1, self.id_2, self.SN, self.RN, 0, 0, size , msg)
                    # print(f"El mensaje a enviar es el {messageSAW[12:]}")
                    self.logBook.writeToLogEvent("Stop&Wait:\tThe message received from the upper layer has been placed in the payload of the S&W message.")
                    # print(f"Stop&Wait:\t{self.id_1} Envio mensaje {self.SN}")

            self.previousRTT  = 0.2 * pastRTT + 0.8 * self.previousRTT 
            
            #print(f"Stop&Wait:\t{self.id_1}:: El RTT que se tiene es actualmente de {self.previousRTT}")
            self.logBook.writeToLogEvent(f"Stop&Wait:\tThe RTT has been updated, right now, it's {self.previousRTT}")

            timerLostConnection = threading.Timer(self.TIMER_FOR_LOST_CONNECTION, self.stoppedReceivingMessagesFromOtherEnd)#Este es el timer en caso de que el receiver se muera cuando estaba comunicandose con él
            timerLostConnection.start()
            self.logBook.writeToLogEvent("Stop&Wait:\tThe timer indicating the lost of the connection between sender and receiver has started.")
            resent = False
            while(continueAsking):
                
                if self.stoppedReceivingMessages == True:
                    continueAsking = False
                else:    
                    #print(f"\n\nColoco mensaje {self.SN} en la cola para ser enviado, voy a verificar que se haya mandado correctamente.")
                    begin_RTT_measurement = time.time()
                    #print(f"Stop&Wait:\t{self.id_1}:: Mensaje:   {messageSAW.nodeSrc},{messageSAW.nodeDest},{messageSAW.SN},{messageSAW.RN},{messageSAW.FLAGS}")

                    self.agentPinkSender.pushMessage(messageSAW,protocolPinkToGreen.protocolStopAndWait.value)
                    self.logBook.writeToLogEvent("Stop&Wait:\tThe message has been sent to the receiver")
                    
                    timerToWaitAnswer = threading.Timer(self.TIMEOUT, self.isLost)
                    self.logBook.writeToLogEvent("Stop&Wait:\tThe timeout timer has started.")

                    timerToWaitAnswer.start()

                    msgPacket = self.ackQueue.get()
                    final_RTT_measurement = time.time()
                    msgCodified = struct.unpack(f'=HH6BH{self.PAYLOAD_SIZE}s',msgPacket)

                    messageRecv = MessageStopAndWait(msgCodified[0], msgCodified[1], msgCodified[2], msgCodified[3], msgCodified[4], msgCodified[5], msgCodified[6], msgCodified[7], msgCodified[8], msgCodified[9])

                    print(f"Nodo destino era {messageRecv.nodeDest} con id1 {messageRecv.ID_1} y id2 {messageRecv.ID_2}")
                    if (messageRecv.nodeDest == self.myInfo.nodeNum) and (messageRecv.RN == (self.SN+1)%256):
                        timerLostConnection.cancel()
                        timerToWaitAnswer.cancel()
                        if not resent: 
                            pastRTT = final_RTT_measurement - begin_RTT_measurement
                        else:
                            pastRTT = self.previousRTT
                        self.logBook.writeToLogEvent("Stop&Wait:\tThe ACK message has been successfully received, so the timer have been cancelled and the next packet will be sent.")
                        self.logBook.writeToLogEvent(f"Stop&Wait:\tThe RTT for the last package sent was {pastRTT}, it will be used to update the RTT of the S&W unit")

                        # print(f"RTT pasado = {pastRTT}")
                        # print("Stop&Wait\t\tMe llega confirmacion de que el mensaje llega de forma exitosa.")
                        continueAsking = False
                        self.SN = messageRecv.RN

                    elif messageRecv.nodeDest == self.MAX_INT16 and messageRecv.nodeSrc == self.MAX_INT16:

                        continueAsking = False
                        parContinueLoop = False
                        #print("Stop&Wait\t\tSENDER VA A MORIR, LLEGA MENSAJE DE FINALIZAR")
                        timerToWaitAnswer.cancel()
                        timerLostConnection.cancel()

                    else: 
                        self.logBook.writeToLogEvent("Stop&Wait:\tThe ACK wasn't received in the time given.")
                        #print("Stop&Wait\t\tNo me llega confirmacion, por lo que voy a volver a tratar de enviarlo")
                        tries+=1
                        resent = True
                    
                    if not self.ackQueue.empty():
                        quit_Message_Packet =  self.ackQueue.get()
                        msgCodified = struct.unpack(f'=HH6BH{self.PAYLOAD_SIZE}s',msgPacket)
                        quit_Message = MessageStopAndWait(msgCodified[0], msgCodified[1], msgCodified[2], msgCodified[3], msgCodified[4], msgCodified[5], msgCodified[6], msgCodified[7], msgCodified[8], msgCodified[9])
                        
                        if quit_Message.nodeDest == self.MAX_INT16 and quit_Message.nodeSrc == self.MAX_INT16:
                            continueAsking = False
                            self.closing = True
                            parContinueLoop = False
                            #print("Stop&Wait\t\tSENDER VA A MORIR, LLEGA MENSAJE DE FINALIZAR")
                            timerToWaitAnswer.cancel()
        if self.closing is True:
            self.logBook.writeToLogEvent("Stop&Wait:\tThe two way close is going to start.")
            # print(f"Stop&Wait:\t{self.id_1}:: Voy a intentar finalizar la ejecucion")
            closes = self.closeSender(self.nodeInComms)
            

    #Function: It begins the connection from the side of the sender, which is done by a three way handshake. 
    #Modifies: id_1, id_2, nodeInComms, previousRTT.   
    #Requirements: The destination of the message and the id of the connection in the multiplexor.
    #Returns: A boolean which indicates wheather it received the ACK from the receiver or no.
    #Author: Luis Sanchez
    def initSender(self):
        tries = self.MAX_TRIES
        msg = ""
        self.logBook.writeToLogEvent(f"Stop&Wait:\t{self.id_1}::The initial message (with the SYN flag raised) will be sent, so the three way handshake may be established.")
        self.nodeInComms = self.destIdentifier
        initialMessage =  struct.pack(f"=HH6BH{self.PAYLOAD_SIZE}s",self.myInfo.nodeNum, self.destIdentifier, self.id_1 , 0, 0, 0, self.SYN_RAISED, 0, 0 , "".encode("utf-8"))
        #MessageStopAndWait(self.myInfo.nodeNum , self.destIdentifier , self.id_1 , 0 , 0 , 0 , self.SYN_RAISED , self.destIdentifier, 0 , msg.encode("utf-8"))
        
        while tries > 0:
            begin_RTT_measurement = time.time()
            self.agentPinkSender.pushMessage(initialMessage,protocolPinkToGreen.protocolStopAndWait.value)
            self.logBook.writeToLogEvent(f"Stop&Wait:\t{self.id_1}::The message has been sent to the receiver")
            timerToWaitAnswer = threading.Timer(self.TIMEOUT, self.isLost)
            timerToWaitAnswer.start()
            self.logBook.writeToLogEvent(f"Stop&Wait:\t{self.id_1}::The timers have been started.")

            messageRecvPacket = self.ackQueue.get()
            msgCodified = struct.unpack(f'=HH6BH{self.PAYLOAD_SIZE}s',messageRecvPacket)
            messageRecv = MessageStopAndWait(msgCodified[0], msgCodified[1], msgCodified[2], msgCodified[3], msgCodified[4], msgCodified[5], msgCodified[6], msgCodified[7], msgCodified[8], msgCodified[9])            

            if(messageRecv.nodeDest == self.myInfo.nodeNum):
                timerToWaitAnswer.cancel()
            
                if messageRecv.FLAGS == self.SYN_ACK_RAISED:

                    final_RTT_measurement = time.time()
                    self.previousRTT = final_RTT_measurement - begin_RTT_measurement
                    #print(f"Stop&Wait:\t{self.id_1}::Mi RTT actual es de {self.previousRTT}")

                    # print(f"id_1: {messageRecv.ID_1} id_2: {messageRecv.ID_2} flags: {messageRecv.FLAGS}")
                    self.id_2 = messageRecv.ID_2
                    self.logBook.writeToLogEvent("Stop&Wait:\tThe SYN_ACK message has been received by the sender.")

                    # print(f"Stop&Wait:\t{self.id_1}::Me llega mensaje de SYN_ACK.")
                    return self.SUCCESS
            else:
                tries -= 1
                self.logBook.writeToLogEvent("Stop&Wait:\tThe connection between sender and receiver couldn't be estabished, so, it will be tried again.")
                # print(f"Stop&Wait:\t{self.id_1}::No pude establecer conexion en este turno, voy a volver a intentarlo.")
        self.logBook.writeToLogEvent(f"Stop&Wait:\tThe connection between sender and receiver couldn't be established in the {self.MAX_TRIES} attempts")

        # print(f"Stop&Wait:\t{self.id_1}::No se pudo establecer la conexion entre emisor y fuente en los intentos dados.")
        return self.FAILURE


    #Function: It begins the connection from the side of the receiver, which is done by a three way handshake. 
    #Modifies: Nothing.  
    #Requirements: The message received with the SYN flag raised and the id of the connection in the multiplexor.
    #Returns: A boolean which indicates wheather it received the ACK from the receiver or no.
    #Author: Luis Sanchez
    def initReceiver(self, parReceivedMessage):
        tries = self.MAX_TRIES

        msgCodified = struct.unpack(f'=HH6BH{self.PAYLOAD_SIZE}s',parReceivedMessage)
        receivedMessage = MessageStopAndWait(msgCodified[0], msgCodified[1], msgCodified[2], msgCodified[3], msgCodified[4], msgCodified[5], msgCodified[6], msgCodified[7], msgCodified[8], msgCodified[9])            

        #print(f"Stop and wait init receiver: source {receivedMessage.nodeSrc}, dest(me){receivedMessage.nodeDest}, id: {receivedMessage.ID_1},{receivedMessage.ID_2}, flags = {receivedMessage.FLAGS}")
        self.nodeInComms = receivedMessage.nodeSrc

        if receivedMessage.FLAGS == self.SYN_RAISED:
            self.logBook.writeToLogEvent(f"Stop&Wait:\tA message with the SYN flag was received in the receiver side of the connection.")

            #print(f"Stop&Wait:\t {self.id_2}Me llega mensaje de SYN")
            msg = ""
            initialMessage = struct.pack(f"=HH6BH{self.PAYLOAD_SIZE}s",self.myInfo.nodeNum , receivedMessage.nodeSrc , receivedMessage.ID_1 , self.id_2 , 0, 0, self.SYN_ACK_RAISED, 0, 0,  msg.encode("utf-8"))
            #MessageStopAndWait(self.myInfo.nodeNum , receivedMessage.nodeSrc , receivedMessage.ID_1 , self.id_2 , 0, 0, self.SYN_ACK_RAISED, self.destIdentifier, 0,  msg.encode("utf-8") )
            self.logBook.writeToLogEvent(f"Stop&Wait:\tThe SYN_ACK message will be sent to the sender.")
            #print(f"ESTABLECIENDO CONEXION EN RECEIVER ES DE{initialMessage.nodeSrc} A {initialMessage.nodeDest} ID={initialMessage.ID_1},{initialMessage.ID_2} FLAGS= {initialMessage.FLAGS}")
            while tries > 0:
                self.agentPinkSender.pushMessage(initialMessage,protocolPinkToGreen.protocolStopAndWait.value)
                timerToWaitAnswer = threading.Timer(self.TIMEOUT, self.isLost)
                timerToWaitAnswer.start()
                
                messageRecvPacket = self.messagesQueue.get()
                msgCodified = struct.unpack(f'=HH6BH{self.PAYLOAD_SIZE}s',messageRecvPacket)
                messageRecv = MessageStopAndWait(msgCodified[0], msgCodified[1], msgCodified[2], msgCodified[3], msgCodified[4], msgCodified[5], msgCodified[6], msgCodified[7], msgCodified[8], msgCodified[9])            

                if(messageRecv.FLAGS == self.ACK_RAISED):
                    timerToWaitAnswer.cancel()
                    if not self.messagesQueue.empty():
                        basura = self.messagesQueue.get()
                    self.pushNewPacket(messageRecvPacket)
                    self.logBook.writeToLogEvent(f"Stop&Wait:\tThe ACK message has been received, indicating the establishment of the three way handshake.")
                    #print(f"Stop&Wait:\t {self.id_2}ME LLEGA MENSAJE ACK, POR LO QUE INICIO YA EL FUNCIONAMIENTO NORMAL.")

                    return self.SUCCESS
                else:
                    self.logBook.writeToLogEvent(f"Stop&Wait:\tThe ACK message wasn't received in the last attempt.")

                    #print(f"Stop&Wait:\t {self.id_2}tNo me llega el mensaje esperado")
                tries -=1
                self.logBook.writeToLogEvent(f"Stop&Wait:\t There are still {tries} attempts to establish the three way handshake correctly.")
                #print(f"Stop&Wait:\t {self.id_2}ME QUEDAN {tries} INTENTOS")
        self.logBook.writeToLogEvent(f"Stop&Wait:\t The connection between sender and receiver couldn't be established correctly.")

        return self.FAILURE


    #Function: It closes the connection from the side of the sender, which is done by the two way close. 
    #Modifies: Nothing.  
    #Requirements: Number of the receiver node.
    #Returns: A boolean which indicates wheather it received the FIN_ACK from the receiver or no.
    #Author: Luis Sanchez
    def closeSender(self, receiver):
        self.logBook.writeToLogEvent(f"Stop&Wait:\t The connection will be cut between sender and receiver.")
        #print(f"Stop&Wait:\t{self.id_1}::Hora de cerrar comunicacion entre emisor y receptor")

        msg = ""
        closingMessage =  struct.pack(f"=HH6BH{self.PAYLOAD_SIZE}s",self.myInfo.nodeNum, receiver, self.id_1, self.id_2, 0 , 0, self.FIN_RAISED, 0, 0, msg.encode("utf-8"))
        #MessageStopAndWait(self.myInfo.nodeNum, receiver, self.id_1, self.id_2, 0 , 0, self.FIN_RAISED, self.destIdentifier, 0, msg.encode("utf-8"))
        self.logBook.writeToLogEvent(f"Stop&Wait:\tThe FIN message has been created.")

        received = False
        userInfluence = False
        timerToClose = threading.Timer(self.TIMER_FOR_LOST_CONNECTION, self.isLost)
        timerToClose.start()

        while(not received):
            self.agentPinkSender.pushMessage(closingMessage,protocolPinkToGreen.protocolStopAndWait.value)
            self.logBook.writeToLogEvent("Stop&Wait:\tThe message has been sent to the receiver")

            messageRecvPacket = self.ackQueue.get()
            msgCodified = struct.unpack(f'=HH6BH{self.PAYLOAD_SIZE}s',messageRecvPacket)
            messageRecv = MessageStopAndWait(msgCodified[0], msgCodified[1], msgCodified[2], msgCodified[3], msgCodified[4], msgCodified[5], msgCodified[6], msgCodified[7], msgCodified[8], msgCodified[9])      
            if messageRecv.FLAGS == self.FIN_ACK_RAISED:
                received = True
                timerToClose.cancel()
                #print(f"\t\tStop&Wait:\t{self.id_1}::LLEGA MENSAJE DE CIERRE ENTRE EMISOR Y RECEPTOR.")
                self.logBook.writeToLogEvent("Stop&Wait:\tThe FIN_ACK message has been correctly received, so the connection was closed correctly.")

            elif(messageRecv.nodeDest == self.MAX_INT16 and messageRecv.nodeSrc == self.MAX_INT16):
                userInfluence = True
                break
            else: 
                break

        if not received and userInfluence == True:
            #print(f"Stop&Wait:\t{self.id_1}::Se ciera por comando del usuario.")
            return self.FAILURE
        elif not received:
            self.logBook.writeToLogEvent("Stop&Wait:\tThe FIN_ACK message wasn't received, so the connection was closed incorrectly.")

            #print(f"Stop&Wait:\t{self.id_1}::Nunca llega el mensaje de cierre.")
        else:
            return self.SUCCESS
    

    #Function: It creates the thread of the dispatcher and depending on the role of the Stop&Wait instance, it will create the secure channel between sender and receiver as the sender or the receiver. 
    #Modifies: Nothing.  
    #Requirements: The continueLoop parameter. 
    #Returns:Nothing
    #Author: Luis Sanchez
    def execute(self,parContinueLoop):
        if self.role != self.FIRST_INIT:
            # self.eventReadyToStart.wait()
            # self.eventReadyToStart.clear() 
            #print("hola antes de dispat")
            threadDispatcher = threading.Thread(target=self.dispatcher,args=(parContinueLoop,))   
            threadDispatcher.start()
            #print("hola despues de la picha")

            self.logBook.writeToLogEvent("Stop&Wait:\tThe dispatcher thread was created correctly.")

            if self.role is self.SENDER:
                connectionEstablished = self.initSender() #cambio aca cuando cambio el caso de prueba 

                if connectionEstablished:  
    
                    #print(f"Stop&Wait:\t{self.id_1} SE ESTABLECE CONEXION CON EXITO CON EL RECEPTOR")

                    # threadSendingMessages = threading.Thread(target=self.sendingMessages,args=(parContinueLoop,))
                    # threadSendingMessages.start()
                    # threadSendingMessages.join()
                    self.sendingMessages(parContinueLoop)

                else:
                    self.forcedClosure()
                    #print(f"Stop&Wait:\t{self.id_1} NO SE PUDO ESTABLECER LA COMUNICACION CON EL RECEPTOR")
            else:
            
                # timerToStart = threading.Timer(10.0, self.isLost)
                # timerToStart.start()
                #print("voy a empezar como receptor")
                msgReceivedPacket = self.messagesQueue.get()
                
                connectionEstablished = self.initReceiver(msgReceivedPacket)
                #print(f"Stop&Wait:\t{self.id_2} obtengo mensaje de la cola")

                if connectionEstablished == self.SUCCESS:
                    #print(f"Stop&Wait:\t{self.id_2} Se establece conexion con exito con el emisor")
                    # timerToStart.cancel()
                    self.receivingMessages(parContinueLoop)
                    # threadAskingForMessages = threading.Thread(target=self.receivingMessages,args=(parContinueLoop,))
                    # threadAskingForMessages.start()
                    # threadAskingForMessages.join()

                else: 
                    self.forcedClosure()
                    
                    #print(f"Stop&Wait:\t{self.id_2} No se pudo establecer la conexion con el emisor")
            #print("Salgo de mi labor de stop and wait, sea cual sea")
            #if not self.closing:
            self.forcedClosure()
            #    print("cierro a la fuerza")

            threadDispatcher.join()
        self.inUse = False
            #print("uno al thread del despachador")
        #print("Salgo de execute en stop and wait")


    def imInUse(self):
        if self.inUse == True:
            return True
        else:
            return False