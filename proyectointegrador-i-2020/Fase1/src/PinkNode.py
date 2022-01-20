import sys, threading, os

from LogBook import LogBook 
from AgentPinkSender import AgentPinkSender
from AgentPinkReceiver import AgentPinkReceiver
from HeartBeat import HeartBeat
from SpanningTree import SpanningTree
from Routing import Routing
from StopAndWait import StopAndWait
from StatisticsControlPlane import StatisticsControlPlane
from Sandbox import Sandbox
from Multiplexor import Multiplexor
class PinkNode:
    """
    Class in charge of activate all the functionalities in the pink node.

    Author:     Jose Andres Viquez
    """
    SENDER = 1
    RECEIVER = 2

    def main(self):
        """
        Function in charge of create an instance of each functionality of the pink node, pass them parameters
        and create one thread for each of them to be processing information parallel.

        Parameters
        ----------
        Does not requires parameters

        Modify
        ------
        Does not modify anything

        Return
        ------
        Does not return anything
        """ 
        # argv1 is the pipe between green and pink
        pipe_green_pink = str(sys.argv[1])

        # argv2 is the pipe between pink and green
        pipe_pink_green = str(sys.argv[2])

        # argv3 is the path of the logbook
        path_logBook = str(sys.argv[3])

        logBookPink = LogBook(path_logBook)
        
        agentPinkSender = AgentPinkSender(logBookPink)

        statisticsControlPlane = StatisticsControlPlane(logBookPink, agentPinkSender)        

        routing = Routing(agentPinkSender, logBookPink, statisticsControlPlane)

        spanningTree = SpanningTree(routing, agentPinkSender, logBookPink, statisticsControlPlane)

        hearBeat = HeartBeat(logBookPink, agentPinkSender, spanningTree)

        multiplexor = Multiplexor(agentPinkSender, logBookPink)

        sandbox = Sandbox(logBookPink, statisticsControlPlane, agentPinkSender, multiplexor)
        
        agentPinkReceiver = AgentPinkReceiver(spanningTree, hearBeat, routing, logBookPink, statisticsControlPlane, multiplexor, sandbox)

        continueLoop = True
        logBookPink.writeToLogEvent("PinkNode:\t\tPink node initialized")
        
        threadAgentReceiver = threading.Thread(target=agentPinkReceiver.execute,args=(continueLoop, pipe_green_pink,))           
        threadSpanningTree = threading.Thread(target=spanningTree.execute,args=(continueLoop,))
        threadRouting = threading.Thread(target=routing.execute,args=(continueLoop,))
        threadAgentSender = threading.Thread(target=agentPinkSender.execute,args=(continueLoop, pipe_pink_green,))
        threadHeartBeat = threading.Thread(target=hearBeat.execute,args=(continueLoop,))
        threadMultiplexor = threading.Thread(target=multiplexor.execute, args=(continueLoop,))
        threadSandbox = threading.Thread(target=sandbox.execute, args=(continueLoop,))
        threadStatisticsControlPlane = threading.Thread(target=statisticsControlPlane.execute, args=(continueLoop,))

        threadAgentReceiver.start()
        threadSpanningTree.start()
        threadHeartBeat.start()
        threadRouting.start()
        threadAgentSender.start()
        threadStatisticsControlPlane.start()
        threadMultiplexor.start()
        threadSandbox.start()

        threadSandbox.join()
        threadAgentReceiver.join()
        continueLoop = False
        threadHeartBeat.join()
        threadRouting.join()
        threadSpanningTree.join()
        threadMultiplexor.join()

        agentPinkSender.pushMessage(0,255)
        threadStatisticsControlPlane.join()
        threadAgentSender.join()

if __name__ == "__main__":
    pinkNode = PinkNode()
    pinkNode.main()
