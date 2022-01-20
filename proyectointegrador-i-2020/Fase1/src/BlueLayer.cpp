#include "../headers/BlueLayer.h"

#define ERROR -1
#define SUCCESS 1
#define WRITE_END 1 
#define READ_END 0
#define MAX_INT16 65535

BlueLayer::BlueLayer(){
    this->logBook = NULL;
    this->greenNode = 0; 
}

BlueLayer::BlueLayer(std::shared_ptr<LogBook> parLogBook)
{
    this->logBook = parLogBook;
    this->greenNode = 0;
}

BlueLayer::~BlueLayer()
{
    logBook = NULL;
}

int BlueLayer::getSocketClientTCP()
{
    struct sockaddr_in serverTCP;
    struct sockaddr_in clientTCP;
    int sockTCP = socket(AF_INET, SOCK_STREAM, 0);
    
    if (greenNode == 0)
    {
        logBook->writeToLogError("BlueLayer:\tThis node's number hasn't been assigned");
        return ERROR;
    }

    if (sockTCP < 0)
    {
        // ERROR(TCP socket could not be created) written to log
        logBook->writeToLogError("BlueLayer:\tSocket TCP gave an error while being created");
        printf("BlueLayer:\tError creating socket for TCP.\n");
        return ERROR;
    }

    bzero((char *)&serverTCP, sizeof(serverTCP));

    uint16_t port = 5000 + greenNode;
    serverTCP.sin_family = AF_INET;
    serverTCP.sin_port = htons(port);
    
    inet_pton(AF_INET, ip.c_str(), &serverTCP.sin_addr);
    
    if (bind(sockTCP, (struct sockaddr *)&serverTCP, sizeof(serverTCP)) < 0)
    {
        // ERROR(TCP socket could not be binded) written to log
        logBook->writeToLogError("BlueLayer:\tSocket TCP gave an error while being binded");
        printf("BlueLayer:\tError binding the TCP server.\n");
        return ERROR;
    }

    // EVENT(TCP socket has been successfully binded) written to log
    logBook->writeToLogEvent("BlueLayer:\tThe TCP socket has been successfully binded");

    if (listen(sockTCP, 1) < 0)
    {
        // ERROR(TCP error while listening for incoming connections) written to log
        logBook->writeToLogError("BlueLayer:\tServer TCP gave an error while listening for incoming connections");
        printf("BlueLayer:\tError listening the TCP server.\n");
        return ERROR;
    }

    // EVENT(TCP socket is now listening for incoming connections) written to log
    logBook->writeToLogEvent("BlueLayer:\tThis side of the TCP socket is now listening for incoming connections");

    bzero((char *)&clientTCP, sizeof(clientTCP));
    socklen_t len = sizeof(struct sockaddr_in);
    sockClient = accept(sockTCP, (struct sockaddr *)&clientTCP, &len);

    // EVENT(Connection between green and blue node establsihed successfully) written to log
    logBook->writeToLogEvent("BlueLayer:\tThe TCP server received a connection from a client. The connection between the green node and blue node has been established");
    return SUCCESS;
}


//Establish connection with node blue (setTCP to linkLayer) & receive incoming messages from user and change format of the message(a msgGreenGreen).
void BlueLayer::recvMessageFromBlue(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueToDispatcher,std::shared_ptr<std::mutex> mutexForWritingInQueue, std::shared_ptr<sem_t> dispatcherSemaphore)//::recvMessageFromBlue(int pipeBlueLink)
{
    while(*continueLoop) 
    {
        srand((unsigned) time(0)); 
        struct MessageGreenBlue msgReceive;
        // Estar recibiendo los mensajes del azul
        memset(&msgReceive, 0, sizeof(msgReceive));
        read(sockClient, &msgReceive, sizeof(msgReceive));
        // EVENT(TCP socket has received a message) written to log
        logBook->writeToLogEvent("BlueLayer:\tIn communication between blue-green nodes: The TCP socket has received a message coming from the blue node");
       
        std::shared_ptr<struct MessageGreenGreen> msgToSend = std::shared_ptr<struct MessageGreenGreen>(new struct MessageGreenGreen);
        msgToSend->protocol = -1;
        msgToSend->msgForwarding.src = -1;
        msgToSend->msgForwarding.msgNum = -1;
        msgToSend->msgForwarding.dest = -1;
        memset(msgToSend->msgForwarding.msg, 0, sizeof(msgToSend->msgForwarding.msg));
        msgToSend->msgBroadcast.nodeSrc = -1;
        msgToSend->msgBroadcast.protocol = -1; 
        msgToSend->msgBroadcast.TTL = -1; 
        msgToSend->msgBroadcast.DataBroadcast.msgNum = -1; 
        msgToSend->msgBroadcast.DataBroadcast.RN = -1; 
        memset(msgToSend->msgBroadcast.DataBroadcast.msg, 0, sizeof(msgToSend->msgBroadcast.DataBroadcast.msg));
        memset(msgToSend->msgSandbox.spiderLuggage, 0, sizeof(msgToSend->msgSandbox.spiderLuggage)); 

        if (msgReceive.dest == 0) 
        {
            // EVENT(TCP socket is no longer receiving messages from the blue node) written to log
            logBook->writeToLogEvent("BlueLayer:\tIn communication between blue-green nodes: The TCP socket is not receiving any more messages coming from the blue node");

            *continueLoop = 0;
            if (close(sockClient) < 0){
                logBook->writeToLogError("BlueLayer:\tIn communication between blue-green nodes: Error closing socket.");
                printf("BlueLayer:\tError closing socket\n");
            }
        }
        else if (msgReceive.dest == MAX_INT16)
        {
            msgToSend->protocol = broadcastProtocol; 
            msgToSend->msgBroadcast.protocol = 1; 
            //BroadcastData Protocol
            msgToSend->msgBroadcast.nodeSrc = greenNode; 
            msgToSend->msgBroadcast.TTL = 5; 
            msgToSend->msgBroadcast.DataBroadcast.src = greenNode;
            msgToSend->msgBroadcast.DataBroadcast.dest = MAX_INT16;
            msgToSend->msgBroadcast.DataBroadcast.msgNum = msgReceive.msgNum; 
            msgToSend->msgBroadcast.DataBroadcast.RN = ( rand() % MAX_INT16 )+1; 
            strncpy(msgToSend->msgBroadcast.DataBroadcast.msg, msgReceive.msg, strlen(msgReceive.msg)); 
        }
        else if(msgReceive.src == MAX_INT16)
        {
            msgToSend->protocol = sandboxToBlueProtocol;
            strncpy(msgToSend->msgSandbox.spiderLuggage, msgReceive.msg, strlen(msgReceive.msg));
        }
        else                                                        
        {                                                                                                                                                                                                                                                                                                                                                                   
            msgToSend->protocol = forwardingProtocol;
            msgToSend->msgForwarding.dest = msgReceive.dest; 
            msgToSend->msgForwarding.src = greenNode;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            msgToSend->msgForwarding.msgNum = msgReceive.msgNum;
            strncpy(msgToSend->msgForwarding.msg, msgReceive.msg, strlen(msgReceive.msg));                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
        }
        mutexForWritingInQueue->lock();
        queueToDispatcher->push(msgToSend);
        sem_post(&(*dispatcherSemaphore));
        mutexForWritingInQueue->unlock();                                                                                                                                                                                                                                                                                                                                                                                                                 
    }
    return;
}

void BlueLayer::sendMessageToBlue(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlueNode, std::shared_ptr<sem_t> blueLayerSemaphore)
{
    while(*continueLoop)
    {                
        sem_wait(&(*blueLayerSemaphore));
        std::shared_ptr<MessageForwarding> message_to_send = std::shared_ptr<struct MessageForwarding>(new MessageForwarding);
        // if(!queueMessageToBlueNode->empty()) {
        message_to_send = queueMessageToBlueNode->front();
        queueMessageToBlueNode->pop();
                    
        if(message_to_send->dest == 0) 
        {
            *continueLoop = 0;
        } 
        else 
        {
            struct MessageGreenBlue msg = {};
            memset(&msg, 0, sizeof(msg));
            msg.dest = message_to_send->src; 
            msg.msgNum =  message_to_send->msgNum;
            strncpy(msg.msg, message_to_send->msg, strlen(message_to_send->msg));

            int sent = write(sockClient, &msg, sizeof(msg));
            if (sent < 0)
                logBook->writeToLogWarning("BlueLayer:\tIn communication between blue-green nodes: The TCP server hasnt been able to send the message to its blue node that it received from another green node");
            else
                logBook->writeToLogEvent("BlueLayer:\tIn communication between blue-green nodes: The TCP server has sent the message to its blue node that it received from another green node");
        }
        
    }
    return;
}


void BlueLayer::setGreenNum(uint16_t parNodeNum)
{
    this->greenNode = parNodeNum;
    logBook->writeToLogEvent("BlueLayer:\tThe node number has been asigned to this green node");
}

void BlueLayer::setGreenIP(char* parIP) 
{
    this->ip = parIP;
    logBook->writeToLogEvent("BlueLayer:\tThe IP has been asigned to this green node");
}

// Create forks for recv  and sends.
void BlueLayer::execute(std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessageToDispatcher, std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlueNode, std::shared_ptr<int> continueLoop,std::shared_ptr<std::mutex> mutexForWritingInQueue, std::shared_ptr<sem_t> blueLayerSemaphore, std::shared_ptr<sem_t> dispatcherSemaphore)
{
    if(getSocketClientTCP() < 0) {
        logBook->writeToLogError("BlueLayer:\tError establishing the connection with one blue node client");
        *continueLoop = 0;
        return;
    }

    std::thread threadRecvFromBlue(&BlueLayer::recvMessageFromBlue, this,  continueLoop, queueMessageToDispatcher, mutexForWritingInQueue, dispatcherSemaphore);

    std::thread threadSendToBlue(&BlueLayer::sendMessageToBlue, this, continueLoop, queueMessageToBlueNode, blueLayerSemaphore);

    threadRecvFromBlue.join();
    threadSendToBlue.join();

    close(this->sockClient);

    return;
}