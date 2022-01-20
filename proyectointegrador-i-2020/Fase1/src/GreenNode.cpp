#include "../headers/GreenNode.h"

GreenNode::GreenNode() {

    nodeID = {};

    std::vector<struct NodeInfo> nodeAdy;

    this->sharedLogBook = std::shared_ptr<LogBook>(new LogBook());
    
    sharedLogBook->writeToLogEvent("GreenNode:\tOrange layer is going to be initialized");
    orangeLayer = OrangeLayer(sharedLogBook);
    
    sharedLogBook->writeToLogEvent("GreenNode:\tLink layer is going to be initialized");
    linkLayer = LinkLayer(sharedLogBook);

    sharedLogBook->writeToLogEvent("GreenNode:\tBlue layer is going to be initialized"); 
    blueLayer = BlueLayer(sharedLogBook);
   
}

GreenNode::~GreenNode() {
    nodeAdy.clear();
}


/*
* Function: Creates threads and executes and modifies the instances in order for the green node to execute properly
* Modifies: orangeLayer, linkLayer, blueLayer and sharedLogBook
* Requirements: initialized class, csv file with node info and csv file with adyacent nodes info
* Returns: int (to determine whether it executed properly or had an error)
* Author: Daniela Vargas
*/
int GreenNode::execute(int argc, char* argv[]) {

    if(argc != 3) {
        sharedLogBook->writeToLogError("GreenNode:\tUse as first parameter the path and name of the file with the identifier of the green node, and \
        the second argument the path and name of the file of the adyacent list of the green node");
        
        return printf("GreenNode:\tUse as first parameter the path and name of the file with the identifier of the green node, and \
        the second argument the path and name of the file of the adyacent list of the green node.\n"), -1;
    }

    sharedLogBook->writeToLogEvent("GreenNode:\tThe program starts its execution");

    if(orangeLayer.execute(argv[1], argv[2]) < 0) {
        // Error in the orange node.
        sharedLogBook->writeToLogError("GreenNode:\tError in the orange layer");
        sharedLogBook->close();
        return -2;
    }

    nodeID = orangeLayer.getInfoNodeID(); 
    sharedLogBook->writeToLogEvent("GreenNode:\tThe green node has received its information coming from the orange node");
    nodeAdy = orangeLayer.getInfoNodesAdy(); 
    sharedLogBook->writeToLogEvent("GreenNode:\tThe green node has received the information of its adyacent nodes"); 
    orangeLayer.~OrangeLayer(); 

    //cambia nombre de bitacora
    char buffLogNum[MAXBUFF];
    std::string logBookNewName = LOGBOOK_PATH;
    sprintf(buffLogNum, "%d.txt", nodeID.nodeNum);
    logBookNewName.append(buffLogNum);
    sharedLogBook->setNewName(logBookNewName); 
    
    linkLayer.setGreenID(nodeID); 
    linkLayer.setGreenAdys(nodeAdy);
    sharedLogBook->writeToLogEvent("GreenNode:\tHas set the linkLayer's node ID and adyacent nodes info");

    blueLayer.setGreenNum(nodeID.nodeNum);
    blueLayer.setGreenIP(nodeID.ip);
    sharedLogBook->writeToLogEvent("GreenNode:\tHas set the blueLayer's node ID and adyacent nodes info");
    
    std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>> queueMessageToDispatcher = std::shared_ptr<std::queue<std::shared_ptr<struct MessageGreenGreen>>>(new std::queue<std::shared_ptr<struct MessageGreenGreen>>());
    std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>> queueMessageToBlueNode = std::shared_ptr<std::queue<std::shared_ptr<struct MessageForwarding>>>(new std::queue<std::shared_ptr<struct MessageForwarding>>());
    std::shared_ptr<std::mutex> mutexForWritingInQueue = std::shared_ptr<std::mutex>(new std::mutex()) ;
    std::shared_ptr<sem_t> semaphoreBlueLayer = std::shared_ptr<sem_t>(new sem_t());
    sem_init(&(*semaphoreBlueLayer), 1, 0);
    std::shared_ptr<sem_t> semaphoreDispatcher = std::shared_ptr<sem_t>(new sem_t());
    sem_init(&(*semaphoreDispatcher), 1, 0);
    std::shared_ptr<int> continueLoop(new int(1));
    
    sharedLogBook->writeToLogEvent("GreenNode:\tCreated linkLayer's thread and will begin its execution");
    std::thread threadLinkLayer(&LinkLayer::execute, &linkLayer, queueMessageToDispatcher, queueMessageToBlueNode, continueLoop, mutexForWritingInQueue, semaphoreBlueLayer, semaphoreDispatcher);
    sharedLogBook->writeToLogEvent("GreenNode:\tCreated blueLayer's thread and will begin its execution");
    std::thread threadBlueLayer(&BlueLayer::execute, &blueLayer, queueMessageToDispatcher, queueMessageToBlueNode, continueLoop, mutexForWritingInQueue, semaphoreBlueLayer, semaphoreDispatcher);

    threadLinkLayer.join();
    threadBlueLayer.join();
    sharedLogBook->close();
    return 0;
}