#ifndef NETWORKLAYER
#define NETWORKLAYER

#include "Forwarding.h"
#include "Broadcast.h"

#define PATH_PIPE_PINK_GREEN "../tmp/pipePG"
#define PATH_PIPE_GREEN_PINK "../tmp/pipeGP"
#define PATH_LOGBOOK_PINK "../etc/LogBookPink"
#define MAXBUFF 50 
#define SPANNINGLINKPROTO 2
#define HEARTBEATLINKPROTO 4
#define PYTHON3 "python3"
#define PINKNODE "PinkNode.py"
#define protocolSandboxToBlue 10

/**
 * Definition:      A class in charge of the communication between the green node and its pink node.
 * Author:          Jose Andres Viquez
 */
class PinkLayer{
    private:
        /**
         * Definition:  A enumeration of the possible protocols that green node can send to its pink node.
         */
        enum protocolsGreenToPink{
            protocolGreenInfo = 0,
            protocolAdyacentsInfo,
            protocolSpanningTree,
            protocolDijkstra,
            protocolHeartBeat,
            protocolStopAndWait,
            protocolPacketsFromSrcToSrc,
            protocolPacketsSentToNeighbor,
            protocolPacketsRecvFromNeighbor,
            protocolPacketsForwardingBroadcast, 
            protocolSandboxFromBlue //REVISAAR
        };
        /**
         * Definition:  A enumeration with the possibles protocols that green node can received from its pink node.
         */
        enum protocolsPinkToGreen{
            protocolForwarding = 0,     // Forwarding table
            protocolBroadcastTable,     // Packets with update of the spanning tree table
            protocolBroadcast,          // Packet to send to its adjacents nodes about updates in spanning tree
            protocolSpanningQuestion,   // Packet with question to neighbor about spanning tree
            protocolHeartBeatQuestion,   // Packet with question to neighbor about hearbeat
        };

        

        std::shared_ptr<std::queue<std::shared_ptr<struct MessageSpanningTree>>> queuePinkMessagesForSpanningTree;
        std::shared_ptr<std::queue<std::shared_ptr<struct MessageDijkstra>>> queuePinkMessagesForDijkstra;
        std::shared_ptr<std::queue<std::shared_ptr<struct MessageHeartBeat>>> queuePinkMessagesForHeartBeat;
        std::shared_ptr<std::queue<std::shared_ptr<struct MessageStopAndWait>>> queuePinkMessagesForStopAndWait;
        std::shared_ptr<std::queue<std::shared_ptr<struct MessageSandbox>>> queuePinkMessagesForSandbox;

        std::vector<struct NodeInfo> adyNodesInfo;
        struct NodeInfo greenNode;

        std::string pathPipePinkGreen;
        std::string pathPipeGreenPink;

        std::shared_ptr<int> pipePinkGreen;
        std::shared_ptr<int> pipeGreenPink;

        std::shared_ptr<LogBook> logBook;
        std::shared_ptr<Broadcast> broadcast;
        std::shared_ptr<Forwarding> forwarding;
        std::shared_ptr<StatisticsDataPlane> statisticsDataPlane;

        std::map<uint16_t, uint16_t> forwardingTable;
        std::vector<uint16_t> nodesInSpanningTree;

        std::shared_ptr<sem_t> semToWriteInPipe;

        sem_t semForDijkstra;
        sem_t semForStopAndWait;
        sem_t semForHeartBeat;
        sem_t semForSpanningTree;
        sem_t semForSandbox;

        /**
         * Function:    That function is in charge of sending the packets about stop and wait to the pink node
         * Parameters:  A shared integer indicating the activity of the green node.
         *              A shared integer with the file descriptor of the pipe to pink node to send messages.
         * Modify:      Does not affect any data structure, only get the items of the queue of Stop and Wait messages.
         * Return:      Does not return anything.
         */    
        
        void sendStopAndWait(std::shared_ptr<int> continueLoop, std::shared_ptr<int> parPipeGreenToPink);


        /**
         * Function:    That function is in charge of sending the packets about spanning tree to the pink node
         * Parameters:  A shared integer indicating the activity of the green node.
         *              A shared integer with the file descriptor of the pipe to pink node to send messages.
         * Modify:      Does not affect any data structure, only get the items of the queue of Spanning tree messages.
         * Return:      Does not return anything.
         */    
        void sendSpanningTree(std::shared_ptr<int> continueLoop, std::shared_ptr<int> pipeGreenToPink);

        /**
         * Function:    That function is in charge of sending the packets about spanning tree to the pink node
         * Parameters:  A shared integer indicating the activity of the green node.
         *              A shared integer with the file descriptor of the pipe to pink node to send messages.
         * Modify:      Does not affect any data structure, only get the items of the queue of Dijkstra messages.
         * Return:      Does not return anything.
         */ 
        void sendDijkstra(std::shared_ptr<int> continueLoop, std::shared_ptr<int> pipeGreenToPink);

        /**
         * Function:    That function is in charge of sending the packets about heart beat to the pink node
         * Parameters:  A shared integer indicating the activity of the green node.
         *              A shared integer with the file descriptor of the pipe to pink node to send messages.
         * Modify:      Does not affect any data structure, only get the items of the queue of Heart beat messages.
         * Return:      Does not return anything.
         */ 
        void sendHeartBeat(std::shared_ptr<int> continueLoop, std::shared_ptr<int> pipeGreenToPink);

        /**
         * Function:    That function is in charge of sending the packets about SandBox to the pink node
         * Parameters:  A shared integer indicating the activity of the green node.
         *              A shared integer with the file descriptor of the pipe to pink node to send messages.
         * Modify:      Does not affect any data structure, only get the items of the queue of SandBox messages.
         * Return:      Does not return anything.
         */    
        void sendSandbox(std::shared_ptr<int> continueLoop, std::shared_ptr<int> pipeGreenToPink); 

        /**
         * Function:    A function in charge of sending message to the pink node. To do that it create three threads, each one 
         *              for one of the functions explained above.
         * Parameters:  A shared integer indicating the activity of the green node.
         *              A shared integer with the file descriptor of the pipe to pink node to send messages.
         * Modify:      Does not affect any data structure, only get the items of the queue of Heart beat messages.
         * Return:      Does not return anything.
         */
        void sender(std::shared_ptr<int> pipeGreenToPink, std::shared_ptr<int> continueLoop);

        /**
         *  Function:   A function in charge of get, from the class statisticsDataPlane, the map that have the counter of packets
         *              received from each adjacent node and sent it to the pink node by the pipe using an internal
         *              protocol.
         *  Parameters: This function doesn't requiere any parameter.
         *  Modify:     This function only use the pipe to send pairs of node number and counter that indicate the amount of
         *              packet received from the node indicated in the pair.
         *  Return:     This function doesn't return anything.
         */
        void getStatisticsOfPacketsRecvFromNeighbor();

        /**
         *  Function:   A function in charge of get, from the class statisticsDataPlane, the map that have the counter of packets
         *              sent to each adjacent node and sent it to the pink node by the pipe using an internal protocol.
         *  Parameters: This function doesn't requiere any parameter.
         *  Modify:     This function only use the pipe to send pairs of node number and counter that indicate the amount of
         *              packet sent to the node indicated in the pair.
         *  Return:     This function doesn't return anything.
         */
        void getStatisticsOfPacketsSentToNeighbor();
        
        /**
         *  Function:   A function in charge of get, from the class statisticsDataPlane, the pair that have the counter of forwarding
         *              and broadcast packets.
         *  Parameters: This function doesn't requiere any parameter.
         *  Modify:     This function only use the pipe to send pairs that indicate the counter of forwarding and broadcast packets, in
         *              that order.
         *  Return:     This function doesn't return anything.
         */
        void getStatisticsOfPacketsForwardingAndBroadcast();


        /**
         * Function:    A function in charge of receiving messages from the pink node. To do that it manage the protocols of 
         *              information received from its pink node, and give that packet to the corresponding functionality to process
         *              them.
         * Parameters:  A shared integer with the file descriptor of the pipe to receive messages from the pink node.
         *              A shared integer indicating the activity of the green node.
         *              A shared queue of message to be send to the link layer and process them correctly.
         *              A shared mutex to push message to the queue.
         * Modify:      Does not modify anything, only read information from the pipe and send to the proper functionality to process 
         *              them.
         * Return:      Does not return anythong.
         **/ 
        void receiver(std::shared_ptr<int> pipePinkToGreen, std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue< std::shared_ptr<struct MessageGreenGreen>>> queueControlMessages, std::shared_ptr<std::mutex> mutexToQueue,  std::shared_ptr<sem_t> semaphoreDispatcher);

        /**
         * Function:    A function in charge of send to the pink node the information of the green node, it mean, to send 
         *              the information of myself and my neighbors.
         * Parameter:   A shared integer with the file descriptor of the pipe to send message to the pink node.
         * Modify:      Does nodt modify anything, only send information, in the pipe, to the pink node.
         * Return:      Does not return anything.
         */
        void sendAdyacentInfo(std::shared_ptr<int> pipeGreenPink);

    public:
        PinkLayer();
        PinkLayer(std::shared_ptr<LogBook> logBook, std::shared_ptr<Broadcast> broadcast, std::shared_ptr<Forwarding> forwarding, std::shared_ptr<StatisticsDataPlane> statisticsDataPlane);
        ~PinkLayer();
        
        /**
         * Function:    Is the main function. It is in charge of the initialization of the pipes to communicate with the pink node,
         *              create a fork to initialize the pink node, and two thread, one for receive messages from the pink node
         *              and other to send message to the pink node.
         * Parameters:  A shared integer indicating the activity of the green node.
         *              A shared queue of message to be send to the link layer and process them correctly.
         *              A shared mutex to push message to the queue.
         *              A vector with the information of my neighbors.
         *              A struct with the information of myself (the green node).
         * Modify:      Does not modify anything, only initialize the pink node, and initialize the threads in charge of 
         *              the communication with the pink node.
         * Return:      Does not return anything.
         */
        int execute(std::shared_ptr<int> continueLoop, std::shared_ptr<std::queue< std::shared_ptr<struct MessageGreenGreen>>> queueControlMessages, std::shared_ptr<std::mutex> mutexToQueue, std::vector<struct NodeInfo> adyNodesInfo, struct NodeInfo greenNode,  std::shared_ptr<sem_t> semaphoreDispatcher);
        
        /**
         * Function:    A public function in charge of adding message to the private queue of packets that are going to be send to
         *              the pink node. The messages are about spanning tree question or answers.
         * Parameter:   A struct with the message to add in the private queue.
         * Modify:      A private queue with the message of spanning tree to be send to the pink node.
         * Return:      Does not return anything.
         */
        void pushMessagePinkForSpanningTree(struct MessageSpanningTree msg);

        /**
         * Function:    A public function in charge of adding message to the private queue of packets that are going to be send to
         *              the pink node. The messages are Dijkstra, that means, updates in the spanning tree.
         * Parameter:   A struct with the message to add in the private queue.
         * Modify:      A private queue with the message of Dijkstra to be send to the pink node.
         * Return:      Does not return anything.
         */
        void pushMessagePinkForDijkstra(struct MessageDijkstra msg);

        /**
         * Function:    A public function in charge of adding message to the private queue of packets that are going to be send to
         *              the pink node. The messages are about heart beat protocol.
         * Parameter:   A struct with the message to add in the private queue.
         * Modify:      A private queue with the message of heart beat to be send to the pink node.
         * Return:      Does not return anything.
         */
        void pushMessagePinkForHeartBeat(struct MessageHeartBeat msg);

        /**
         * Function:    A public function in charge of adding message to the private queue of packets that are going to be send to
         *              the pink node. The messages are about a request of a spider.
         * Parameter:   A struct with the message to add in the private queue.
         * Modify:      A private queue with the message of sandbox to be send to the pink node.
         * Return:      Does not return anything.
         */
        void pushMessageForSandbox(struct MessageSandbox msg); 

        /**
         * Function:    A public function in charge of adding message to the private queue of packets that are going to be send to
         *              the pink node. The messages are about stop and wait protocol.
         * Parameter:   A struct with the message to add in the private queue.
         * Modify:      A private queue with the message of stop and wait to be send to the pink node.
         * Return:      Does not return anything.
         */
        void pushMessagePinkForStopAndWait(struct MessageStopAndWait msg);
        
};
#endif