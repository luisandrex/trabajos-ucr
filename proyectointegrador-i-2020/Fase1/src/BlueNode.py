import sys
import socket
import os
import datetime
import time

from LogBook import LogBook 
from ctypes import *

#Structure of messages between green nodes and blue nodes:
# First two bytes correspond to the green node that will be receiving the message
# Two more bytes with the message number used exclusively by the blue node  
# The last 200 bytes are left for the message itself
class messageBetweenGreenBlue(Structure):
    _fields_ = [("dest", c_uint16), 
                ("src", c_uint16),
                ("msgNum", c_uint16), 
                ("msg", c_char*200)]

#Function: waits to receive a message from another green node to the green node associated with this blue node and displays it on screen
#Modifies: this method does not modify anything
#Requirements: sockTCP, which is the socket, and f, which is the log to write the events to 
def receiveMSG( sockTCP, logBookBlue, greenNum):  #dest es el emisor en este metodo (Nodo verde que envia mensaje a este)
    cont = True
    MAX_INT16 = 65535
    while cont:
        try:
            msgRecv = sockTCP.recv(sizeof(messageBetweenGreenBlue))
            message = messageBetweenGreenBlue.from_buffer_copy(msgRecv)

            if(message.dest == MAX_INT16):
                #recibe arana
                event = f"Green node has received {message.msg} from spider and will be displayed"
                logBookBlue.writeToLogEvent(event)

                print(f"Spider data can be found at {message.msg}")

                luggage = open(message.msg, "r")

                print("\nSpider Data: \n")

                machStats = luggage.readline()
                machStats = machStats.split(",")

                netStats = luggage.readline()
                netStats = netStats.split(",")

                IP = luggage.readline()

                while(IP):

                    print(f"Node number: {IP}")

                    print("\n Machine Statistics: \n")
                    for mStat in range (len(machStats)-1):
                        stat = machStats[mStat]
                        statContent = luggage.readline()

                        if(stat == "1"):
                            print(f"Total memory (MB): {statContent}")
                        elif (stat == "2"):
                            print(f"Free memory (percentage with 2 decimal digits): {statContent}")
                        elif (stat == "3"):
                            print(f"Total space in File System (GB): {statContent}")
                        elif (stat == "4"):
                            print(f"Free space in File System (percentage with 2 decimal digits): {statContent}")
                    

                    print("\n Network Statistics: \n")
                    for nStat in range (len(netStats)-1):
                        stat = netStats[nStat]
                        statContent = luggage.readline()
                        if(stat == "1"):
                            print(f"Forwarding Tables (green): {statContent}")
                        elif (stat == "2"):
                            print(f"List of neighbor nodes in AG (green): {statContent}")
                        elif (stat == "3"):
                            print(f"Graph map (pink): {statContent}")
                        elif (stat == "4"):
                            print(f"Number of seen packages by each source (either from transit or as final destiny): {statContent}")
                        elif (stat == "5"):
                            print(f"Number of sent packages through each neighbor: {statContent}")
                        elif (stat == "6"):
                            print(f"Number of received packages from each neighbor (either from transit or as final destiny): {statContent}")
                        elif (stat == "7"):
                            print(f"Number of broadcast and routing packages received: {statContent}")
                
                    IP = luggage.readline()

                luggage.close()


            else:
                # EVENT(message received successfully by green node) written to log
                event = f"Green node has received message #{message.msgNum} from green node #{message.dest}. \nMessage({message.msg})"
                logBookBlue.writeToLogEvent(event)
                # Message is displayed on screen            
                print(f"\nGreen node has received message #{message.msgNum} from green node #{message.dest}. \nMessage({message.msg}).\n")

        except ValueError:

            # EVENT(user stopped program) written to log
            event = "Stop receiving messages from user"
            logBookBlue.writeToLogWarning(event)
            cont = False            

    return 

#Function: asks the user for messages to send to other green nodes 
#Modifies: this method does not modify anything
#Requirements: sockTCP, which is the socket, and f, which is the log to write the events to 
def askForMSG( sockTCP, logBookBlue, greenNum):
    MAX_INT16 = 65535
    msgNum = 0
    spiderNum = 0
    cont = True
    while cont:
        msg = input("Type the message to send: ")
        # If the message is null, the blue node gets deleted
        if not len(msg):
            dest = 0
            src = 0
            event = "The blue node is going to be deleted"
            logBookBlue.writeToLogEvent(event)

            # Message sent through TCP to the green node to inform that communication between green and blue node is going to be stopped
            Message_sent = messageBetweenGreenBlue(c_uint16(dest), c_uint16(src), c_uint16(msgNum), msg.encode("utf-8"))
            n = sockTCP.send(Message_sent)

            cont = False
        elif msg == "#":
            #se crea una arana
            event = "A spider will be created"
            logBookBlue.writeToLogEvent(event)

            
            askForStats = True
            while (askForStats == True):
                print("\nMachine statistics: \n 1. Total memory (MB)\n 2. Free memory (percentage with 2 decimal digits)")
                print(" 3. Total space in File System (GB)\n 4. Free space in File System (percentage with 2 decimal digits)\n")
                machStatistcs = input("Please enter all the machine stadistics you would like the spider to recollect separated by comas\n ")
                list_machStadistics = machStatistcs.split(",")
                list_machStadistics = list(dict.fromkeys(list_machStadistics))
                askForStats = False
                counter = 0
                while(askForStats == False and counter < len(list_machStadistics)):
                    mStat = list_machStadistics[counter]
                    if( mStat.isdigit() == False):
                        print("Invalid value entered, please reenter the list of machine statistics")
                        askForStats = True
                    else:
                        mStat = int(mStat)
                        if( mStat <= 0 or mStat > 4 ):
                            print("Invalid value entered, please reenter the list of machine statistics")
                            askForStats = True
                    counter += 1


            event = "Machine statistics have been collected"
            logBookBlue.writeToLogEvent(event)

            askForStats = True
            while (askForStats == True):
                print("\nNetwork statistics:\n 1. Forwarding Tables (green)\n 2. List of neighbor nodes in AG (green)\n 3. Graph map (pink)")
                print(" 4. Number of seen packages by each source (either from transit or as final destiny) \n 5. Number of sent packages through each neighbor")
                print(" 6. Number of received packages from each neighbor (either from transit or as final destiny).")
                print(" 7. Number of broadcast and routing packages received. \n")
                netStatistcs = input("Please enter all the network stadistics you would like the spider to recollect separated by comas\n ")
                list_netStadistics = netStatistcs.split(",")
                list_netStadistics = list(dict.fromkeys(list_netStadistics))
                askForStats = False
                counter = 0
                while(askForStats == False and counter < len(list_netStadistics)):
                    nStat = list_netStadistics[counter]
                    if( nStat.isdigit() == False):
                        print("Invalid value entered, please reenter the list of machine statistics")
                        askForStats = True
                    else:
                        nStat = int(nStat)
                        if( nStat <= 0 or nStat > 7 ):
                            print("Invalid value entered, please reenter the list of machine statistics")
                            askForStats = True
                    counter += 1

            event = "Network statistics have been collected"
            logBookBlue.writeToLogEvent(event)

            luggage = open(f"luggage{greenNum}_{spiderNum}.txt", "w+")
            path = f"luggage{greenNum}_{spiderNum}.txt"
            for i in list_machStadistics:
                luggage.write(f"{i},")
                if(i == list_machStadistics[-1]):
                    luggage.write("-\n")

            for j in list_netStadistics:
                luggage.write(f"{j},")
                if(j == list_netStadistics[-1]):
                    luggage.write("-\n")
            
            luggage.close()

            event = "Luggage has been created and is going to be sent"
            logBookBlue.writeToLogEvent(event)

            Message_sent = messageBetweenGreenBlue(c_uint16(greenNum),c_uint16(MAX_INT16), c_uint16(MAX_INT16), path.encode("utf-8"))

            n = sockTCP.send(Message_sent)

            spiderNum += 1

            # EVENT(message sent successfully by blue node) written to log                
            event = f"Spider message sent by blue node to green node"
            logBookBlue.writeToLogEvent(event)


        else :
            dest = input("Type the number of the green node to send the message: ")
            if dest.isdigit():

                # EVENT(message received successfully by blue node) written to log
                event = f"Message #{msgNum} received by blue node.\nMessage({msg})."
                logBookBlue.writeToLogEvent(event)

                # Message sent through TCP to the green node
                Message_sent = messageBetweenGreenBlue(c_uint16(int(dest)), c_uint16(greenNum), c_uint16(msgNum), msg.encode("utf-8"))
                n = sockTCP.send(Message_sent)

                # EVENT(message sent successfully by blue node) written to log                
                event = f"Message #{msgNum} sent by blue node to green node #{dest}"
                logBookBlue.writeToLogEvent(event)

                # Increasing the number of messages sent by this blue node
                msgNum += 1
            elif (dest == '*'):

                # EVENT(message received successfully by blue node) written to log
                event = f"Message (broadcast) #{msgNum} received by blue node.\nMessage({msg})."
                logBookBlue.writeToLogEvent(event)

                # Message sent through TCP to the green node
                Message_sent = messageBetweenGreenBlue(c_uint16(MAX_INT16), c_uint16(greenNum), c_uint16(msgNum), msg.encode("utf-8"))
                n = sockTCP.send(Message_sent)

                # EVENT(message sent successfully by blue node) written to log                
                event = f"Message (broadcast) #{msgNum} sent by blue node to green node #{dest}"
                logBookBlue.writeToLogEvent(event)
                msgNum += 1

            else: 
                # ERROR(invalid destination) written to log
                event = f"The number of adyacent node is not valid. You typed {dest}"
                logBookBlue.writeToLogWarning(event)
                

    return


def main() :
    #Verification of the arguments 
    if len(sys.argv) < 3 :
        print("Use python3 BlueNode.py <IP of the TCP server> <number of green node>.")
        exit(1)

    ipServer = sys.argv[1]
    numGreenNode = int(sys.argv[2])
    portServer = 5000 + numGreenNode
 
    path_logBook = f"../etc/LogBookBlue{numGreenNode}.txt"
 
    logBookBlue = LogBook(path_logBook)

    # EVENT(initialization of blue node) written to log

    event = f"Blue node successfully initialized with number #{numGreenNode}"
    logBookBlue.writeToLogEvent(event)
    
    try:
        sockTCP = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        
        # EVENT(successfully socketing blue node) written to log

        event = "Blue node successfully socketed"
        logBookBlue.writeToLogEvent(event)

    except: 

        # ERROR(unsuccessfully socketing blue node) written to log

        event = "Error in socket"
        logBookBlue.writeToLogError(event)

    connection = False
    while not connection:
        try:
            sockTCP.connect((ipServer, portServer))

            # EVENT (successfully connecting to server) written to log

            event = "Connection to server successful"
            logBookBlue.writeToLogEvent(event)

            connection = True
        except: 
            # ERROR (unsuccessfully connecting to server) written to log

            event = "Error connecting to server"
            logBookBlue.writeToLogError(event)
    
            time.sleep(1.5)

    fork_id = os.fork()
    if fork_id == 0 :
        # Child process is going to send the messages typed in by the user 
        askForMSG(sockTCP,logBookBlue, numGreenNode)

        sys.exit(0)
    elif fork_id > 0 :
        # Parent process is going to constantly receive messages
        receiveMSG(sockTCP,logBookBlue, numGreenNode)

        os.waitpid(fork_id, 0)
        sockTCP.close()
    else :
        # ERROR (unsuccessful fork) written to log
        event = "Error in fork"
        logBookBlue.writeToLogError(event)
        exit(4)

    #    f.close()

    return

main()