#ifndef GREENNODE
#define GREENNODE

#include "LogBook.h"
#include "OrangeLayer.h"
#include "BlueLayer.h"
#include "PinkLayer.h"
#include "Forwarding.h"
#include "Broadcast.h"
#include "Includes.h"
#include "Packets.h"

#define LOGBOOK_PATH "../etc/LogBookGreen"
#define LOGBOOK_PATH_PINK "../etc/LogBookPink"
#define MAXBUFF 50
#define PIPE_PATH_PINK_GREEN "../tmp/pipePG"
#define PIPE_PATH_GREEN_PINK "../tmp/pipeGP"
#define PIPE_PATH_BLUE_LINK "../tmp/pipeBL"
#define PIPE_PATH_LINK_BLUE "../tmp/pipeLB"

class GreenNode{
    private:
        std::shared_ptr<LogBook> sharedLogBook;

        OrangeLayer orangeLayer;
        BlueLayer blueLayer;
        LinkLayer linkLayer;

        struct NodeInfo nodeID;

        std::vector<struct NodeInfo> nodeAdy;

    public:
        GreenNode();

        ~GreenNode();
        /*
        inicializar y crear instancias, hacer los hilos
         
        metodo ejecutar naranja(orangelayer.h)

        llama a otras clases/funcionalidades
        1. Bitacora
        2. Naranja (execute y luego gets)
        3. Hilo de la capa de enlace (comm entre verdes)
        4. Hilo con la capa de transporte (comm entre verde-azu)
        */
        int execute(int argc, char* argv[]);

};
#endif