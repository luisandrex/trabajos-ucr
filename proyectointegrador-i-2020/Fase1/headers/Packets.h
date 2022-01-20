#ifndef PACKETS
#define PACKETS
#include <iostream>
#include <netinet/in.h>
#include <arpa/inet.h>

struct MessageGreenPink {
    uint16_t dest;
    uint16_t nodeInter;
};

struct NodeInfo {
    uint16_t nodeNum;
    uint portUDP;
    char ip[16];
};

struct SockUDPInfo {
    uint16_t nodeNum;
    int sock; 
    struct sockaddr_in server;
};

struct MessageGreenBlue {
    uint16_t dest;
    uint16_t src; 
    uint16_t msgNum;
    char msg[200];
};

#pragma pack(push, 2)
struct MessageForwarding {
    uint16_t src;
    uint16_t dest;
    uint16_t msgNum;
    uint16_t RN;
    char msg[200];
};
#pragma pack(pop)

#pragma pack(push, 1)
struct AdjacentListInfo{
    uint16_t nodeID;
    uint8_t distance;
};
#pragma pack(pop)

#pragma pack(push, 1)
struct MessageDijkstra {
    uint16_t nodeSrc;
    uint8_t numAdyNodes;
    AdjacentListInfo pairAdyDistance[256];
};
#pragma pack(pop)

#pragma pack(push, 1)
struct MessageBroadcastSP {
    struct MessageDijkstra msgDijkstra;
};
#pragma pack(pop)

#pragma pack(push, 2)
struct MessageBroadcastData{
    uint16_t src;
    uint16_t dest;
    uint16_t msgNum; 
    uint16_t RN; 
    char msg[200]; 
}; 
#pragma pack(pop)

#pragma pack(push, 1)
struct BroadcastContainer {
    //uint8_t protocol; 
    uint16_t nodeSrc; 
    uint8_t TTL; 
    uint8_t protocol;
    MessageBroadcastData DataBroadcast; 
    MessageBroadcastSP SpanningBroadcast; 
}; 
#pragma pack(pop)

#pragma pack(push, 1)
struct BroadcastFromPink {
    uint16_t nodeSrc;
    uint8_t TTL;
    //uint8_t protocol;
    struct MessageDijkstra msgDijkstra;
};
#pragma pack(pop)

#pragma pack(push, 1)
struct MessageSpanningTree {
    uint8_t question;
    uint16_t src;
    uint16_t dest;
    uint16_t SN;
    uint16_t RN;
};
#pragma pack(pop)

#pragma pack(push, 1)
struct MessageHeartBeat {
    uint16_t nodeSrc;
    uint16_t nodeDest;
    uint8_t code;
    uint16_t SN;
    uint16_t RN;
};
#pragma pack(pop)

#pragma pack(push, 1)
struct MessageStopAndWait{// protocolo entre stop&wait
    uint16_t src;
    uint16_t dest;
    u_int8_t ID_1;
    u_int8_t ID_2;
    u_int8_t SN;
    u_int8_t RN;
    u_int8_t FLAGS;
    u_int8_t identifier;
    u_int16_t payload_size;
    char payload[4096];
};
#pragma pack(pop)

#pragma pack(push,1)
struct MessageSandbox{
    char spiderLuggage[200]; 
}; 
#pragma pack(pop)

struct UDPPacket{
    uint8_t protocol;
    char payload[sizeof(MessageStopAndWait)];
};

struct MessageGreenGreen {
    uint8_t protocol;
    struct MessageForwarding msgForwarding;
    struct BroadcastContainer msgBroadcast;
    struct MessageSpanningTree msgSpanning;
    struct MessageDijkstra msgDijkstra;
    struct MessageHeartBeat msgHeartBeat;
    struct MessageStopAndWait msgStopandWait;
    struct MessageSandbox msgSandbox;
    uint16_t nodeInter;
};

struct ForwardingContainer{
    struct MessageForwarding msgForwarding;
    struct MessageStopAndWait msgStopAndWait; //le agrego el stop&wait ya que este es un tipo de mensaje a enviar por forwarding. 
    int typeOfForwarding;
};

struct PairOfStatisticsElements{
    uint16_t node;
    uint16_t counter;
};

struct TripletOfStatisticsElements{
    uint16_t node1;
    uint16_t node2;
    uint16_t counter;
};

#endif
