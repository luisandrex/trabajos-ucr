import os
import datetime
import time
import threading 

class LogBook:
    

    def __init__(self, path):
        self.file = open(path, "w+")
        self.mutex = threading.Lock()
        self.numberEvent = 0
        self.numberError = 0
        self.numberWarning = 0

    #Function: writes a certain event to a certain node's log, where the node was implemented in Python
    #Modifies: modifies a certain node's log by writing certain event to it 
    #Requirements: event, which is a string that contains the details of said event
    #Returns: this function does not return anything
    def writeToLogEvent(self, event):
        now = datetime.datetime.now()
        ms = (int)((now.microsecond) / 1000)
        dateAndTime = now.strftime("%H:%M:%S")
        self.mutex.acquire()
        self.file.write(f"{dateAndTime}.{ms}\t Event #{self.numberEvent}: {event}.\n")
        self.numberEvent += 1    
        self.file.flush()
        self.mutex.release()

    #Function: writes a certain error to a certain node's log, where the node was implemented in Python
    #Modifies: modifies a certain node's log by writing certain event to it 
    #Requirements: error, which is a string that contains the details of said error
    #Returns: this function does not return anything
    def writeToLogError(self, error):
        now = datetime.datetime.now()
        ms = (int)((now.microsecond) / 1000)
        dateAndTime = now.strftime("%H:%M:%S")
        self.mutex.acquire()
        self.file.write(f"{dateAndTime}.{ms}\t Error #{self.numberError}: {error}.\n")
        self.numberError = self.numberError + 1
        self.file.flush()
        self.mutex.release()

    #Function: writes a certain warning to a certain node's log, where the node was implemented in Python
    #Modifies: modifies a certain node's log by writing certain event to it 
    #Requirements: error, which is a string that contains the details of said warning
    #Returns: this function does not return anything
    def writeToLogWarning(self, warning):
        now = datetime.datetime.now()
        ms = (int)((now.microsecond) / 1000)
        dateAndTime = now.strftime("%H:%M:%S")
        self.mutex.acquire()
        self.file.write(f"{dateAndTime}.{ms}\t Warning #{self.numberWarning}: {warning}.\n")
        self.numberWarning = self.numberWarning + 1
        self.file.flush()
        self.mutex.release()

