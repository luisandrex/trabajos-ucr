#include "../headers/Forwarding.h"

Forwarding::Forwarding(){

}


Forwarding::Forwarding(std::shared_ptr<LogBook> logBook) {
    this->logBook = logBook; 
    sem_init(&sem_forwardingTable, 1, 1);
    sem_init(&sem_queue_message_green_green, 1, 0);
}


void Forwarding::execute(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessagesToLink, std::shared_ptr<std::mutex> mutexToQueue,  std::shared_ptr<sem_t> semaphoreDispatcher) {
    logBook->writeToLogEvent("Forwarding:\tBegun its execution");
    srand (time(NULL));
    while(*continueLoop) {
        sem_wait(&sem_queue_message_green_green);
        resendMessage(queueMessagesToLink, mutexToQueue, semaphoreDispatcher);
    }
    return;
}

void Forwarding::resendMessage(std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessagesToLink, std::shared_ptr<std::mutex> mutexToQueue,  std::shared_ptr<sem_t> semaphoreDispatcher) {
    struct ForwardingContainer container = queueMessageGreenGreen.front(); 
    queueMessageGreenGreen.pop();

    if(container.typeOfForwarding == FORWARDING_DATA) 
    {
        struct MessageForwarding message = container.msgForwarding; 
        logBook->writeToLogEvent("Forwarding:\tI have received a message and will process it");
        sem_wait(&sem_forwardingTable); 
        //printf("message dest is %d\n", message.dest);
        auto itr = forwardingTable.find(message.dest);  //itr->first = dest & itr->second = prev
        sem_post(&sem_forwardingTable); 

        if(itr != forwardingTable.end()) {
            std::shared_ptr<struct MessageGreenGreen> msg = std::shared_ptr<struct MessageGreenGreen>(new struct MessageGreenGreen);
            memset(msg->msgForwarding.msg, 0, sizeof(msg->msgForwarding.msg));
            msg->msgForwarding.dest = message.dest;
            msg->msgForwarding.src = message.src;
            msg->msgForwarding.msgNum = message.msgNum;
            strncpy(msg->msgForwarding.msg, message.msg, strlen(message.msg)); 
            msg->nodeInter = itr->second;
            msg->protocol = forwardingToNeighborProtocol; //cambie de 6 a 7
            bool resend = true;
            if(msg->nodeInter == 0) {
                if(msg->msgForwarding.src == greenNode.nodeNum) {
                    logBook->writeToLogWarning("Forwarding:\tThe message destinity was myself");
                    resend = false;
                } else {
                    msg->protocol = messageToBlueProtocol;  //cambie de 9 a 10  
                }
            }
            if(resend) {
                mutexToQueue->lock();
                queueMessagesToLink->push(msg); // std::shared_ptr<sem_t> semaphoreDispatcher
                sem_post(&(*semaphoreDispatcher));
                mutexToQueue->unlock();
                logBook->writeToLogEvent("Forwarding:\tThe forwarding functionality has found the intermediate green node to send the message");
            }
        } else {
            logBook->writeToLogWarning("Forwarding:\tThe green node to send the message does not exist in the graph");
        }
    }
    else if (container.typeOfForwarding == STOPWAIT)
    {
        int posibilidad_perdida = rand() % 100 + 1;
        //printf("Forwarding:\t\tLa posibilidad de que se pierda el paquete es de %d por ciento.\n", posibilidad_perdida);
        if(posibilidad_perdida <= 100)
        {
            //printf("Forwarding:\t\tRecibo un mensaje de tipo Stop&Wait en forwarding (no se pierde)\n");
            struct MessageStopAndWait message = container.msgStopAndWait; 
            logBook->writeToLogEvent("Forwarding:\tI have received a message and will process it");
            sem_wait(&sem_forwardingTable); 
            auto itr = forwardingTable.find(message.dest);  //itr->first = dest & itr->second = prev
            sem_post(&sem_forwardingTable); 

            if(itr != forwardingTable.end()) {
                std::shared_ptr<struct MessageGreenGreen> msg = std::shared_ptr<struct MessageGreenGreen>(new struct MessageGreenGreen);
                memset(msg->msgStopandWait.payload, 0, sizeof(msg->msgStopandWait.payload));

                msg->msgStopandWait.dest = message.dest;
                msg->msgStopandWait.src = message.src;

                msg->msgStopandWait.SN = message.SN;
                msg->msgStopandWait.RN = message.RN;

                msg->msgStopandWait.ID_1 = message.ID_1;
                msg->msgStopandWait.ID_2 = message.ID_2;

                msg->msgStopandWait.FLAGS = message.FLAGS;

                msg->msgStopandWait.identifier = message.identifier;

                msg->msgStopandWait.payload_size = message.payload_size;
                // strncpy(msg->msgStopandWait.payload, message.payload, strlen(message.payload));
                
                memcpy(msg->msgStopandWait.payload, message.payload, sizeof(message.payload));

                msg->nodeInter = itr->second;
                msg->protocol = SWFO; // si no es para mi, lo forwardeo
                if(msg->msgStopandWait.dest == greenNode.nodeNum) {
                    //printf("Forwarding:\t\tEl mensaje de Stop&Wait era para mi %d\n", message.dest);
                    msg->protocol = SWFP; //caso donde el s&w es para mi
                }
                // else
                //     printf("Forwarding:\t\tHago forwarding a mensaje de Stop&Wait\n");
                mutexToQueue->lock();
                queueMessagesToLink->push(msg); 
                sem_post(&(*semaphoreDispatcher));
                mutexToQueue->unlock();
                logBook->writeToLogEvent("Forwarding:\tThe forwarding functionality has found the intermediate green node to send the message");
            } else {
                logBook->writeToLogWarning("Forwarding:\tThe green node to send the message does not exist in the graph");
            }
        }
        // else
        // {
        //     printf("Forwarding:\t\tPerdi el paquete que iba entre %d y %d.\n",container.msgStopAndWait.src, container.msgStopAndWait.dest);
        // }
    }
    
}

void Forwarding::pushMessageForForwarding(struct ForwardingContainer message) {
    queueMessageGreenGreen.push(message); 
    sem_post(&sem_queue_message_green_green);
    logBook->writeToLogEvent("Forwarding:\tA forwarding message has been added to the queue to be processed"); 
}

void Forwarding::setForwardingTable(std::map<uint16_t, uint16_t> NewForwardingTable) {
    sem_wait(&sem_forwardingTable); 
    //copia la tabla
    forwardingTable = NewForwardingTable; 

    forwardingTable.insert({greenNode.nodeNum, 0});

    sem_post(&sem_forwardingTable); 
    logBook->writeToLogEvent("Forwarding:\tA new forwarding table has been set"); 
}

void Forwarding::setOriginalForwardingTable(std::vector<struct NodeInfo> adyNodes, struct NodeInfo nodeID) {
    greenNode = nodeID;
    for(int i = 0; i < adyNodes.size(); i++){
        forwardingTable.insert( {adyNodes[i].nodeNum, adyNodes[i].nodeNum} );
    }
    
    forwardingTable.insert({greenNode.nodeNum, 0});

    logBook->writeToLogEvent("Forwarding:\tForwarding table created with adjacent nodes"); 
}