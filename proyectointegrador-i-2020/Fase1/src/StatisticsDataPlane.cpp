#include "../headers/StatisticsDataPlane.h"

StatisticsDataPlane::StatisticsDataPlane(std::shared_ptr<LogBook> parLogBook) {
    logBook = parLogBook;
}

void StatisticsDataPlane::addPacketSendTo(uint16_t nodeDest) {
    std::lock_guard<std::mutex> guard(mutexToData);
    logBook->writeToLogEvent("StatisticsDataPlane:\tThe counter of packets sent to one node increase");
    auto itr = counterPacketsSendTo.find(nodeDest);
    if(itr == counterPacketsSendTo.end()) {
        counterPacketsSendTo.emplace(nodeDest, 1);
    } else {
        itr->second++;
    }
}

void StatisticsDataPlane::addPacketRecvFrom(uint16_t nodeSrc) {
    std::lock_guard<std::mutex> guard(mutexToData);
    logBook->writeToLogEvent("StatisticsDataPlane:\tThe counter of packets received from one node increase");
    auto itr = counterPacketsRecvFrom.find(nodeSrc);
    if(itr == counterPacketsRecvFrom.end()) {
        counterPacketsRecvFrom.emplace(nodeSrc, 1);
    } else {
        itr->second++;
    }
}

void StatisticsDataPlane::addPacketForwarding() {
    std::lock_guard<std::mutex> guard(mutexToData);
    logBook->writeToLogEvent("StatisticsDataPlane:\tThe counter of forwarding packets increase");
    ++counterPacketsForwardingAndBroadcast.first;
}

void StatisticsDataPlane::addPacketBroadcast() {
    std::lock_guard<std::mutex> guard(mutexToData);
    logBook->writeToLogEvent("StatisticsDataPlane:\tThe counter of broadcast packets increase");
    ++counterPacketsForwardingAndBroadcast.second;
}

std::map<uint16_t, uint16_t> StatisticsDataPlane::getCounterPacketsSendTo() {
    std::lock_guard<std::mutex> guard(mutexToData);
    logBook->writeToLogEvent("StatisticsDataPlane:\tThe counter of packets send to each of the nodes have been gotten by the PinkLayer");
    return counterPacketsSendTo;   
}
std::map<uint16_t, uint16_t> StatisticsDataPlane::getCounterPacketsRecvFrom() {
    std::lock_guard<std::mutex> guard(mutexToData);
    logBook->writeToLogEvent("StatisticsDataPlane:\tThe counter of packets receive from each of the nodes have been gotten by the PinkLayer");
    return counterPacketsRecvFrom;
}
std::pair<uint16_t, uint16_t> StatisticsDataPlane::getCounterPacketsBroadcastAndForwarding() {
    std::lock_guard<std::mutex> guard(mutexToData);
    logBook->writeToLogEvent("StatisticsDataPlane:\tThe counter of forwarding and broadcast packets have been gotten by the PinkLayer");
    return counterPacketsForwardingAndBroadcast;
}