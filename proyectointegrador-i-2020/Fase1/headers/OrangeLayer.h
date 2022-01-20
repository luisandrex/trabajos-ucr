#ifndef ORANGELAYER
#define ORANGELAYER

#include "Includes.h"
#define PYTHON "python3"
#define ORANGE_NODE "OrangeNode.py"
#define PATH_PIPE "../tmp/pipeGO"
#define NAME_LOGBOOK "../etc/LogBookGreen"
#define MAXBUFF 50
/**
 * Definition:      Is an intermediate between green node and its orange node. It initialize the orange node and get its information
 * Authors:         Jose Andres Viquez
 */
class OrangeLayer{
    private:

        std::shared_ptr<LogBook> logBook;

        struct NodeInfo nodeID;
        std::vector<struct NodeInfo> vectorAdyNodes;

        int pipeOrangeGreen;

        std::string pathIDFile;
        std::string pathAdyFile;
        
        /**
         * Function:    Read the identity of the green node from the pipe use to communicate between green node and its orange 
         *              node.
         * Parameters:  Does not requires parameters.
         * Modify:      Update the private data nodeID with the information of the green node.
         * Return:      A variable indicating a posible error. 0 indicate no error, a negative value indicate error.
         */ 
        int getNodeID();
        /**
         * Function:    Read the list of adyacent nodes from the pipe use to communicate between green node and its orange node.
         * Parameters:  Does not requires parameters.
         * Modify:      Update the private data vectorAdyNodes with the information of the adyacent nodes of the actual green node.
         * Return:      A variable indicating a posible error. 0 indicate no error, a negative value indicate error.
         */
        int getAdyNodes();

    public:
        OrangeLayer();
        OrangeLayer(std::shared_ptr<LogBook> parLogBook);
        ~OrangeLayer();
        
        /**
         * Function:    Get the information of the green node read.
         * Parameters:  Does not requiers anything.
         * Modify:      Does not modify anything.
         * Return:      A struct with the information of the green node.
         */ 
        struct NodeInfo getInfoNodeID();
        /**
         * Function:    Get the list of adyacent nodes of the green node. 
         * Parameters:  Does not requiers anything.
         * Modify:      Does not modify anything.
         * Return:      A vetor with all the information of the adyacent nodes of the green node.
         */
        std::vector<struct NodeInfo> getInfoNodesAdy();
        /**
         * Function:    Execute all the functionality of the Orange Node. It create the pipe between green and 
         *              orange node. Then, it read, from the pipe, the data of the identity of the green node and the 
         *              list of adyacent nodes.
         * Parameter:   Path of the file with the identify file.
         * Parameter:   Path of the file with the list of adyacent nodes.
         * Modify:      Update the private data nodeInfo and vectorAdyNodes after read them from the pipe.
         * Return:      An error flag. 0 indicate no error, otherwise indicate an error.
         */
        int execute(std::string parPathIDFile, std::string parPathAdyFile); // Get ID info and ady nodes info

};
#endif