#ifndef FORWARDING
#define FORWARDING

#include "../headers/Includes.h"

#define FORWARDING_DATA 1
#define STOPWAIT 2
#define SWFP 11
#define SWFO 12

class Forwarding{
    private:
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
            stopAndWaitToNeighborProtocol
        }; 

        std::queue<struct ForwardingContainer> queueMessageGreenGreen;
        std::shared_ptr<int> continueLoop;

        std::shared_ptr<std::queue<std::shared_ptr<struct GreenMessageToSend>>> messagesToLink;

        std::map<uint16_t, uint16_t> forwardingTable;

        struct NodeInfo greenNode;

        std::shared_ptr<LogBook> logBook;
               
        sem_t sem_forwardingTable;

        sem_t sem_queue_message_green_green;

        std::string pathPipe;

        /*
        * Function: deques messages from queueMessageGreenGreen, looks up the destination node in the forwarding table, and pushes them to the queue "queueMessagesToLink" 
                    with the respective node they should be sent to 
        * Modifies: queueMessageGreenGreen and queueMessagesToLink
        * Requirements: shared queue of MessageGreenGreen messages (queueMessagesToLink), and  shared mutex
        * Returns: void
        * Author: Daniela Vargas
        */
        void resendMessage(std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessagesToLink, std::shared_ptr<std::mutex> mutexToQueue,  std::shared_ptr<sem_t> semaphoreDispatcher); 
        
    public:
        Forwarding(); 
        Forwarding(std::shared_ptr<LogBook> logBook);

        /*
        * Function: constantly checks queueMessageGreenGreen in order to process the messages 
        * Modifies: queueMessagesToLink
        * Requirements: variable that determines whether the cycle should continue or not (continueLoop), 
                        shared queue of MessageGreenGreen messages (queueMessagesToLink), and  shared mutex
        * Returns: void
        * Author: Daniela Vargas
        */
        void execute(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessagesToLink, std::shared_ptr<std::mutex> mutexToQueue, std::shared_ptr<sem_t> semaphoreDispatcher);
        
        /*
        * Function: adds the upcoming message to the queue “queueMessageGreenGreen” 
        * Modifies: queue “queueMessageGreenGreen”
        * Requirements: message of type MessageForwarding
        * Returns: void
        * Author: Daniela Vargas
        */
        void pushMessageForForwarding(struct ForwardingContainer);
        
        /*
        * Function: sets a new forwarding table
        * Modifies: forwardingTable
        * Requirements: initialized class and map (new forwarding table)
        * Returns: void
        * Author: Daniela Vargas
        */
        void setForwardingTable(std::map<uint16_t, uint16_t> forwardingTable);

        /*
        * Function: sets the initial forwarding table with the green node’s adyacent nodes
        * Modifies: forwardingTable
        * Requirements: initialized class, green node info and vector of adyacent nodes info) 
        * Returns: void
        * Author: Daniela Vargas
        */
        void setOriginalForwardingTable(std::vector<struct NodeInfo> nodeAdy, struct NodeInfo greenNode);

};
#endif