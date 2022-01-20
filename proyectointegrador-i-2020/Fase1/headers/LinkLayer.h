#ifndef LINKLAYER
#define LINKLAYER

#include "PinkLayer.h"

#define ERROR -1
#define SUCCESS 1
#define READ_END 0
#define WRITE_END 1 
#define PATH_PIPE_LINK_FUNCT "../tmp/pipePinkFunct"

/**
  *Authors: Luis Sanchez Romero & Fernando Morales
  *Definition: This class is in charge of the communication between the green node and its adjacent nodes.          
  */class LinkLayer {
    /**
     * Definition:  A enumeration of the possible protocols that green node can send to its pink node.
     */

     enum Broadcastprotocols{
        BroadcastSpanningProtocol = 0,
        BroadcastDataProtocol
    };

    enum protocols {
        forwardingProtocol = 0,
        broadcastProtocol,
        spanningTreeProtocol,
        dijkstraProtocol,
        heartBeatProtocol,
        stopAndWaitProtocol,
        broadcastToNeighborProtocol,
        forwardingToNeighborProtocol,
        controlMessageHeartbeatProtocol,
        controlMessageSpanningTreeProtocol,
        messageToBlueProtocol,
        stopAndWaitToPink,
        stopAndWaitToNeighborProtocol, 
        sandboxFromBlueProtocol
    };        

    private:

        struct NodeInfo nodeGreen;

        std::shared_ptr<LogBook> logBook;
        
        std::shared_ptr<PinkLayer> pinkLayer;

        std::shared_ptr<Forwarding> forwardingFunctionality;

        std::shared_ptr<Broadcast> broadcastFunctionality;

        std::shared_ptr<StatisticsDataPlane> statisticsDataPlane;

        std::queue<std::shared_ptr<struct MessageGreenGreen>> queueMessagesToGreen; 

        std::vector<struct NodeInfo> adyNodesInfo;

        std::vector<struct SockUDPInfo> adyNodes;


        /**
         *  Function: It creates the UPD socket, and receives messages comming from other green nodes, and places them in the queueMesageToDispatcher to be processed by the dispatcher. 
         *  Modifies: modifies the queueMesageToDispatcher, since it places elements in it, and in case the method recvfrom() fails, it modifies the pointer continueLoop to 0.
         *  Requirements: It requires the pointer to continueLoop to know when will the program stop its execution, the queueMessageToDispatcher, to place messages comming from other green nodes in it and a mutex used to write in the queue, since there are several threads that can write in the queue at the same time. 
         *  Returns: this function does not return anything 
         */
        void recvGreenMessageFromGreen(std::shared_ptr<int> continueLoop, std::shared_ptr<std::mutex> mutexForWritingInQueue, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessageToDispatcher, std::shared_ptr<sem_t> semaphoreDispatcher);

        /**
         *  Function: pops messages from the pointer to queue queueMesageToDispatcher and sends them to its correspoding method, based on the first byte which indicates the protocol of the message. 
         *  Modifies: modifies the queueMesageToDispatcher, since it pops elements and sends them to its corresponding method, it also modifies continueLoop, if the protocol is -1, it indicates that the thread will end its execution.   
         *  Requirements: continueLoop, which is used to know when the execution of the program has stopped, queueMesageToDispatcher,which is a pointer to queue containing the messages that are processed by the dispatcher. 
         *  Returns: this function does not return anything 
         */        
        void dispatcherMessageGreenGreen(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMesageToDispatcher, std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlue, std::shared_ptr<sem_t> blueLayerSemaphore, std::shared_ptr<sem_t> semaphoreDispatcher);
        
        /**
         *  Function: send via UDP different messages between green nodes and queueing messages received that have a forwarding protocol onto the blue layer's queue
         *  Modifies: modifies both queues, the green one, where it sends the coresponding message to the corresponding green node, and the blue one, which it pushes the corresponding messages onto that queue  
         *  Requirements: continueLoop, which is used to know when the execution of the program has stopped, queueMessageGreenGreen, which contains all of the messages waiting to be sent
         *  Requirements: queueMessageToBlueNode, which is a pointer to a queue of messages used by the blue node exclusively
         *  Returns: this function does not return anything 
        */
        void sendGreenGreenMessages(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlueNode);

        struct ForwardingContainer placeInContainer(std::shared_ptr<struct MessageGreenGreen> msg, uint8_t protocol);


        std::shared_ptr<struct MessageForwarding> copyMessage(MessageForwarding msg);

    public:
        LinkLayer();

        LinkLayer(std::shared_ptr<LogBook> logBook);

        ~LinkLayer();
        
        /**
         * Function: set the green node attribute of the link layer
         * Modifies: sets the green node attribute of the link layer
         * Requirements: nodeGreen, which contains the green node's information  
         * Returns: this function does not return anything 
         * */
        void setGreenID(struct NodeInfo nodeGreen);

        /**
         *  Function: set the green node's adjacent nodes atribute of the link layer
         *  Modifies: sets the green node's adjacent nodes atribute of the link layer
         *  Requirements: nodeAdy, which is a vector that contains all of the info of the adjacent nodes
         *  Returns: this function does not return anything 
         */
        void setGreenAdys(std::vector<struct NodeInfo> nodesAdy);

        /**
         *  Function: create three processes: one that will be receiving messages, one for the dispatcher, and one for the sender  
         * Modifies: creates different shared structures that will be used in the execution of the link layer, such as the queue that it will be using to receive messages and the queue that it will be using to send messages  
         * Requirements: queueMessageToDispatcher, which is the pointer to the queue that the receiver will use to enqueue the messages it receives, queueMessageToBlueNode, which is a pointer to the queue that the blue layer will use to send messages to the blue node
         * Requirements: continueLoop, which a pointer to an int, that will determine if the program keeps executing or not,  and mutexForWritingInQueue, which is used as a control mechanism between the different processes which used the same queue
         * Returns: this function does not return anything 
        */
        void execute(std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessageToDispatcher, std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlueNode, std::shared_ptr<int> continueLoop, std::shared_ptr<std::mutex> mutexForWritingInQueue, std::shared_ptr<sem_t> semaphoreBlueLayer, std::shared_ptr<sem_t> semaphoreDispatcher); 

};

#endif