import csv
import os
import sys
import datetime
import time

from LogBook import LogBook 
from ctypes import *

class messageBetweenGreenOrange(Structure):
    _fields_ = [("nodeNum", c_int16), 
                ("portUDP", c_int), 
                ("ip", c_char*16)]


# Function: reads a CSV file, which contains the adjacent green nodes, and sends it through a pipe to the green node 
# Modifies: this method does not modify anything
# Requirements: csv_file, which is the CSV file that contains all of the information, fd_pipe_OtoG, which is the pipe used to communicate green and orange nodes with one another
# Requirements: csv_reader, which is used to read in CSV's format and log, which is the log of this orange node 
def readCSV( csv_file, fd_pipe_OtoG, csv_reader, logBookOrange, logBookName):
    greenNum = -1
    for row in csv_reader:
        # read data from CSV file 
        greenNum = int(row[0])
        ip = str(row[1])
        portUDP = int(row[2])

        n = os.write(fd_pipe_OtoG, messageBetweenGreenOrange(c_int16(greenNum), c_int(portUDP), ip.encode("utf-8")))

        # EVENT(CSV file successfully read and sent to green node) written to log
        logBookOrange.writeToLogEvent("Data from file read and sent to Green node")
        
        if n < 0: 
            # ERROR(CSV file unsuccessfully read and sent to green node) written to log
            logBookOrange.writeToLogError("Error writing")
            os.rename(logBookName, f"../etc/LogBookOrange{greenNum}.txt")
            exit(1)
    return greenNum#que verguenza compa

# Function: reads a file, which contains the green node's ID, and sends it through a pipe to the green node  
# Modifies: this method does not modify anything
# Requirements: fd_pipe, which is the pipe used to communicate green and orange nodes with one another, path_file_id, which contains the path of the ID file 
# Requirements: and log, which is the log of this orange node
def readID( fd_pipe, path_file_id, logBookOrange, logBookName) :
    try:
        csv_id_file =  open(path_file_id)
        
        # EVENT(ID file successfully opened) written to log
        logBookOrange.writeToLogEvent(f"Opening file {path_file_id}")
        
        csv_reader = csv.reader(csv_id_file, delimiter=',')
        
        # EVENT(ID file is being read) written to log
        logBookOrange.writeToLogEvent(f"Reading file {path_file_id}")
        
        greenNum = readCSV(csv_id_file, fd_pipe, csv_reader, logBookOrange, logBookName)
        return greenNum

    except FileNotFoundError:
        # In case the ID file does not exist, the pipe between nodes is closed
        ip = ""
        os.write(fd_pipe, messageBetweenGreenOrange(-1, 0, ip.encode("utf-8")))
        os.close(fd_pipe) 
        
        # ERROR(ID file successfully read and sent to green node) written to log
        logBookOrange.writeToLogError(f"Error opening identifier file of green node. The file was {path_file_id}")
        
        exit(1)

# Function: reads a file, which contains the green node's adjacent nodes, and sends it through a pipe to the green node  
# Modifies: this method does not modify anything
# Requirements: fd_pipeG, which is the pipe used to communicate green and orange nodes with one another, path_file_ady, which contains the path of the CSV file 
# Requirements: and log, which is the log of this orange node
def readAdyNodes( fd_pipe, path_file_ady, logBookOrange, logBookName, numGreen):
    try:
        csv_ady_file =  open(path_file_ady)
        
        # EVENT(CSV file successfully opened) written to log
        logBookOrange.writeToLogEvent(f"Opening file {path_file_ady}")

        csv_reader = csv.reader(csv_ady_file, delimiter=',')
        
        # EVENT(CSV file is being read) written to log
        logBookOrange.writeToLogEvent(f"Reading file {path_file_ady}")
        
        readCSV(csv_ady_file, fd_pipe, csv_reader, logBookOrange, logBookName)

        # Indicate that the amount of adyacent nodes was all readede sucessfull
        ip = ""
        os.write(fd_pipe, messageBetweenGreenOrange(0, 1, ip.encode("utf-8")))

    except FileNotFoundError:
        # In case the CSV file does not exist, the pipe between nodes is closed
        ip = ""
        os.write(fd_pipe, messageBetweenGreenOrange(0, 0, ip.encode("utf-8")))
        # ERROR(CSV file unsuccesfully read) written to log
        logBookOrange.writeToLogError(f"Error opening adyacent list file of green node. The file was {path_file_ady}")
        os.close(fd_pipe) 
        os.rename(logBookName, f"../etc/LogBookOrange{numGreen}.txt")

        exit(1)

def main():
    # Assigning respective data from arguments     
    path_file_id = str(sys.argv[1])
    path_file_ady = str(sys.argv[2])
    PATH_PIPE_OtoG = str(sys.argv[3])

    path_logBook = str(f"../etc/LogBookOrange{os.getpid()}.txt") 
        
    logBookOrange = LogBook(path_logBook)

    
    # EVENT(path of ID file) written to log
    # writeToLog(f"The file with the identifier of Green Node is {path_file_id}", log)
    
    logBookOrange.writeToLogEvent(f"The file with the identifier of Green Node is {path_file_id}")

    # EVENT(path of CSV file) written to log
    logBookOrange.writeToLogEvent(f"The file with the adyacent list of Green Node is {path_file_ady}")

    fd_pipe_OtoG = os.open(PATH_PIPE_OtoG, os.O_WRONLY)

    # EVENT(opening pipe) written to log
    logBookOrange.writeToLogEvent(f"Opening pipe {PATH_PIPE_OtoG}")
    
    if fd_pipe_OtoG < 0:
        # ERROR(error opening pipe) written to log
        logBookOrange.writeToLogError(f"Error opening pipe {PATH_PIPE_OtoG}")
        exit(1)

    greenNum = readID( fd_pipe_OtoG, path_file_id, logBookOrange, path_logBook)
    readAdyNodes( fd_pipe_OtoG, path_file_ady, logBookOrange, path_logBook, greenNum)

    os.close(fd_pipe_OtoG)

    os.rename(path_logBook,  f"../etc/LogBookOrange{greenNum}.txt")

main()