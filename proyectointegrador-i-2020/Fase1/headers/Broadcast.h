#ifndef BROADCAST
#define BROADCAST

#include "../headers/Includes.h"

class Broadcast{
    enum Broadcastprotocols{
        BroadcastSpanningProtocol = 0,
        BroadcastDataProtocol
    };
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

        std::queue<struct BroadcastContainer> queueMessageGreenGreen;

        std::vector<uint16_t> nodesInSpanningTree;

        std::shared_ptr<LogBook> logBook;

        struct NodeInfo nodeGreen;

        sem_t sem_queue_message_green_green;

        // No escribir en el arbol al mismo momento que lee.
        sem_t sem_SpanningTree;//cambiar posteriormente a std::mutex sem_SpanningTree

                
        //Function: queues in the link layers queue the corresponding broadcast messages for their adjacent nodes.
        //Modifies: this function does not modify anything.
        //Requirements: queueMessagesToLink, which is used to push certain messages to the main queue and mutexToQueue, which is used as a control mechanism, since there are multiple processes using that queue.    
        //Returns: this function does not return anything.
        void resendMessageSpanning(BroadcastContainer msg, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessagesToLink, std::shared_ptr<std::mutex> mutexToQueue, std::shared_ptr<sem_t> dispatcherSemaphore); 

        //Function: queues in the link layers queue the corresponding broadcast messages about data packets.
        //Modifies: this function does not modify anything.
        //Requirements: queueMessagesToLink, which is used to push certain messages to the main queue and mutexToQueue, which is used as a control mechanism, since there are multiple processes using that queue.    
        //Returns: this function does not return anything.
        void resendMessageData(BroadcastContainer msg, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessagesToLink, std::shared_ptr<std::mutex> mutexToQueue, std::shared_ptr<sem_t> dispatcherSemaphore); 


    public:    
        Broadcast();
        Broadcast(std::shared_ptr<LogBook> logBook);
        
        //Function: continually checks if the program is still working and, if it is and there are messages in its queue, calls the method resendMessage, so the messages can be processed. 
        //Modifies: this function does not modify anything.
        //Requirements: continueLoop, which determines if the program continues its execution or not, queueMessagesToLink, which is used to push certain messages to the main queue
        //Requirements: mutexToQueue, which is used as a control mechanism, since there are multiple processes using that queue.    
        //Returns: this function does not return anything.
        void execute(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessagesToLink, std::shared_ptr<std::mutex> mutexToQueue, std::shared_ptr<sem_t> dispatcherSemaphore);
        
                
        //Function: pushes certain message into the broadcast functionality's queue
        //Modifies: this function does not modify anything.
        //Requirements: messageBroadcast, which is the broadcast message that is going to be inserted into the broadcast functionality's queue.
        //Returns: this function does not return anything.
        void pushMessageForBroadcast(struct BroadcastContainer);

        //Function: correctly sets the green nodes in the spanning tree of this broadcast functionality's green node. 
        //Modifies: this function modifies the nodesInSpanningTree atribute of this class.
        //Requirements: nodesInSpanningTree, which is a vector that contains all of the spanning tree nodes' information.   
        //Returns: this function does not return anything.
        void setSpanningTree(std::vector<uint16_t> nodesInSpanningTree);
        
        //Function: correctly sets the green Nodes ID of this broadcast functionality. 
        //Modifies: this function modifies the nodeGreen atribute of this class.
        //Requirements: greenNode, which contains the information of the green node of this broadcast funtionality. 
        //Returns: this function does not return anything.
        void setGreenNodeID(struct NodeInfo greenNode);

};
#endif