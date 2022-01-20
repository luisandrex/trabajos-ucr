from ctypes import *
from enum import Enum

class protocolPinkToGreen(Enum):
        protocolForwarding = 0              # the message is a list of pairs
        protocolBroadcastTable = 1          # the message is a list of integers
        protocolBroadcast = 2               # the message is an structure MessageBroadcast
        protocolSpanningQuestion = 3        # the message is an structure SpanningTreeQuestion
        protocolHeartBeatQuestion = 4       # the message is an structure HeartBeatQuestion
        protocolStopAndWait = 5
        protocolPacketsFromSrcToSrc = 6
        protocolPacketsSentToNeighbor = 7
        protocolPacketsRecvFromNeighbor = 8
        protocolPacketsForwardingBroadcast = 9
        protocolSandboxToBlue = 10

class protocolGreenToPink(Enum):
        protocolGreenInfo = 0
        protocolAdyacentsInfo = 1
        protocolSpanningTree = 2
        protocolDjikstra = 3
        protocolHeartBeat = 4
        protocolStopAndWait = 5
        protocolPacketsFromSrcToSrc = 6
        protocolPacketsSentToNeighbor = 7
        protocolPacketsRecvFromNeighbor = 8
        protocolPacketsForwardingBroadcast = 9
        protocolSandboxFromBlue = 10

class NodeInfo(Structure):
    _fields_ = [("nodeNum", c_uint16), 
                ("portUDP", c_uint), 
                ("ip", c_char*16)]

class MessageForwarding(Structure):
    _fields_ = [("dest", c_uint16),
                ("inter", c_uint16)]

class MessageBroadcastTable(Structure):
    _fields_ = [("nodeInSpanning", c_uint16)]

class AdjacentListInfo(Structure):
    _pack_ = 1
    _fields_ = [("nodeID", c_uint16),
                ("distance", c_uint8)]

class MessageDijkstra(Structure):
    _pack_ = 1
    _fields_ = [("nodeSrc", c_uint16), 
                ("numAdyNodes", c_uint8),
                ("pairAdyDistance", AdjacentListInfo*256)]

class MessageBroadcast(Structure):
    _pack_ = 1
    _fields_ = [("nodeSrc", c_uint16),
                ("TTL", c_uint8),
                ("msgDijkstra", MessageDijkstra)]

class SpanningTreeQuestion(Structure):
    _pack_ = 1
    _fields_ = [("question", c_uint8),
                ("nodeSrc", c_uint16),
                ("nodeDest", c_uint16),
                ("SN", c_uint16), 
                ("RN", c_uint16)]

class HeartBeatQuestion(Structure):
    _pack_ = 1
    _fields_ = [("nodeSrc", c_uint16),
                ("nodeDest", c_uint16),
                ("code", c_uint8),
                ("secNum", c_uint16),
                ("resNum", c_uint16)]

class MessageStopAndWait(Structure):
    _pack_ = 1
    _fields_ = [("nodeSrc",c_uint16),
                ("nodeDest",c_uint16),
                ("ID_1",c_uint8),
                ("ID_2",c_uint8),
                ("SN",c_uint8),
                ("RN",c_uint8),
                ("FLAGS",c_uint8),
                ("identifier", c_uint8),
                ("payload_size",c_uint16),
                ("payload",c_char*4096)]

class StatisticsRequest(Structure):
    _fields_ = [("spiderFrom", c_uint16),
                ("request", c_uint8)]

class PairOfStatisticsElements(Structure):
    _fields_ = [("node", c_uint16),
                ("counter", c_uint16)]

class TripletOfStatisticsElements(Structure):
    _fields_ = [("node1", c_uint16),
                ("node2", c_uint16),
                ("counter", c_uint16)]


class ContainerFromSandbox(Structure):
    _fields_ = [("typeMSG", c_int8),
                ("PacketToSend", c_char*2048),
                ("unit", c_uint8),
                ("dest", c_uint8)]

class MessageToSandbox(Structure):
    _pack_ = 1
    _fields_ = [("luggagePath", c_char*200)]

class SpiderData(Structure):
    _fields_ = [("executable_name", c_char*16),
                ("spider_num", c_uint8),
                ("origin_node", c_uint16),
                ("list_of_nodes", c_char*256),
                ("luggage_name", c_char*32  ),
                ("mailbox_name", c_char*16),
                ("PID", c_uint32),
                ("internal_spider_num", c_uint16)]

class MessageSandbox(Structure):
    _fields_ = [("protocol", c_uint8),
                ("type", c_uint8),
                ("origin_node", c_uint8),
                ("spider_ID", c_uint8),
                ("payload_size", c_uint16),
                ("payload",c_char*2048)]
