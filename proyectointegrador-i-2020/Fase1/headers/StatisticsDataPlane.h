#ifndef STATISTICS
#define STATISTICS

#include "LogBook.h"
#include <memory>
#include <map>

/**
 *  Definition: Class in charge of recollect the statistics about the data plane of the network.
 *  Author: Jose Andres Viquez 
 */
class StatisticsDataPlane{
    private: 

        std::shared_ptr<LogBook> logBook;
        std::mutex mutexToData;
        // std::map<std::pair<uint16_t, uint16_t>, uint16_t> counterPacketsFromSrcToDest;
        std::map<uint16_t, uint16_t> counterPacketsSendTo;
        std::map<uint16_t, uint16_t> counterPacketsRecvFrom;
        std::pair<uint16_t, uint16_t> counterPacketsForwardingAndBroadcast;

    public:
        StatisticsDataPlane(std::shared_ptr<LogBook> parLogBook);

        /**
         * Function:    That function is in charge of add a counter indicating the amout of packets send to each of
         *              the adjacent nodes.
         * Parameters:  The adjacent node whom's counter of packets sent to it would increase.
         * Modify:      A map that store each of the counters for each node, and get a mutex used to doesn't
         *              read while writing.
         * Return:      Does not return anything.
         */  
        void addPacketSendTo(uint16_t nodeDest);
        
        /**
         * Function:    That function is in charge of add a counter indicating the amout of packets received from
         *              one of the adjacent nodes.
         * Parameters:  The adjacent node whom's counter of packets received from it would increase.
         * Modify:      A map that store each of the counters for each node, and get a mutex used to doesn't
         *              read while writing.
         * Return:      Does not return anything.
         */  
        void addPacketRecvFrom(uint16_t nodeSrc);

        /**
         * Function:    That function is in charge of add a counter indicating the amout of forwarding packets received and sent.
         * Parameters:  Doesn't require any parameters.
         * Modify:      A counter that indicate the amount of forwarding packets received and sent, and get a mutex used to doesn't
         *              read while writing.
         * Return:      Does not return anything.
         */ 
        void addPacketForwarding();

        /**
         * Function:    That function is in charge of add a counter indicating the amout of broadcast packets received and sent.
         * Parameters:  Doesn't require any parameters.
         * Modify:      A counter that indicate the amount of broadcast packets received and sent, and get a mutex used to doesn't
         *              read while writing.
         * Return:      Does not return anything.
         */ 
        void addPacketBroadcast();

        /**
         * Function:    That function is in charge of return a map in whom's key is the node num and the value is the counter 
         *              of packets sent to that node indicated in the key.
         * Parameters:  Doesn't require any parameters.
         * Modify:      A mutex used to read while there's anyone writing.
         * Return:      A map with the counter of packets sent to each destination node.
         */ 
        std::map<uint16_t, uint16_t> getCounterPacketsSendTo();

        /**
         * Function:    That function is in charge of return a map in whom's key is the node num and the value is the counter 
         *              of packets received from the node indicated in the key.
         * Parameters:  Doesn't require any parameters.
         * Modify:      A mutex used to read while there's anyone writing.
         * Return:      A map with the counter of packets received from each source node.
         */
        std::map<uint16_t, uint16_t> getCounterPacketsRecvFrom();

        /**
         * Function:    That function is in charge of return a pair with the counter of forwarding and broadcast packets.
         * Parameters:  Doesn't require any parameters.
         * Modify:      A mutex used to read while there's anyone writing.
         * Return:      A pair with the counter of forwarding and broadcast packets.
         */
        std::pair<uint16_t, uint16_t> getCounterPacketsBroadcastAndForwarding();

};
#endif