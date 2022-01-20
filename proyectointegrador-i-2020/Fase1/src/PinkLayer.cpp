#include "../headers/PinkLayer.h"
#define MAX_INT16 65535

PinkLayer::PinkLayer(){

}

PinkLayer::PinkLayer(std::shared_ptr<LogBook> parLogBook, std::shared_ptr<Broadcast> parBroadcast, std::shared_ptr<Forwarding> parForwarding, std::shared_ptr<StatisticsDataPlane> parStatisticsDataPlane) {
    this->logBook = parLogBook;
    this->broadcast = parBroadcast;
    this->forwarding = parForwarding;
    this->statisticsDataPlane = parStatisticsDataPlane;
    semToWriteInPipe = std::shared_ptr<sem_t>(new sem_t);
    sem_init(semToWriteInPipe.get(), 1, 1);
    this->queuePinkMessagesForSpanningTree = std::shared_ptr<std::queue<std::shared_ptr<struct MessageSpanningTree>>>(new std::queue<std::shared_ptr<struct MessageSpanningTree>>());
    this->queuePinkMessagesForDijkstra = std::shared_ptr<std::queue<std::shared_ptr<struct MessageDijkstra>>>(new std::queue<std::shared_ptr<struct MessageDijkstra>>());
    this->queuePinkMessagesForHeartBeat = std::shared_ptr<std::queue<std::shared_ptr<struct MessageHeartBeat>>>(new std::queue<std::shared_ptr<struct MessageHeartBeat>>());
    this->queuePinkMessagesForSandbox = std::shared_ptr<std::queue<std::shared_ptr<struct MessageSandbox>>>(new std::queue<std::shared_ptr<struct MessageSandbox>>());
    this->queuePinkMessagesForStopAndWait = std::shared_ptr<std::queue<std::shared_ptr<struct MessageStopAndWait>>>(new std::queue<std::shared_ptr<struct MessageStopAndWait>>());

    this->pipeGreenPink = std::shared_ptr<int>(new int);
    this->pipePinkGreen = std::shared_ptr<int>(new int);
    char buff[MAXBUFF];
    sprintf(buff, "%d", getpid());
    pathPipeGreenPink = PATH_PIPE_GREEN_PINK;
    pathPipeGreenPink.append(buff);
    pathPipePinkGreen = PATH_PIPE_PINK_GREEN;
    pathPipePinkGreen.append(buff);
    sem_init(&semForHeartBeat, 1, 0);
    sem_init(&semForDijkstra, 1, 0);
    sem_init(&semForSpanningTree, 1, 0);
    sem_init(&semForStopAndWait, 1, 0);
    sem_init(&semForSandbox, 1, 0);
}

PinkLayer::~PinkLayer() {
    sem_close(semToWriteInPipe.get());
}

void PinkLayer::sendStopAndWait(std::shared_ptr<int> continueLoop, std::shared_ptr<int> parPipeGreenToPink)
{
    while(*continueLoop) {
        sem_wait(&semForStopAndWait);
        
        logBook->writeToLogEvent("PinkLayer:\tA stop and wait packet is going to be sent to Pink Node");
        std::shared_ptr<struct MessageStopAndWait> msgStopAndWait;
        msgStopAndWait = queuePinkMessagesForStopAndWait->front();
        queuePinkMessagesForStopAndWait->pop();
        if (msgStopAndWait->FLAGS != std::numeric_limits<uint8_t>::max()) 
        {
            //printf("PinkLayer:\t en pink layer meto un nuevo mensaje de Stop&Wait, SN = %d\n", msgStopAndWait->SN);
            int8_t protocol = protocolStopAndWait;
            sem_wait(semToWriteInPipe.get());
            write(*parPipeGreenToPink, &protocol, sizeof(protocol));
            write(*parPipeGreenToPink, &(*msgStopAndWait), sizeof(*msgStopAndWait));
            sem_post(semToWriteInPipe.get());
        }
    }
}

void PinkLayer::sendSandbox(std::shared_ptr<int> continueLoop, std::shared_ptr<int> parPipeGreenToPink){
    while(*continueLoop){
        sem_wait(&semForSandbox); 
        logBook->writeToLogEvent("PinkLayer:\tA sandbox packet is going to be sent to Pink Node");
        std::shared_ptr<struct MessageSandbox> msgSandbox;
        msgSandbox = queuePinkMessagesForSandbox->front();
        queuePinkMessagesForSandbox->pop(); 
        if (strlen(msgSandbox->spiderLuggage) != 0) { 
            uint8_t protocol = protocolSandboxFromBlue;
            sem_wait(semToWriteInPipe.get());
            write(*parPipeGreenToPink, &protocol, sizeof(protocol));
            write(*parPipeGreenToPink, &(*msgSandbox), sizeof(*msgSandbox));
            //printf("mando por pipes %zu bytes\n", );
            sem_post(semToWriteInPipe.get());    
        }
    }
}


void PinkLayer::sendHeartBeat(std::shared_ptr<int> continueLoop, std::shared_ptr<int> parPipeGreenToPink) {
    while(*continueLoop) {
        sem_wait(&semForHeartBeat);
        
        logBook->writeToLogEvent("PinkLayer:\tA packet about heartbeat is going to be sent to Pink Node");
        std::shared_ptr<struct MessageHeartBeat> msgHeartBeat;
        msgHeartBeat = queuePinkMessagesForHeartBeat->front();
        queuePinkMessagesForHeartBeat->pop();
        if (msgHeartBeat->nodeSrc != std::numeric_limits<uint16_t>::max()) {
            uint8_t protocol = protocolHeartBeat;
            sem_wait(semToWriteInPipe.get());
            write(*parPipeGreenToPink, &protocol, sizeof(protocol));
            write(*parPipeGreenToPink, &(*msgHeartBeat), sizeof(*msgHeartBeat));
            sem_post(semToWriteInPipe.get());    
        }
    }
}

void PinkLayer::sendDijkstra(std::shared_ptr<int> continueLoop, std::shared_ptr<int> parPipeGreenToPink) {
    while(*continueLoop) {
        sem_wait(&semForDijkstra);

        logBook->writeToLogEvent("PinkLayer:\tA packet for control plane about Dijkstra is going to be sent to Pink Node");
        std::shared_ptr<struct MessageDijkstra> msgDijkstra;
        msgDijkstra = queuePinkMessagesForDijkstra->front();
        queuePinkMessagesForDijkstra->pop();
        if(msgDijkstra->nodeSrc != std::numeric_limits<uint16_t>::max()) {
            uint8_t protocol = protocolDijkstra;
            sem_wait(semToWriteInPipe.get());
            write(*parPipeGreenToPink, &protocol, sizeof(protocol));
            write(*parPipeGreenToPink, &(*msgDijkstra), sizeof(*msgDijkstra));
            sem_post(semToWriteInPipe.get());
        }
    }
}

void PinkLayer::sendSpanningTree(std::shared_ptr<int> continueLoop, std::shared_ptr<int> parPipeGreenToPink) {
    while(*continueLoop) {
        
        sem_wait(&semForSpanningTree);

        logBook->writeToLogEvent("PinkLayer:\tA packet for control plane about the spanning tree is going to be sent to Pink Node");
        std::shared_ptr<struct MessageSpanningTree> msgSpanning;
        msgSpanning = queuePinkMessagesForSpanningTree->front();
        queuePinkMessagesForSpanningTree->pop();
        if (msgSpanning->question != std::numeric_limits<uint8_t>::max())
        {
            uint8_t protocol = protocolSpanningTree;
            sem_wait(semToWriteInPipe.get());
            write(*parPipeGreenToPink, &protocol, sizeof(protocol));
            write(*parPipeGreenToPink, &(*msgSpanning), sizeof(*msgSpanning));
            sem_post(semToWriteInPipe.get());
        }
    }   
}

void PinkLayer::sender(std::shared_ptr<int> pipeGreenToPink, std::shared_ptr<int> continueLoop) {

    std::thread threadMessageSpanning(&PinkLayer::sendSpanningTree, this, continueLoop, pipeGreenToPink);

    std::thread threadMessageDijkstra(&PinkLayer::sendDijkstra, this, continueLoop, pipeGreenToPink);

    std::thread threadMessageHeartBeat(&PinkLayer::sendHeartBeat, this, continueLoop, pipeGreenToPink);

    std::thread threadMessageStopAndWait(&PinkLayer::sendStopAndWait,this,continueLoop, pipeGreenPink);

    std::thread threadMessageSandbox(&PinkLayer::sendSandbox,this,continueLoop, pipeGreenPink);

    threadMessageStopAndWait.join();

    threadMessageHeartBeat.join();

    threadMessageDijkstra.join();

    threadMessageSpanning.join();

    threadMessageSandbox.join(); 

    int8_t notifyEndPink = -1;
    write(*pipeGreenPink, &notifyEndPink, sizeof(int8_t));
}

void PinkLayer::getStatisticsOfPacketsSentToNeighbor() {
    std::map<uint16_t, uint16_t> counterPacketsSentTo = statisticsDataPlane->getCounterPacketsSendTo();
    logBook->writeToLogEvent("PinkLayer:\tThe statistics about the counter of packets sent to each neighbor is goint to be send to the Pink Node");
    sem_wait(this->semToWriteInPipe.get());
    uint8_t protocol = protocolPacketsSentToNeighbor;
    write(*pipeGreenPink, &protocol, sizeof(protocol));       
    for(auto statistics : counterPacketsSentTo) {
        struct PairOfStatisticsElements dataToSend({statistics.first, statistics.second});
        write(*pipeGreenPink, &dataToSend, sizeof(dataToSend)); 
    }
    //printf("mando\n");
    struct PairOfStatisticsElements dataToSend({std::numeric_limits<uint16_t>::max(), std::numeric_limits<uint16_t>::max()});
    write(*pipeGreenPink, &dataToSend, sizeof(dataToSend));
    sem_post(this->semToWriteInPipe.get());
    //logBook->writeToLogEvent("PinkLayer:\tThe statistics about the counter of packets sent to each neighbor is sent to the Pink Node");
}
        
void PinkLayer::getStatisticsOfPacketsRecvFromNeighbor() {
    std::map<uint16_t, uint16_t> counterPacketsRecvFrom = statisticsDataPlane->getCounterPacketsRecvFrom();
    logBook->writeToLogEvent("PinkLayer:\tThe statistics about the counter of packets received from each of the nodes by which I'm neighbor is going to be sent to the Pink Node");
    sem_wait(this->semToWriteInPipe.get());
    uint8_t protocol = protocolPacketsRecvFromNeighbor;
    write(*pipeGreenPink, &protocol, sizeof(protocol));  
    for(auto statistics : counterPacketsRecvFrom) {
        struct PairOfStatisticsElements dataToSend({statistics.first, statistics.second});
        write(*pipeGreenPink, &dataToSend, sizeof(dataToSend)); 
    }
    struct PairOfStatisticsElements dataToSend({std::numeric_limits<uint16_t>::max(), std::numeric_limits<uint16_t>::max()});
    write(*pipeGreenPink, &dataToSend, sizeof(dataToSend));
    sem_post(this->semToWriteInPipe.get());
    //logBook->writeToLogEvent("PinkLayer:\tThe statistics about the counter of packets received from each of the nodes by which I'm neighbor is sent to the Pink Node");
}

void PinkLayer::getStatisticsOfPacketsForwardingAndBroadcast() {
    std::pair<uint16_t, uint16_t> counterPacketsForwardingAndBroadcast = statisticsDataPlane->getCounterPacketsBroadcastAndForwarding();
    logBook->writeToLogEvent("PinkLayer:\tThe statistics about the counter of forwarding and broadcast packets is going to be sent to the Pink Node");
    sem_wait(this->semToWriteInPipe.get());
    struct PairOfStatisticsElements dataToSend({counterPacketsForwardingAndBroadcast.first, counterPacketsForwardingAndBroadcast.second}); 
    uint8_t protocol = protocolPacketsForwardingBroadcast;
    write(*pipeGreenPink, &protocol, sizeof(protocol));  
    write(*pipeGreenPink, &dataToSend, sizeof(dataToSend));
    //logBook->writeToLogEvent("PinkLayer:\tThe statistics about the counter of forwarding and broadcast packets is sent to the Pink Node");
    sem_post(this->semToWriteInPipe.get());
}

void PinkLayer::receiver(std::shared_ptr<int> parPipePinkToGreen, std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue< std::shared_ptr<struct MessageGreenGreen>>> queueControlMessages, std::shared_ptr<std::mutex> mutexToQueue, std::shared_ptr<sem_t> semaphoreDispatcher) {
    while(*continueLoop) {
        uint8_t protocol = 0;
        read(*parPipePinkToGreen, &protocol, sizeof(protocol));
        //printf("Protocolo recv es %d\n", protocol);
        logBook->writeToLogEvent("PinkLayer:\tProtocol of the message sent by the pink node read");
        switch (protocol) {
            case protocolForwarding:
                {
                    logBook->writeToLogEvent("PinkLayer:\tThe protocol received indicates that a new forwarding table is going to be set up");
                    struct MessageGreenPink msgForwarding;
                    bool continueReading = true;
                    forwardingTable.clear();
                    while (continueReading) {
                        read(*parPipePinkToGreen, &msgForwarding, sizeof(msgForwarding));
                        if(msgForwarding.dest == std::numeric_limits<uint16_t>::max()) {
                            continueReading = false;
                        } else {
                            forwardingTable.emplace(msgForwarding.dest, msgForwarding.nodeInter);
                        }
                    }
                    forwarding->setForwardingTable(forwardingTable);
                }
                break;
            case protocolBroadcastTable:
                {
                    logBook->writeToLogEvent("PinkLayer:\tThe protocol received indicates that a new spanning tree is going to be set up");
                    uint16_t nodeInSpanning;
                    bool continueReading = true;
                    nodesInSpanningTree.clear();
                    while(continueReading) {
                        read(*parPipePinkToGreen, &nodeInSpanning, sizeof(nodeInSpanning));
                        if(nodeInSpanning == std::numeric_limits<uint16_t>::max()) {
                            continueReading = false;
                        } else {
                            nodesInSpanningTree.emplace_back(nodeInSpanning);
                        }
                    }
                    broadcast->setSpanningTree(nodesInSpanningTree);
                }
                break;
            case protocolBroadcast:
                logBook->writeToLogEvent("PinkLayer:\tThe protocol received indicates that a the new state of the spanning tree is going to be sent to all neighbors in the spanning tree");
                struct BroadcastFromPink msgPinkBroadcast;
                read(*parPipePinkToGreen, &msgPinkBroadcast, sizeof(msgPinkBroadcast));
                struct BroadcastContainer msgBroadcast; 
                msgBroadcast.nodeSrc = msgPinkBroadcast.nodeSrc; 
                msgBroadcast.TTL = msgPinkBroadcast.TTL; 
                msgBroadcast.protocol = 0; //SpanningBroadcastProtocol
                //msgBroadcast.SpanningBroadcast.protocol = msgPinkBroadcast.protocol; 
                msgBroadcast.SpanningBroadcast.msgDijkstra = msgPinkBroadcast.msgDijkstra;
                broadcast->pushMessageForBroadcast(msgBroadcast);
                break;
            case protocolSpanningQuestion:
                {
                    logBook->writeToLogEvent("PinkLayer:\tThe protocol received indicates that a spanning tree question is going to be sent to a neighbor");                
                    std::shared_ptr<MessageGreenGreen> msgSpanning = std::shared_ptr<MessageGreenGreen>(new MessageGreenGreen);
                    read(*parPipePinkToGreen, &msgSpanning->msgSpanning, sizeof(msgSpanning->msgSpanning));
                    msgSpanning->protocol = 9;
                    mutexToQueue->lock();
                    queueControlMessages->push(msgSpanning);
                    sem_post(&(*semaphoreDispatcher));
                    mutexToQueue->unlock();
                }
                break;
            case protocolHeartBeatQuestion:
                {
                    logBook->writeToLogEvent("PinkLayer:\tThe protocol received indicates that a question about protocol heart beat is going to be sent to one of the neighbors");
                    std::shared_ptr<MessageGreenGreen> msgHeartBeat = std::shared_ptr<MessageGreenGreen>(new MessageGreenGreen);
                    read(*parPipePinkToGreen, &msgHeartBeat->msgHeartBeat, sizeof(msgHeartBeat->msgHeartBeat));
                    msgHeartBeat->protocol = 8;
                    mutexToQueue->lock();
                    queueControlMessages->push(msgHeartBeat);
                    sem_post(&(*semaphoreDispatcher));
                    mutexToQueue->unlock();
                    logBook->writeToLogEvent("PinkLayer:\tThe packet received from the pink node was a question to one of my neighbors about the heart beat");
                }
                break;
            case protocolStopAndWait:
            {
                logBook->writeToLogEvent("PinkLayer:\tThe protocol received indicates that a question about protocol stop and wait is going to be sent to one of the neighbors");
                std::shared_ptr<MessageGreenGreen> msgStopAndWait = std::shared_ptr<MessageGreenGreen>(new MessageGreenGreen);
                read(*parPipePinkToGreen, &msgStopAndWait->msgStopandWait, sizeof(msgStopAndWait->msgStopandWait));
                msgStopAndWait->protocol = protocolStopAndWait;
                //printf("PinkLayer:\tvoy a enviar mensaje de Stop&Wait a LinkLayer, src = %d y dest = %d\n", msgStopAndWait->msgStopandWait.src,msgStopAndWait->msgStopandWait.dest );
                mutexToQueue->lock();
                queueControlMessages->push(msgStopAndWait);
                //printf("\n\n\npayload de stop and wait es %s\n\n\n", msgStopAndWait->msgStopandWait.payload);
                sem_post(&(*semaphoreDispatcher));
                mutexToQueue->unlock();
                logBook->writeToLogEvent("PinkLayer:\tThe packet received from the pink node was a question to one of my neighbors about the heart beat");
            }
            break;
            case protocolSandboxToBlue:
            {
                logBook->writeToLogEvent("PinkLayer:\tThe protocol received indicates that the spiders luggage is going to be sent to the blue node");
                std::shared_ptr<MessageGreenGreen> msgSandbox = std::shared_ptr<MessageGreenGreen>(new MessageGreenGreen);
                memset(&msgSandbox->msgSandbox, 0, sizeof(msgSandbox->msgSandbox));
                read(*parPipePinkToGreen, &msgSandbox->msgSandbox, sizeof(msgSandbox->msgSandbox));
                msgSandbox->protocol = 10; //message to blue protocol
                msgSandbox->msgForwarding.dest = MAX_INT16; 
                msgSandbox->msgForwarding.src = MAX_INT16; 
                msgSandbox->msgForwarding.RN = MAX_INT16; 
                msgSandbox->msgForwarding.msgNum = MAX_INT16; 
                strncpy(msgSandbox->msgForwarding.msg, msgSandbox->msgSandbox.spiderLuggage, strlen(msgSandbox->msgSandbox.spiderLuggage)); 
                //PONER EL MENSAJE EN EL DISPATCHER 
                mutexToQueue->lock();
                queueControlMessages->push(msgSandbox);
                sem_post(&(*semaphoreDispatcher));
                mutexToQueue->unlock();
                logBook->writeToLogEvent("PinkLayer:\tSandbox message sent to link layer");
            }
            break; 
            case protocolPacketsFromSrcToSrc:
                //logBook->writeToLogEvent("PinkLayer:\tThe statistics about the counter of packets received from each source to another source is going to been processed");
                getStatisticsOfPacketsRecvFromNeighbor();
            break;
            case protocolPacketsSentToNeighbor:
                //logBook->writeToLogEvent("PinkLayer:\tThe statistics about the counter of packets sent to each of the adjacent nodes is going to been processed");
                getStatisticsOfPacketsSentToNeighbor();
            break;
            case protocolPacketsRecvFromNeighbor:
                //logBook->writeToLogEvent("PinkLayer:\tThe statistics about the counter of packets received from each of the adjacent nodes is going to been processed");
                getStatisticsOfPacketsRecvFromNeighbor();
            break;
            case protocolPacketsForwardingBroadcast:
                //logBook->writeToLogEvent("PinkLayer:\tThe statistics about the counter of forwarding and broadcast packets is going to been processed");
                getStatisticsOfPacketsForwardingAndBroadcast();
            break;                
            default:
                logBook->writeToLogWarning("PinkLayer:\tThe green node has been finished");
                break;
        }
    }
}

void PinkLayer::sendAdyacentInfo(std::shared_ptr<int> parPipeGreenPink) {
    uint8_t protocol = protocolGreenInfo;
    write(*parPipeGreenPink, &protocol, sizeof(protocol));
    write(*parPipeGreenPink, &greenNode, sizeof(greenNode));
    logBook->writeToLogEvent("PinkLayer:\tThe information of myself has been sent to the pink node");
    protocol = protocolAdyacentsInfo;
    write(*parPipeGreenPink, &protocol, sizeof(protocol));
    for(auto adyNode : this->adyNodesInfo) {
        write(*parPipeGreenPink, &adyNode, sizeof(adyNode));
    }
    struct NodeInfo nodeEnd;
    nodeEnd.nodeNum = -1; 
    nodeEnd.portUDP = -1;
    write(*parPipeGreenPink, &nodeEnd, sizeof(nodeEnd));
    logBook->writeToLogEvent("PinkLayer:\tThe information of the adyacent nodes sent to the pink node successfully");
}

//std::thread threadPinkLayer(&PinkLayer::execute, pinkLayer, continueLoop, queueMessageToDispatcher, mutexForWritingInQueue, adyNodesInfo, nodeGreen);    
int PinkLayer::execute(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue< std::shared_ptr<struct MessageGreenGreen>>> queueControlMessages, std::shared_ptr<std::mutex> mutexToQueue, std::vector<struct NodeInfo> adyNodesInfo, struct NodeInfo greenNode,  std::shared_ptr<sem_t> semaphoreDispatcher) {
    this->greenNode = greenNode;
    this->adyNodesInfo = adyNodesInfo;
    
    if(mkfifo(pathPipeGreenPink.c_str(), 0666) < 0) {
        logBook->writeToLogError("PinkLayer:\tError creating pipe between Green and Pink Node");
        return -1;
    }
    if(mkfifo(pathPipePinkGreen.c_str(), 0666) < 0) {
        logBook->writeToLogError("PinkLayer:\tError creating pipe between Pink and Green Node");
        return -2; 
    }

    std::string pathLogBookPink = PATH_LOGBOOK_PINK;
    char buff[MAXBUFF];
    sprintf(buff, "%d.txt", greenNode.nodeNum);
    pathLogBookPink.append(buff);
    if(*continueLoop){
        pid_t forkID = fork();
        if(forkID < 0) {
            logBook->writeToLogError("PinkLayer:\tError creating the pipe for the communication between Green node and Pink node");
            return -5;
        } else if(forkID == 0) {
            execlp(PYTHON3, PYTHON3, PINKNODE, pathPipeGreenPink.c_str(), pathPipePinkGreen.c_str(), pathLogBookPink.c_str(), NULL);
        } else { 

            if((*pipeGreenPink = open(pathPipeGreenPink.c_str(), O_WRONLY)) < 0) {
                logBook->writeToLogError("PinkLayer:\tError opening pipe between Green and Pink Node");
                return -3;
            }

            logBook->writeToLogEvent("PinkLayer:\tPipe between green node and pink node was successfully opened");

            if((*pipePinkGreen = open(pathPipePinkGreen.c_str(), O_RDONLY)) < 0) {
                logBook->writeToLogError("PinkLayer:\tError opening pipe between Pink and Green Node");
                return -4; 
            }

            logBook->writeToLogEvent("PinkLayer:\tPipe between pink node and pink node was successfully opened");

            sendAdyacentInfo(pipeGreenPink);

            std::thread threadSender(&PinkLayer::sender, this, pipeGreenPink, continueLoop);
            logBook->writeToLogEvent("PinkLayer:\tThe thread in charge of sending the messages to the pink node initialized");
            std::thread threadReceiver(&PinkLayer::receiver, this, pipePinkGreen, continueLoop, queueControlMessages, mutexToQueue, semaphoreDispatcher);
            logBook->writeToLogEvent("PinkLayer:\tThe thread in charge of receiving the messages from the pink node initialized");


            threadSender.join();
            
            threadReceiver.join();

            wait(NULL);
        }
    }
    remove(pathPipePinkGreen.c_str());
    remove(pathPipeGreenPink.c_str());
}

void PinkLayer::pushMessagePinkForSpanningTree(struct MessageSpanningTree parMessage) {
    std::shared_ptr<MessageSpanningTree> msg = std::shared_ptr<MessageSpanningTree>(new MessageSpanningTree(parMessage));
    queuePinkMessagesForSpanningTree->emplace(msg);
    sem_post(&semForSpanningTree);
}

void PinkLayer::pushMessagePinkForDijkstra(struct MessageDijkstra parMessage) {
    std::shared_ptr<MessageDijkstra> msg = std::shared_ptr<MessageDijkstra>(new MessageDijkstra(parMessage));
    queuePinkMessagesForDijkstra->emplace(msg);
    sem_post(&semForDijkstra);
}

void PinkLayer::pushMessagePinkForHeartBeat(struct MessageHeartBeat parMessage) {
    std::shared_ptr<MessageHeartBeat> msg = std::shared_ptr<MessageHeartBeat>(new MessageHeartBeat(parMessage));
    queuePinkMessagesForHeartBeat->push(msg);
    sem_post(&semForHeartBeat);
}

void PinkLayer::pushMessagePinkForStopAndWait(struct MessageStopAndWait parMessage){
    std::shared_ptr<MessageStopAndWait> msg = std::shared_ptr<MessageStopAndWait>(new MessageStopAndWait(parMessage));
    queuePinkMessagesForStopAndWait->push(msg);
    sem_post(&semForStopAndWait);
    //printf("PinkLayer:\tMeto mensaje en cola de Stop&Wait de pink\n");
}

void PinkLayer::pushMessageForSandbox(struct MessageSandbox parMessage){
    std::shared_ptr<MessageSandbox> msg = std::shared_ptr<MessageSandbox>(new MessageSandbox(parMessage));
    queuePinkMessagesForSandbox->push(msg);
    sem_post(&semForSandbox);
}
