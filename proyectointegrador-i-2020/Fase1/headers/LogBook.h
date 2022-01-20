#ifndef LOGBOOK
#define LOGBOOK

#include <semaphore.h>
#include <iostream>
#include <string>
#include <sys/types.h>
#include <unistd.h>
#include <time.h>
#include <sys/time.h>
#include <mutex>         

#define LOGBOOK_PATH "../etc/LogBookGreen"
#define MAXBUFF 50
/**
 * Definition: This class is used by the other classes to generate the logbook of the several events/errors/warning that occur during the execution of the program.
 */
class LogBook{
    private:
        FILE* logBook;
        std::mutex* mutexLogBook;
        int numEvent;
        int numError;
        int numWarning;
        std::string logBookNamepid;
        std::string logBookName;
    public:
        LogBook();
        ~LogBook();
        
        /**
         * Function: writes a certain event to a certain node's log, where the node was implemented in Python
         * Modifies: modifies a certain node's log by writing certain event to it 
         * Requirements: event, which is a string that contains the details of said event
         * Returns: this function does not return anything
         */
        void writeToLogEvent(std::string event);
        
        /**
         * Function: writes a certain error to a certain node's log, where the node was implemented in Python
         * Modifies: modifies a certain node's log by writing certain event to it 
         * Requirements: error, which is a string that contains the details of said error
         * Returns: this function does not return anything
         */
        void writeToLogError(std::string error);
        
        /**
         * Function: writes a certain warning to a certain node's log, where the node was implemented in Python
         * Modifies: modifies a certain node's log by writing certain event to it 
         * Requirements: error, which is a string that contains the details of said warning
         * Returns: this function does not return anything
         */
        void writeToLogWarning(std::string warning);
        
         /**
         * Function: This function changes the name of the logBook to match the name of the node. 
         * Modifies: modifies the attribute logBookName
         * Requirements: warning, which is a string that contains the name of the node
         * Returns: this function does not return anything
         */
        void setNewName(std::string name);
        
        /**
         * Function: This function closes the file corresponding to the logBook and changes its name to match the name of the node instead of the process id. 
         * Modifies: Closes the .txt corresponding to the logBook and changes the name of the file.
         * Requirements: This method doesn't require anything.
         * Returns: this function does not return anything
         */
        void close();
};
#endif