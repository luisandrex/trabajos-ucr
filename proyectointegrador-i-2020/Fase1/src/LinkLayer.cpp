#include "../headers/LinkLayer.h"

LinkLayer::LinkLayer(){
    this->logBook = NULL;
}

LinkLayer::~LinkLayer(){
    this->logBook = NULL;
}

LinkLayer::LinkLayer(std::shared_ptr<LogBook> logBook){
    this->logBook = logBook;
    statisticsDataPlane = std::shared_ptr<StatisticsDataPlane>(new StatisticsDataPlane(logBook));
    broadcastFunctionality = std::shared_ptr<Broadcast>(new Broadcast(logBook));
    forwardingFunctionality = std::shared_ptr<Forwarding>(new Forwarding(logBook));
    pinkLayer =  std::shared_ptr<PinkLayer>(new PinkLayer(logBook, broadcastFunctionality, forwardingFunctionality, statisticsDataPlane));
}


void LinkLayer::recvGreenMessageFromGreen(std::shared_ptr<int> continueLoop, std::shared_ptr<std::mutex> mutexForWritingInQueue, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessageToDispatcher, std::shared_ptr<sem_t> semaphoreDispatcher) 
{
    int sockUDP = socket(AF_INET, SOCK_DGRAM, 0);
    if(sockUDP < 0) 
    {   
        // ERROR(UDP socket could not be created ) written to log
        logBook->writeToLogError("LinkLayer:\tIn communication between green-green nodes: There is a problem creating the server socket UDP for the green node");
        
        printf("LinkLayer:\tError creating server UDP.\n");

        continueLoop = 0;
        return;
    }

    // EVENT(UDP socket has been created successfully) written to log
    logBook->writeToLogEvent("LinkLayer:\tIn communication between green-green nodes: The server socket UDP for the green node have been created");

    struct sockaddr_in serverUDP;
    serverUDP.sin_family = AF_INET;
    serverUDP.sin_port = htons(nodeGreen.portUDP);
    inet_pton(AF_INET, nodeGreen.ip, &(serverUDP.sin_addr.s_addr));
    if(bind(sockUDP, (struct sockaddr*)&serverUDP, sizeof(serverUDP)) < 0) 
    {
        // EVENT(UDP server could not be binded with the green node) written to log
        logBook->writeToLogError("LinkLayer:\tIn communication between green-green nodes: There is a problem binding the server UDP for the green node");
                 
        printf("LinkLayer:\tError binding server UDP.\n");
        continueLoop = 0;
        return;
    }
    
    // EVENT(UDP socket has been binded successfully) written to log
    logBook->writeToLogEvent("LinkLayer:\tIn communication between green-green nodes: The UDP socket has been successfully binded");

    struct sockaddr_in clientUDP;
    int clientLen = sizeof(struct sockaddr_in);

    while(*continueLoop) 
    { 
        struct UDPPacket packetRecv;
        int bytesRecv = recvfrom(sockUDP, (void*)&packetRecv, sizeof(packetRecv), MSG_DONTWAIT, (struct sockaddr*)&clientUDP,  (socklen_t*)&clientLen);
        if (bytesRecv < 0)
        {
            if (errno != EAGAIN) 
            {
                // EVENT(The server UDP had an error in the recvfrom) written to log
                logBook->writeToLogWarning("LinkLayer:\tIn communication between green-green nodes: The server UDP had an error in the recvfrom");
                *continueLoop = 0;
            }
        } 
        else 
        {
            std::shared_ptr<struct MessageGreenGreen> msg = std::shared_ptr<struct MessageGreenGreen>(new MessageGreenGreen());
            msg->protocol = packetRecv.protocol;
            bool validProtocol = true;
            printf("\t\tProtocolo RECV %d\n", msg->protocol);
            switch(msg->protocol) {
                case forwardingProtocol:
                {
                    memset(&msg->msgForwarding, 0, sizeof(msg->msgForwarding));
                    memcpy(&msg->msgForwarding, packetRecv.payload, sizeof(MessageForwarding));
                    msg->msgForwarding.src = ntohs(msg->msgForwarding.src);
                    msg->msgForwarding.dest = ntohs(msg->msgForwarding.dest);
                    msg->msgForwarding.msgNum = ntohs(msg->msgForwarding.msgNum);
                    strncpy(msg->msgForwarding.msg, msg->msgForwarding.msg, strlen(msg->msgForwarding.msg));
                    
                    statisticsDataPlane->addPacketForwarding();
                    statisticsDataPlane->addPacketRecvFrom(msg->msgForwarding.src);
                    
                    logBook->writeToLogEvent("LinkLayer:\tA message with protocol of forwarding received");
                }
                break;
                case broadcastProtocol:
                {
                    memset(&msg->msgBroadcast, 0, sizeof(msg->msgBroadcast));
                    
                    memcpy(&msg->msgBroadcast.nodeSrc, packetRecv.payload, sizeof(uint16_t));
                    msg->msgBroadcast.nodeSrc = ntohs(msg->msgBroadcast.nodeSrc);

                    memcpy(&msg->msgBroadcast.TTL, packetRecv.payload + sizeof(uint16_t), sizeof(uint8_t));
                    
                    memcpy(&msg->msgBroadcast.protocol, packetRecv.payload + sizeof(uint16_t) + sizeof(uint8_t), sizeof(uint8_t));

                    if(msg->msgBroadcast.protocol == dijkstraProtocol) 
                        msg->msgBroadcast.protocol = BroadcastSpanningProtocol;
                    else 
                        msg->msgBroadcast.protocol = BroadcastDataProtocol;                        
                    
                    if(msg->msgBroadcast.protocol == BroadcastSpanningProtocol) {
                        memcpy(&msg->msgBroadcast.SpanningBroadcast, packetRecv.payload + sizeof(uint16_t) + sizeof(uint8_t) + sizeof(uint8_t), sizeof(MessageBroadcastSP));

                        msg->msgBroadcast.SpanningBroadcast.msgDijkstra.nodeSrc = ntohs(msg->msgBroadcast.SpanningBroadcast.msgDijkstra.nodeSrc);
                        msg->msgBroadcast.SpanningBroadcast.msgDijkstra.numAdyNodes = msg->msgBroadcast.SpanningBroadcast.msgDijkstra.numAdyNodes;
                        for(int counterAdy = 0; counterAdy < msg->msgBroadcast.SpanningBroadcast.msgDijkstra.numAdyNodes; ++counterAdy) {
                            msg->msgBroadcast.SpanningBroadcast.msgDijkstra.pairAdyDistance[counterAdy].nodeID = ntohs(msg->msgBroadcast.SpanningBroadcast.msgDijkstra.pairAdyDistance[counterAdy].nodeID);
                        }

                    } else if (msg->msgBroadcast.protocol == BroadcastDataProtocol){  
                        memcpy(&msg->msgBroadcast.DataBroadcast, packetRecv.payload + sizeof(uint16_t) + sizeof(uint8_t) + sizeof(uint8_t), sizeof(MessageBroadcastData));

                        msg->msgBroadcast.DataBroadcast.src = ntohs(msg->msgBroadcast.DataBroadcast.src);
                        msg->msgBroadcast.DataBroadcast.dest = ntohs(msg->msgBroadcast.DataBroadcast.dest);
                        msg->msgBroadcast.DataBroadcast.msgNum = ntohs(msg->msgBroadcast.DataBroadcast.msgNum); 
                        msg->msgBroadcast.DataBroadcast.RN = ntohs(msg->msgBroadcast.DataBroadcast.RN); 
                        strncpy(msg->msgBroadcast.DataBroadcast.msg, msg->msgBroadcast.DataBroadcast.msg, strlen(msg->msgBroadcast.DataBroadcast.msg));
                    } else {
                        logBook->writeToLogWarning("LinkLayer:\tThe protocol received in the broadcast packet is not recognized");
                    }                    

                    statisticsDataPlane->addPacketBroadcast();
                    statisticsDataPlane->addPacketRecvFrom(msg->msgBroadcast.nodeSrc);
                    
                    logBook->writeToLogEvent("LinkLayer:\tA message with protocol of broadcast received");
                }
                break;
                case spanningTreeProtocol:
                {   
                    memset(&msg->msgSpanning, 0, sizeof(msg->msgSpanning));
                    memcpy(&msg->msgSpanning, packetRecv.payload, sizeof(MessageSpanningTree));
                    msg->msgSpanning.question = msg->msgSpanning.question;
                    msg->msgSpanning.src = ntohs(msg->msgSpanning.src);
                    msg->msgSpanning.dest = ntohs(msg->msgSpanning.dest);
                    msg->msgSpanning.SN = ntohs(msg->msgSpanning.SN);
                    msg->msgSpanning.RN = ntohs(msg->msgSpanning.RN);
                    statisticsDataPlane->addPacketRecvFrom(msg->msgSpanning.src);
                    logBook->writeToLogEvent("LinkLayer:\tA message with protocol of spanning tree received");
                }
                break;
                case dijkstraProtocol:
                {
                    memset(&msg->msgDijkstra, 0, sizeof(msg->msgDijkstra));
                    memcpy(&msg->msgDijkstra, packetRecv.payload, sizeof(MessageDijkstra));
                    msg->msgDijkstra.nodeSrc = ntohs(msg->msgDijkstra.nodeSrc);
                    msg->msgDijkstra.numAdyNodes = msg->msgDijkstra.numAdyNodes;
                    for(int counterAdy = 0; counterAdy < ((int)(2 * msg->msgDijkstra.numAdyNodes)); ++counterAdy) {
                        msg->msgDijkstra.pairAdyDistance[counterAdy].nodeID = ntohs(msg->msgDijkstra.pairAdyDistance[counterAdy].nodeID);
                    }
                    logBook->writeToLogEvent("LinkLayer:\tA message with protocol of dijkstra received");

                }
                break;
                case heartBeatProtocol: 
                {
                    memset(&msg->msgHeartBeat, 0, sizeof(msg->msgHeartBeat));
                    memcpy(&msg->msgHeartBeat, packetRecv.payload, sizeof(MessageHeartBeat));
                    msg->msgHeartBeat.code = msg->msgHeartBeat.code;
                    msg->msgHeartBeat.nodeSrc  = ntohs(msg->msgHeartBeat.nodeSrc);
                    msg->msgHeartBeat.nodeDest = ntohs(msg->msgHeartBeat.nodeDest);
                    msg->msgHeartBeat.SN = ntohs(msg->msgHeartBeat.SN);
                    msg->msgHeartBeat.RN = ntohs(msg->msgHeartBeat.RN);
			        statisticsDataPlane->addPacketRecvFrom(msg->msgHeartBeat.nodeSrc);
                    logBook->writeToLogEvent("LinkLayer:\tA message with protocol of heartBeat received");
                }
                break;
                case stopAndWaitProtocol:
                {
                    //printf("LINK LAYER ME LLEGA MENSAJE DE STOP AND WAIT DE OTRO NODO");

                    memset(&msg->msgStopandWait, 0, sizeof(msg->msgStopandWait));

                    memcpy(&msg->msgStopandWait, packetRecv.payload, sizeof(msg->msgStopandWait));

                    msg->msgStopandWait.dest = ntohs(msg->msgStopandWait.dest);
                    msg->msgStopandWait.src = ntohs(msg->msgStopandWait.src);

                    msg->msgStopandWait.SN = msg->msgStopandWait.SN;
                    msg->msgStopandWait.RN = msg->msgStopandWait.RN;

                    msg->msgStopandWait.ID_1 = msg->msgStopandWait.ID_1;
                    msg->msgStopandWait.ID_2 = msg->msgStopandWait.ID_2;

                    msg->msgStopandWait.FLAGS = msg->msgStopandWait.FLAGS;
                    
                    msg->msgStopandWait.identifier = msg->msgStopandWait.identifier;

                    msg->msgStopandWait.payload_size = ntohs(msg->msgStopandWait.payload_size);
                     
                    statisticsDataPlane->addPacketRecvFrom(msg->msgStopandWait.src);
                    //printf("\nLinkLayer:\t\tRecibo SN %d RN %d dest %d src %d\n", msg->msgStopandWait.SN, msg->msgStopandWait.RN, ntohs(msg->msgStopandWait.dest), ntohs(msg->msgStopandWait.src));
                    logBook->writeToLogEvent("LinkLayer:\tA message with protocol of stop and wait received");
                }
                break;
                default:
                    validProtocol = false;
                    logBook->writeToLogEvent("LinkLayer:\tThe protocol received does not correspond to any known protocol");
                break;
            }
            if(validProtocol) 
            {
                mutexForWritingInQueue->lock();
                queueMessageToDispatcher->push(msg);
                sem_post(&(*semaphoreDispatcher));
                mutexForWritingInQueue->unlock();
            }
        }
    }
    return;
}

     
void LinkLayer::dispatcherMessageGreenGreen(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<MessageGreenGreen>>> queueMessageToDispatcher, std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlue, std::shared_ptr<sem_t> blueLayerSemaphore, std::shared_ptr<sem_t> semaphoreDispatcher) 
{
    while(*continueLoop)
    {   
        std::shared_ptr<struct MessageGreenGreen> msg;

        sem_wait(&(*semaphoreDispatcher));
        msg = queueMessageToDispatcher->front();
        queueMessageToDispatcher->pop();
        //printf("Protocolo del dispatcher %d\n", msg->protocol);
        switch(msg->protocol)
        {
            case forwardingProtocol:
            {
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been sent to the forwarding functionality to be processed there.");
                struct ForwardingContainer container = placeInContainer(msg,forwardingProtocol);
                forwardingFunctionality->pushMessageForForwarding(container);                    
            }
            break;
            case broadcastProtocol:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been sent to the broadcast functionality to be processed there.");
                broadcastFunctionality->pushMessageForBroadcast(msg->msgBroadcast);
                break;
            case spanningTreeProtocol:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been sent to the pink layer to be processed in the spanning tree");
                pinkLayer->pushMessagePinkForSpanningTree(msg->msgSpanning);
                break;
            case dijkstraProtocol:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been sent to the pink layer to be processed for dijkstra");
                pinkLayer->pushMessagePinkForDijkstra(msg->msgDijkstra);
                break;
            case heartBeatProtocol:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been sent to the pink layer to be processed for HeartBeat");
                pinkLayer->pushMessagePinkForHeartBeat(msg->msgHeartBeat);
                break;
            case stopAndWaitProtocol:
            {
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been sent to forwarding functionality to be processed there.");
                //printf("LinkLayer:\tStop&Wait protocol en dis%d dest %d src %d\n", msg->protocol,msg->msgStopandWait.dest, msg->msgStopandWait.src);

                struct ForwardingContainer container = placeInContainer(msg,stopAndWaitProtocol);

                //printf("LinkLayer:\tStop&Wait protocol en dis%d dest %d src %d\n", msg->protocol,container.msgStopAndWait.dest, container.msgStopAndWait.src);
                forwardingFunctionality->pushMessageForForwarding(container);
            }
                break;
            case broadcastToNeighborProtocol:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been added to the queue of message to send to another green node");
                queueMessagesToGreen.push(msg);
                break;
            case forwardingToNeighborProtocol:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been added to the queue of message to send to another green node");
                queueMessagesToGreen.push(msg);
                break;
            case controlMessageHeartbeatProtocol:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been added to the queue of message to send to another green node");
                queueMessagesToGreen.push(msg);
                break;
            case controlMessageSpanningTreeProtocol:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been added to the queue of message to send to another green node");
                queueMessagesToGreen.push(msg);
                break;
            case messageToBlueProtocol:
            {
                std::shared_ptr<struct MessageForwarding> msgToBlue = std::shared_ptr<struct MessageForwarding>(new struct MessageForwarding);
                msgToBlue = copyMessage(msg->msgForwarding);
                queueMessageToBlue->push(msgToBlue);
                sem_post(&(*blueLayerSemaphore));
                logBook->writeToLogEvent("LinkLayer:\tA message has been placed in the blue layer's queue, so it can be sent to the blue node");
            }
                break;
            case stopAndWaitToPink:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue will be sent to the pink layer to be processed for Stop&Wait.");
                //printf("LinkLayer:\tEnvio de tipo Stop&Wait a pink layer\n");
                pinkLayer->pushMessagePinkForStopAndWait(msg->msgStopandWait);
                break;
            case stopAndWaitToNeighborProtocol:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been added to the queue of messages to send to another node for Stop&Wait.");
                //printf("LinkLayer:\tvoy a enviar de tipo Stop&Wait a vecino en despachador\n");
                queueMessagesToGreen.push(msg);
                break;
            case sandboxFromBlueProtocol:
                logBook->writeToLogEvent("LinkLayer:\tThe message read from the queue has been added to the queue of messages for sandbox.");
                pinkLayer->pushMessageForSandbox(msg->msgSandbox); 
                break;
            default:
            // recv -1 has protocol indicate that the blue node is not going to recv more messages
                logBook->writeToLogEvent("LinkLayer:\tThe green node is going to be finished");
                ForwardingContainer nullMessageForwarding = {{}, {}, -1};
                forwardingFunctionality->pushMessageForForwarding(nullMessageForwarding);
                BroadcastContainer nullMessageBroadcast = {0,0,std::numeric_limits<uint8_t>::max(),{},{}};
                broadcastFunctionality->pushMessageForBroadcast(nullMessageBroadcast);
                MessageDijkstra msgNull = {(uint16_t)-1, (uint8_t)-1, {}};
                pinkLayer->pushMessagePinkForDijkstra(msgNull);
                MessageHeartBeat msgNullHB = {std::numeric_limits<uint16_t>::max(), (uint16_t)-1, (uint8_t)-1, (uint16_t)-1, (uint16_t)-1};
                pinkLayer->pushMessagePinkForHeartBeat(msgNullHB);
                MessageStopAndWait msgNullSW = {(uint16_t)-1,(uint16_t) -1, (uint8_t)-1, (uint8_t)-1, (uint8_t)-1, (uint8_t)-1 , (uint8_t)-1, (uint8_t)-1, (uint16_t)-1, ""};
                pinkLayer->pushMessagePinkForStopAndWait(msgNullSW);
                MessageSpanningTree msgNullST = {std::numeric_limits<uint8_t>::max(), 0, 0, 0, 0};
                pinkLayer->pushMessagePinkForSpanningTree(msgNullST);
                std::shared_ptr<struct MessageForwarding> msgToBlue = std::shared_ptr<struct MessageForwarding>(new struct MessageForwarding);
                msgToBlue->dest = 0;
                queueMessageToBlue->push(msgToBlue);
                sem_post(&(*blueLayerSemaphore));
                MessageSandbox msgNullSB = {""};
                pinkLayer->pushMessageForSandbox(msgNullSB);
                *continueLoop = 0;
                break;
        }
        

    }
    return;
}


void LinkLayer::sendGreenGreenMessages(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlueNode) {

    while (*continueLoop) {
        if(!queueMessagesToGreen.empty()) {
            std::shared_ptr<struct MessageGreenGreen> msgGreenGreen = queueMessagesToGreen.front();
            queueMessagesToGreen.pop();

            logBook->writeToLogEvent("LinkLayer:\tA message is going to be proccessed");
            if (msgGreenGreen->protocol == broadcastToNeighborProtocol)
                msgGreenGreen->protocol = broadcastProtocol;
            else if (msgGreenGreen->protocol == forwardingToNeighborProtocol)
                msgGreenGreen->protocol = forwardingProtocol;
            else if (msgGreenGreen->protocol == controlMessageHeartbeatProtocol)
                msgGreenGreen->protocol = heartBeatProtocol;
            else if (msgGreenGreen->protocol == controlMessageSpanningTreeProtocol)
                msgGreenGreen->protocol = spanningTreeProtocol;
            else if (msgGreenGreen->protocol == stopAndWaitToNeighborProtocol)
                msgGreenGreen->protocol = stopAndWaitProtocol;
            
            struct UDPPacket packetToSend = {};
            switch (msgGreenGreen->protocol) {
                case forwardingProtocol:
                {
                    // Message is sent to the corresponding green node 
                    bool find = false;
                    for (int adyNode = 0; adyNode < adyNodes.size() && !find; ++adyNode)
                    {
                        if (adyNodes[adyNode].nodeNum == msgGreenGreen->nodeInter) 
                        {
                            find = true;
                            // printf("LinkLayer:\tMessage %s is going to be sent to %d from %d\n", msgGreenGreen->msgForwarding.msg, msgGreenGreen->msgForwarding.dest, msgGreenGreen->msgForwarding.src);
                            // printf("LinkLayer:\tNode inter is %d\n", msgGreenGreen->nodeInter);
                            //msgGreenGreen->protocol = htons(forwardingProtocol);
                            packetToSend.protocol = forwardingProtocol;

                            msgGreenGreen->msgForwarding.dest = htons(msgGreenGreen->msgForwarding.dest);
                            msgGreenGreen->msgForwarding.src = htons(msgGreenGreen->msgForwarding.src);
                            msgGreenGreen->msgForwarding.msgNum = htons(msgGreenGreen->msgForwarding.msgNum);

                            memcpy(packetToSend.payload, &msgGreenGreen->msgForwarding, sizeof(msgGreenGreen->msgForwarding));

                            int bytesSent = sendto(adyNodes[adyNode].sock, &packetToSend, (sizeof(msgGreenGreen->msgForwarding) + 1), 0, (struct sockaddr*)&adyNodes[adyNode].server, sizeof(struct sockaddr_in));
                            if (bytesSent < 0) 
                            {
                                // ERROR(Socket is not available, therefore, the message could not be sent) written to log
                                logBook->writeToLogWarning("LinkLayer:\tIn communication between green-green nodes: Error sending message to the socket of the adyacent node. That socket is not available yet.");
                            }
                            statisticsDataPlane->addPacketSendTo(msgGreenGreen->nodeInter);
                        }
                    }      
                }
                break;
                case broadcastProtocol:
                {

                    logBook->writeToLogEvent("LinkLayer:\tThe protocol of the message proccessed was broadcast");
                    bool find = false;
                    for(int adyNode = 0; adyNode < adyNodes.size() && !find; ++adyNode) {
                        if(adyNodes[adyNode].nodeNum == msgGreenGreen->nodeInter) {
                            
                            find = true;

                            logBook->writeToLogEvent("LinkLayer:\tDestination of the broadcast packet found");

                            packetToSend.protocol = broadcastProtocol;

                            msgGreenGreen->msgBroadcast.nodeSrc = htons(msgGreenGreen->msgBroadcast.nodeSrc);
                            memcpy(packetToSend.payload, &msgGreenGreen->msgBroadcast.nodeSrc, sizeof(uint16_t));
                            
                            msgGreenGreen->msgBroadcast.TTL = msgGreenGreen->msgBroadcast.TTL;
                            memcpy(&packetToSend.payload[sizeof(uint16_t)], &msgGreenGreen->msgBroadcast.TTL, sizeof(uint8_t));


                            int size = sizeof(msgGreenGreen->msgBroadcast) - sizeof(msgGreenGreen->msgBroadcast.SpanningBroadcast);
                            if(msgGreenGreen->msgBroadcast.protocol == BroadcastSpanningProtocol) {

                                msgGreenGreen->msgBroadcast.protocol = dijkstraProtocol; 
                                memcpy(&packetToSend.payload[sizeof(uint16_t) + sizeof(uint8_t)], &msgGreenGreen->msgBroadcast.protocol, sizeof(uint8_t));

                                msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.nodeSrc = htons(msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.nodeSrc);
                                
                                msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.numAdyNodes = msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.numAdyNodes;
                                
                                for(int counterAdy = 0; counterAdy < msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.numAdyNodes; ++counterAdy) {
                                    msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.pairAdyDistance[counterAdy].nodeID = htons(msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.pairAdyDistance[counterAdy].nodeID);
                                }

                                size =  sizeof(msgGreenGreen->msgBroadcast) - sizeof(msgGreenGreen->msgBroadcast.DataBroadcast) - sizeof(msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.pairAdyDistance) + (msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.numAdyNodes * sizeof(AdjacentListInfo));
                                int sizeToCpy = sizeof(msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra) - sizeof(msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.pairAdyDistance) + (msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra.numAdyNodes * sizeof(AdjacentListInfo));
                                memcpy(&packetToSend.payload[sizeof(uint16_t) + sizeof(uint8_t) + sizeof(uint8_t)], &msgGreenGreen->msgBroadcast.SpanningBroadcast.msgDijkstra, sizeToCpy);
                            } else {

                                msgGreenGreen->msgBroadcast.protocol = forwardingProtocol; 
                                memcpy(&packetToSend.payload[sizeof(uint16_t) + sizeof(uint8_t)], &msgGreenGreen->msgBroadcast.protocol, sizeof(uint8_t));

                                msgGreenGreen->msgBroadcast.DataBroadcast.src = htons(msgGreenGreen->msgBroadcast.DataBroadcast.src);
                                msgGreenGreen->msgBroadcast.DataBroadcast.dest = htons(msgGreenGreen->msgBroadcast.DataBroadcast.dest);
                                msgGreenGreen->msgBroadcast.DataBroadcast.msgNum = htons(msgGreenGreen->msgBroadcast.DataBroadcast.msgNum); 
                                msgGreenGreen->msgBroadcast.DataBroadcast.RN = htons(msgGreenGreen->msgBroadcast.DataBroadcast.RN); 
                                
                                memcpy(packetToSend.payload + sizeof(uint16_t) + sizeof(uint8_t) + sizeof(uint8_t), &msgGreenGreen->msgBroadcast.DataBroadcast, sizeof(msgGreenGreen->msgBroadcast.DataBroadcast));
                            }

                            int bytesSent = sendto(adyNodes[adyNode].sock, &packetToSend, size + 1, 0, (struct sockaddr*)&adyNodes[adyNode].server, sizeof(struct sockaddr_in));
                            if (bytesSent < 0) 
                            {
                                // ERROR(Socket is not available, therefore, the message could not be sent) written to log
                                logBook->writeToLogWarning("LinkLayer:\tIn communication between green-green nodes: Error sending message to the socket of the adjacent node. That socket is not available yet.");
                            } 
                            statisticsDataPlane->addPacketSendTo(msgGreenGreen->nodeInter);
                        }
                    }
                }
                break;
                case heartBeatProtocol:
                {

                    logBook->writeToLogEvent("LinkLayer:\tThe protocol of the message proccessed was heartBeat");
                    bool find = false;
                    for(int adyNode = 0; adyNode < adyNodes.size() && !find; ++adyNode) {
                        if(msgGreenGreen->msgHeartBeat.nodeDest == adyNodes[adyNode].nodeNum) {
                            
                            find = true;

                            packetToSend.protocol = heartBeatProtocol;

                            msgGreenGreen->msgHeartBeat.nodeSrc = htons(msgGreenGreen->msgHeartBeat.nodeSrc);
                            msgGreenGreen->msgHeartBeat.nodeDest = htons(msgGreenGreen->msgHeartBeat.nodeDest);
                            msgGreenGreen->msgHeartBeat.code = msgGreenGreen->msgHeartBeat.code;
                            msgGreenGreen->msgHeartBeat.SN = htons(msgGreenGreen->msgHeartBeat.SN);
                            msgGreenGreen->msgHeartBeat.RN = htons(msgGreenGreen->msgHeartBeat.RN);
                            
                            memcpy(packetToSend.payload, &msgGreenGreen->msgHeartBeat, sizeof(msgGreenGreen->msgHeartBeat));
                            int bytesSent = sendto(adyNodes[adyNode].sock, &packetToSend, sizeof(msgGreenGreen->msgHeartBeat) + 1, 0, (struct sockaddr*)&adyNodes[adyNode].server, sizeof(struct sockaddr_in));
                            if (bytesSent < 0) 
                            {
                                // ERROR(Socket is not available, therefore, the message could not be sent) written to log
                                logBook->writeToLogWarning("LinkLayer:\tIn communication between green-green nodes: Error sending message to the socket of the adyacent node. That socket is not available yet.");
                            } 
                            statisticsDataPlane->addPacketSendTo(adyNodes[adyNode].nodeNum);
                        }
                    }
                }
                break;
                case spanningTreeProtocol:
                {

                    logBook->writeToLogEvent("LinkLayer:\tThe protocol of the message proccessed was spanning tree");
                    bool find = false;
                    for(int adyNode = 0; adyNode < adyNodes.size() && !find; ++adyNode) {
                        if(msgGreenGreen->msgSpanning.dest == adyNodes[adyNode].nodeNum) {
                            find = true;

                            packetToSend.protocol = spanningTreeProtocol;

                            msgGreenGreen->msgSpanning.src = htons(msgGreenGreen->msgSpanning.src);
                            msgGreenGreen->msgSpanning.dest = htons(msgGreenGreen->msgSpanning.dest);
                            msgGreenGreen->msgSpanning.SN = htons(msgGreenGreen->msgSpanning.SN);
                            msgGreenGreen->msgSpanning.RN = htons(msgGreenGreen->msgSpanning.RN);
                            msgGreenGreen->msgSpanning.question = msgGreenGreen->msgSpanning.question;
                            
                            memcpy(packetToSend.payload, &msgGreenGreen->msgSpanning, sizeof(msgGreenGreen->msgSpanning));

                            int bytesSent = sendto(adyNodes[adyNode].sock, &packetToSend, sizeof(msgGreenGreen->msgSpanning) + 1, 0, (struct sockaddr*)&adyNodes[adyNode].server, sizeof(struct sockaddr_in));
                            if (bytesSent < 0) 
                            {
                                // ERROR(Socket is not available, therefore, the message could not be sent) written to log
                                logBook->writeToLogWarning("LinkLayer:\tIn communication between green-green nodes: Error sending message to the socket of the adyacent node. That socket is not available yet.");
                            } 
                            statisticsDataPlane->addPacketSendTo(adyNodes[adyNode].nodeNum);
                        }
                    }
                }
                break;
                case stopAndWaitProtocol:
                {
                    // Message is sent to the corresponding green node 
                    bool find = false;
                    for (int adyNode = 0; adyNode < adyNodes.size() && !find; ++adyNode)
                    {
                        if (adyNodes[adyNode].nodeNum == msgGreenGreen->nodeInter) 
                        {
                            find = true;
                            //printf("LinkLayer:\tMessage is going to be sent to %d from %d\n", msgGreenGreen->msgStopandWait.dest, msgGreenGreen->msgStopandWait.src);
                            //printf("LinkLayer:\tNode inter is %d\n", msgGreenGreen->nodeInter);

                            packetToSend.protocol = stopAndWaitProtocol;

                            msgGreenGreen->msgStopandWait.dest = htons(msgGreenGreen->msgStopandWait.dest);
                            msgGreenGreen->msgStopandWait.src = htons(msgGreenGreen->msgStopandWait.src);
                            //msgGreenGreen->msgStopandWait.ID_1 = msgGreenGreen.ID_1;
                            //msgGreenGreen->msgStopandWait.ID_2 = msgGreenGreen.ID_2;
                            //msgGreenGreen->msgStopandWait.FLAGS = msgGreenGreen->msgStopandWait.FLAGS;
                            int sizeToSend = sizeof(msgGreenGreen->msgStopandWait) - sizeof(msgGreenGreen->msgStopandWait.payload) + msgGreenGreen->msgStopandWait.payload_size;

                            msgGreenGreen->msgStopandWait.payload_size = htons(msgGreenGreen->msgStopandWait.payload_size);

                            //strncpy(msgGreenGreen->msgStopandWait.payload, msgGreenGreen->msgStopandWait.payload, strlen(msgGreenGreen->msgStopandWait.payload));                            
                            
                            memcpy(&packetToSend.payload, &msgGreenGreen->msgStopandWait, sizeof(msgGreenGreen->msgStopandWait));
                            auto t_start = std::chrono::high_resolution_clock::now();
                            
                            int bytesSent = sendto(adyNodes[adyNode].sock, &packetToSend, sizeToSend + 1, 0, (struct sockaddr*)&adyNodes[adyNode].server, sizeof(struct sockaddr_in));
                            
                            auto t_end = std::chrono::high_resolution_clock::now();
                            
                            //std::cout << "\t\tTiempo de transmision: "<< std::chrono::duration<double, std::milli>(t_end-t_start).count() << " ms\n";                           
                            if (bytesSent < 0) 
                            {
                                // ERROR(Socket is not available, therefore, the message could not be sent) written to log
                                logBook->writeToLogWarning("LinkLayer:\tIn communication between green-green nodes: Error sending message to the socket of the adyacent node. That socket is not available yet.");
                            }
                            statisticsDataPlane->addPacketSendTo(adyNodes[adyNode].nodeNum);
                        }
                    }      
                }
                break;
                default:
                    logBook->writeToLogEvent("LinkLayer:\tThe protocol received does not correspond to any known protocol");
                    break;
            }
        }
    }
}


void LinkLayer::setGreenID(struct NodeInfo nodeGreen) {
    this->nodeGreen = nodeGreen;
}


void LinkLayer::setGreenAdys(std::vector<struct NodeInfo> nodesAdy) {
    adyNodesInfo = nodesAdy;
    for (int adyNode = 0; adyNode < nodesAdy.size(); ++adyNode) {
        struct SockUDPInfo sockUDP;
        sockUDP.nodeNum = nodesAdy[adyNode].nodeNum;
        sockUDP.sock = socket(AF_INET, SOCK_DGRAM, 0);

        if(sockUDP.sock < 0) {
            // ERROR(could not create socket for a certain adjacent node) written to log
            this->logBook->writeToLogError("LinkLayer:\tError creating socket of adjacent node");
        }
        // EVENT(UDP socket created successfully) written to log
        
        this->logBook->writeToLogEvent("LinkLayer:\tSocket UDP for one of the adjacent node created");

        sockUDP.server.sin_family = AF_INET; 
        inet_pton(AF_INET, nodesAdy[adyNode].ip, &(sockUDP.server.sin_addr));
        sockUDP.server.sin_port = htons(nodesAdy[adyNode].portUDP);
        
        // EVENT(successfully created socket for a certain adjacent node) written to log
        this->logBook->writeToLogEvent("LinkLayer:\tSocket UDP for adjacent node created successfully");
        adyNodes.emplace_back(sockUDP);
    }
}


void LinkLayer::execute(std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessageToDispatcher, std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlueNode, std::shared_ptr<int> continueLoop, std::shared_ptr<std::mutex> mutexForWritingInQueue, std::shared_ptr<sem_t> blueLayerSemaphore, std::shared_ptr<sem_t> semaphoreDispatcher) 
{
    this->forwardingFunctionality->setOriginalForwardingTable(adyNodesInfo, nodeGreen);

    this->broadcastFunctionality->setGreenNodeID(nodeGreen);
       
    std::thread messageDispatcher(&LinkLayer::dispatcherMessageGreenGreen, this, continueLoop, queueMessageToDispatcher, queueMessageToBlueNode, blueLayerSemaphore, semaphoreDispatcher);
   
    std::thread threadRecvMessageFromGreen(&LinkLayer::recvGreenMessageFromGreen, this, continueLoop, mutexForWritingInQueue, queueMessageToDispatcher, semaphoreDispatcher);

    std::thread threadMessageGreenGreen(&LinkLayer::sendGreenGreenMessages, this, continueLoop, queueMessageToBlueNode);

    std::thread threadForwarding(&Forwarding::execute, forwardingFunctionality, continueLoop, queueMessageToDispatcher, mutexForWritingInQueue, semaphoreDispatcher);

    std::thread threadPinkLayer(&PinkLayer::execute, pinkLayer, continueLoop, queueMessageToDispatcher, mutexForWritingInQueue, adyNodesInfo, nodeGreen, semaphoreDispatcher);    

    std::thread threadBroadcast(&Broadcast::execute, broadcastFunctionality, continueLoop, queueMessageToDispatcher, mutexForWritingInQueue, semaphoreDispatcher);    

    threadPinkLayer.join();
    threadBroadcast.join();
    threadForwarding.join();
    threadMessageGreenGreen.join();
    threadRecvMessageFromGreen.join();
    messageDispatcher.join();

    return;
}

std::shared_ptr<struct MessageForwarding> LinkLayer::copyMessage(MessageForwarding msg) {                    
    std::shared_ptr<struct MessageForwarding> msgToBlue = std::shared_ptr<struct MessageForwarding>(new struct MessageForwarding);
    memset(msgToBlue->msg, 0, sizeof(msgToBlue->msg));
    msgToBlue->src = msg.src;
    msgToBlue->dest = msg.src;
    msgToBlue->msgNum = msg.msgNum;
    strncpy(msgToBlue->msg, msg.msg, strlen(msg.msg));
    return msgToBlue;
}


struct ForwardingContainer LinkLayer::placeInContainer(std::shared_ptr<struct MessageGreenGreen> msg, uint8_t protocol)
{
    struct ForwardingContainer container;
    memset(&container, 0, sizeof(container));
    if(protocol == forwardingProtocol)
    {
        container.msgForwarding.src = msg->msgForwarding.src;
        container.msgForwarding.dest = msg->msgForwarding.dest;
        container.msgForwarding.msgNum = msg->msgForwarding.msgNum;
        strncpy(container.msgForwarding.msg, msg->msgForwarding.msg, strlen(msg->msgForwarding.msg));

        container.typeOfForwarding = 1;
    }
    else
    {
        container.msgStopAndWait.src = msg->msgStopandWait.src; 
        container.msgStopAndWait.dest = msg->msgStopandWait.dest; 

        container.msgStopAndWait.SN = msg->msgStopandWait.SN;
        container.msgStopAndWait.RN = msg->msgStopandWait.RN;

        container.msgStopAndWait.ID_1 = msg->msgStopandWait.ID_1;
        container.msgStopAndWait.ID_2 = msg->msgStopandWait.ID_2;

        container.msgStopAndWait.FLAGS = msg->msgStopandWait.FLAGS;
        
        container.msgStopAndWait.identifier = msg->msgStopandWait.identifier;

        container.msgStopAndWait.payload_size =msg->msgStopandWait.payload_size;
        memcpy(container.msgStopAndWait.payload, msg->msgStopandWait.payload, sizeof(msg->msgStopandWait.payload));

        container.msgForwarding = msg->msgForwarding;
        container.typeOfForwarding = 2;
    }
    return container;
}