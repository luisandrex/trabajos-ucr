#ifndef BLUELAYER
#define BLUELAYER

#include "LinkLayer.h"
 /**
  *Author: Luis Sanchez Romero
  *Definition: This class is in charge of the communication between the blue node (.py) and the link layer.          
  */
class BlueLayer{
    private:
 
        std::shared_ptr<LogBook> logBook;

        uint16_t greenNode;

        std::string ip;

        int sockClient;

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
            sandboxToBlueProtocol
        }; 

        /**
         * Function: This method generates the TCP socket to communicate the blue node with the blue layer. 
         * Modifies: This method doesn't modify anything. 
         * Requirements: It requires the ip to be set, so it can stablish the socket. 
         * Return: A value indicating an error. An 0 indicate no error, otherwise an error.
         */
        int getSocketClientTCP();

        /**
         * Function: This method receives incoming messages from the blue node, which are typed by the user and sends them to the dispatcher located in the link layer. 
         * Modifies: This method modifies the queue to dispatcher, since every time a new message is typed by the user and sent by the blue node, this method is in charge of receiving said message and places it in the queue.
         * Requirements: The pointer to continueLoop, which indicates when the program needs to be stopped, the pointer to queue to dispatcher, to place new elements in this queue and a pointer to mutex, so two different threads don't write in the queue at the same time. 
         * Returns: This method doesn't return a value.
         */
        void recvMessageFromBlue(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueToDispatcher,std::shared_ptr<std::mutex> mutexForWritingInQueue, std::shared_ptr<sem_t> dispatcherSemaphore); //Establish connection with node blue (setTCP to linkLayer) & receive incoming messages from user and change format of the message(a msgGreenGreen).
    
        /**
         * Function: This method has the task of reading the messages sent by other green nodes to resend them to its blue node.
         * Modifies: This method modifies the queue to dispatcher, since every time a new message is typed by the user and sent by the blue node, this method is in charge of receiving said message and places it in the queue.
         * Requirements: The pointer to continueLoop, which indicates when the program needs to be stopped, the pointer to queue message to blue node, so if there are new messages, it pops them and sends them to the blue node. 
         * Returns: This method doesn't return a value.
         */ 
        void sendMessageToBlue(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlueNode,  std::shared_ptr<sem_t> blueLayerSemaphore);
    public:

        BlueLayer();
        
        BlueLayer(std::shared_ptr<LogBook> parLogBook);

        ~BlueLayer();

        /**
         * Function: This method has the task of setting the attribute of the class greenNode
         * Modifies: This method modifies the attribute greenNode. 
         * Requirements: An integer which corresponds to this node's number. 
         * Returns: This method doesn't return a value.
         */ 
        void setGreenNum(uint16_t parNodeNum);

        /**
         * Function: This method has the task of setting the attribute of the class ip
         * Modifies: This method modifies the attribute ip. 
         * Requirements: A char array of size 16  which corresponds to this node's ip address. 
         * Returns: This method doesn't return a value.
         */ 
        void setGreenIP(char parIp[16]);

        /**
         *  Fuction: This method is in charge of creating two processes, which are the receiver from messages comming from the blue node and the process which sends messages to the blue node. 
         *  Modifies:This method doesn't modify any attributes or important variables.
         *  Requirements: A pointer to queue corresponding to the queue of messages that will be sent to the dispatcher, a pointer to queue of messages that will be sent to the blue node (.py), a mutex to write in the queue to dispatcher, a pointer to integer continue loop, which indicates based on its value, if the program is running or is being shut down. 
         *  Return: This method returns a value indicating an error. 0 indicates an successful execution, an -1 indicates that an error ocurred during its execution. 
         */
        void execute(std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessageToDispatcher, std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlueNode, std::shared_ptr<int> continueLoop,std::shared_ptr<std::mutex> mutexForWritingInQueue, std::shared_ptr<sem_t> blueLayerSemaphore, std::shared_ptr<sem_t> dispatcherSemaphore); 

        
};
#endif