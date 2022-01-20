#include "../headers/Broadcast.h"
#define MAX_INT16 65535

Broadcast::Broadcast(){

}

Broadcast::Broadcast(std::shared_ptr<LogBook> logBook) {
    this->logBook = logBook;
    sem_init(&this->sem_SpanningTree, 1, 1);
    sem_init(&this->sem_queue_message_green_green,1,0);
}

void Broadcast::execute(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessagesToLink, std::shared_ptr<std::mutex> mutexToQueue, std::shared_ptr<sem_t> semaphoreDispatcher) {
    logBook->writeToLogEvent("Broadcast:\tBegun its execution ");
    while(*continueLoop){
        sem_wait(&sem_queue_message_green_green);
        BroadcastContainer msg = this->queueMessageGreenGreen.front();
        queueMessageGreenGreen.pop();
        logBook->writeToLogEvent("Broadcast:\tThe broadcast functionality is going to proccess a packet");
        if(msg.protocol == BroadcastSpanningProtocol){
            resendMessageSpanning(msg, queueMessagesToLink, mutexToQueue, semaphoreDispatcher);
        }
        else if (msg.protocol == BroadcastDataProtocol){
            resendMessageData(msg, queueMessagesToLink, mutexToQueue, semaphoreDispatcher);
        } else {
            logBook->writeToLogWarning("Broadcast:\tThe broadcast functionality is going to end its execution");
            *continueLoop = 0;
        }
    }
}

void Broadcast::resendMessageSpanning(BroadcastContainer msg, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessagesToLink, std::shared_ptr<std::mutex> mutexToQueue, std::shared_ptr<sem_t> dispatcherSemaphore) { 
    std::shared_ptr<struct MessageGreenGreen> msgToLink = std::shared_ptr<struct MessageGreenGreen>(new MessageGreenGreen());
    msgToLink->protocol = dijkstraProtocol; //3
    msgToLink->msgDijkstra.nodeSrc = msg.SpanningBroadcast.msgDijkstra.nodeSrc;
    msgToLink->msgDijkstra.numAdyNodes = msg.SpanningBroadcast.msgDijkstra.numAdyNodes;
    for(int counter = 0; counter < msgToLink->msgDijkstra.numAdyNodes; ++counter) {
        msgToLink->msgDijkstra.pairAdyDistance[counter] = msg.SpanningBroadcast.msgDijkstra.pairAdyDistance[counter];
    }

    mutexToQueue->lock();
    queueMessagesToLink->push(msgToLink);
    sem_post(&(*dispatcherSemaphore));
    mutexToQueue->unlock();
    
    msg.TTL--;
    std::shared_ptr< struct MessageGreenGreen> msgToSend;
    if(msg.TTL > 0){
        sem_wait(&sem_SpanningTree);
        for(auto adyNodeInSpanning : nodesInSpanningTree) {
            if(msg.nodeSrc != adyNodeInSpanning) {

                msgToSend = std::shared_ptr<MessageGreenGreen>(new MessageGreenGreen());
                
                msgToSend->protocol = broadcastToNeighborProtocol; //6
                msgToSend->msgBroadcast = BroadcastContainer(msg);

                msgToSend->msgBroadcast.protocol = BroadcastSpanningProtocol; 
                msgToSend->msgBroadcast.nodeSrc = nodeGreen.nodeNum;
                msgToSend->msgBroadcast.TTL = msg.TTL;

                msgToSend->msgBroadcast.SpanningBroadcast.msgDijkstra.nodeSrc = msg.SpanningBroadcast.msgDijkstra.nodeSrc;
                msgToSend->msgBroadcast.SpanningBroadcast.msgDijkstra.numAdyNodes = msg.SpanningBroadcast.msgDijkstra.numAdyNodes;
                for(int pairListAdy = 0; pairListAdy < msg.SpanningBroadcast.msgDijkstra.numAdyNodes; ++pairListAdy) {
                    msgToSend->msgBroadcast.SpanningBroadcast.msgDijkstra.pairAdyDistance[pairListAdy] = msg.SpanningBroadcast.msgDijkstra.pairAdyDistance[pairListAdy];
                }
        
                msgToSend->nodeInter = adyNodeInSpanning;

                mutexToQueue->lock();
                queueMessagesToLink->push(msgToSend);
                sem_post(&(*dispatcherSemaphore));
                mutexToQueue->unlock();
            }
            
    
        }
        sem_post(&sem_SpanningTree);
    }
    else{
        //escribir a logbook
        logBook->writeToLogEvent("Broadcast:\tStop broadcast of message because TTL <= 0");
    }
} 

void Broadcast::resendMessageData(BroadcastContainer msg, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessagesToLink, std::shared_ptr<std::mutex> mutexToQueue, std::shared_ptr<sem_t> dispatcherSemaphore){
    std::shared_ptr<struct MessageGreenGreen> msgToLink = std::shared_ptr<struct MessageGreenGreen>(new MessageGreenGreen());
    memset(msgToLink->msgBroadcast.DataBroadcast.msg, 0, sizeof(msgToLink->msgBroadcast.DataBroadcast.msg));
    
    msgToLink->protocol = messageToBlueProtocol; //10
    msgToLink->msgForwarding.src = msg.DataBroadcast.src; 
    msgToLink->msgForwarding.dest = nodeGreen.nodeNum; 
    msgToLink->msgForwarding.msgNum = msg.DataBroadcast.msgNum; 
    strncpy(msgToLink->msgForwarding.msg, msg.DataBroadcast.msg, strlen(msg.DataBroadcast.msg));

    mutexToQueue->lock();
    queueMessagesToLink->push(msgToLink);
    sem_post(&(*dispatcherSemaphore));
    mutexToQueue->unlock();
    
    msg.TTL--;
    std::shared_ptr< struct MessageGreenGreen> msgToSend;
    if(msg.TTL > 0){
        for(auto adyNodeInSpanning : nodesInSpanningTree) {
            if(msg.nodeSrc != adyNodeInSpanning) {
                msgToSend = std::shared_ptr<MessageGreenGreen>(new MessageGreenGreen());
                memset(msgToSend->msgBroadcast.DataBroadcast.msg, 0, sizeof(msgToSend->msgBroadcast.DataBroadcast.msg));
                msgToSend->protocol = broadcastToNeighborProtocol; //6
                msgToSend->msgBroadcast = BroadcastContainer(msg);

                msgToSend->msgBroadcast.protocol = BroadcastDataProtocol; 
                msgToSend->msgBroadcast.nodeSrc = nodeGreen.nodeNum;
                msgToSend->msgBroadcast.TTL = msg.TTL;

                msgToSend->msgBroadcast.DataBroadcast.src = msg.DataBroadcast.src;
                msgToSend->msgBroadcast.DataBroadcast.dest = msg.DataBroadcast.dest;
                msgToSend->msgBroadcast.DataBroadcast.msgNum = msg.DataBroadcast.msgNum;
                strncpy(msgToSend->msgBroadcast.DataBroadcast.msg, msg.DataBroadcast.msg, strlen(msg.DataBroadcast.msg));
        
                msgToSend->nodeInter = adyNodeInSpanning;

                mutexToQueue->lock();
                queueMessagesToLink->push(msgToSend);
                sem_post(&(*dispatcherSemaphore));
                mutexToQueue->unlock();
            }
        }
    }
    else{
        //escribir a logbook
        logBook->writeToLogEvent("Broadcast:\tStop broadcast of message because TTL <= 0");
    }

} 


void Broadcast::setGreenNodeID(struct NodeInfo greenNode) {
    this->nodeGreen = greenNode;
    //escribir a logbook
    logBook->writeToLogEvent("Broadcast:\tReceived my green node's ID");
}

void Broadcast::pushMessageForBroadcast(struct BroadcastContainer messageBroadcast) {
    this->queueMessageGreenGreen.push(messageBroadcast);
    //escribir a logbook    
    sem_post(&this->sem_queue_message_green_green);
    logBook->writeToLogEvent("Forwarding:\tA broadcast message has been added to the queue to be processed"); 
}

void Broadcast::setSpanningTree(std::vector<uint16_t> nodesInSpanningTree) {
    sem_wait(&sem_SpanningTree);
    this->nodesInSpanningTree = nodesInSpanningTree;
    logBook->writeToLogEvent("Broadcast:\tA new spanning tree table has been given to broadcast functionality");
    sem_post(&sem_SpanningTree);
}