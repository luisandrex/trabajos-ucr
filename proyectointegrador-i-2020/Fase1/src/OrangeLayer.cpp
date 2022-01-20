#include "../headers/OrangeLayer.h"

OrangeLayer::OrangeLayer(){

    nodeID = {};

    std::vector<struct NodeInfo> vectorAdyNodes;

    this->logBook = NULL;
}

OrangeLayer::OrangeLayer(std::shared_ptr<LogBook> logBook) {
    this->logBook = logBook;

    nodeID = {};

    std::vector<struct NodeInfo> vectorAdyNodes;
}

OrangeLayer::~OrangeLayer() {
    logBook = NULL;
    vectorAdyNodes.clear();
}

int OrangeLayer::getNodeID() {
    if(read(pipeOrangeGreen, &nodeID, sizeof(struct NodeInfo)) < 0) {
        logBook->writeToLogError("OrangeLayer:\tError reading in the pipe between GreenNode and OrangeNode");
        printf("OrangeLayer:\tError reading in the pipe between GreenNode and OrangeNode.\n");
        return -1;
    }
    if(nodeID.nodeNum < 0) {
        logBook->writeToLogError("OrangeLayer:\tError reading file of ID node. The file does not exist");
        printf("OrangeLayer:\tError reading file of ID node.\n");
        return -2;
    }
    logBook->writeToLogEvent("OrangeLayer:\tThe ID of the green node read sucessfully");
    return 0;
}

int OrangeLayer::getAdyNodes() {
    bool continueReading = true;
    struct NodeInfo adyNode;
    while(continueReading) {
        if(read(pipeOrangeGreen, &adyNode, sizeof(struct NodeInfo)) < 0) {
            logBook->writeToLogError("OrangeLayer:\tError reading in the pipe between GreenNode and OrangeNode");
            printf("OrangeLayer:\tError reading in the pipe between GreenNode and OrangeNode.\n");
            return -1;
        }
        if(adyNode.nodeNum == 0) {
            if(adyNode.portUDP) {
                logBook->writeToLogEvent("OrangeLayer:\tThe total amount of adyacent nodes read sucessfully");
                continueReading = false;
            } else {
                logBook->writeToLogError("OrangeLayer:\tError reading file of adyacent nodes. The file does not exist");
                printf("OrangeLayer:\tError reading file of adyacent nodes. The file does not exist.\n");
                return -2;
            }
        } else {
            vectorAdyNodes.emplace_back(adyNode);
        }
    }
    return 0;
}

int OrangeLayer::execute(std::string pathIDFile, std::string pathAdyFile) {
    std::string pathPipe = PATH_PIPE;
    char buff[MAXBUFF];
    sprintf(buff, "%d", getpid());
    pathPipe.append(buff);
    if(mkfifo(pathPipe.c_str(), 0666) < 0){
        // ERROR(pipe could not be created) written to log
        logBook->writeToLogError("OrangeLayer:\tError creating pipe between Green and Orange Node");
        printf("OrangeLayer:\tError creating pipe between Green and Orange Node.\n");
        return -1;
    }

    
    int forkID = fork();
    if(forkID < 0){
        // ERROR(error creating fork) written to log
        logBook->writeToLogError("OrangeLayer:\tError creating fork for OrangeLayer of Green Node");
        printf("OrangeLayer:\tError creating fork for OrangeLayer of Green Node.\n");
        return -3;
    } 
    else if(forkID == 0) {
        // child process executes orange node code
        execlp(PYTHON, PYTHON, ORANGE_NODE, pathIDFile.c_str(), pathAdyFile.c_str(), pathPipe.c_str(), NULL);
    } else {

        pipeOrangeGreen = open(pathPipe.c_str(), O_RDONLY);
        if(pipeOrangeGreen < 0) {
            // ERROR (error opening pipe) written to log
            logBook->writeToLogError("OrangeLayer:\tError opening pipe in OrangeLayer of Green node");
            printf("OrangeLayer:\tError opening pipe in OrangeLayer of Green node");
            return -2;
        }

        logBook->writeToLogEvent("OrangeLayer:\tFork has been executed without producing an error");
        if(getNodeID() < 0) {
            logBook->writeToLogError("OrangeLayer:\tError reading information of greenNode");
            return -4;
        }

        if(getAdyNodes() < 0) {
            logBook->writeToLogError("OrangeLayer:\tError reading information of adyacent nodes of the greenNode");
            return -5;
        }
        waitpid(forkID, NULL, WNOHANG);
    }
    close(pipeOrangeGreen);
    remove(pathPipe.c_str());
    return 0;
}

struct NodeInfo OrangeLayer::getInfoNodeID() {
    return nodeID;
}

std::vector<struct NodeInfo> OrangeLayer::getInfoNodesAdy() {
    return vectorAdyNodes;
}