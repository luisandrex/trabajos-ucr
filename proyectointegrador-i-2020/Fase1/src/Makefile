all: bin etc tmp
	g++ -std=c++11 -g main.cpp GreenNode.cpp OrangeLayer.cpp BlueLayer.cpp PinkLayer.cpp LinkLayer.cpp \
	LogBook.cpp Forwarding.cpp Broadcast.cpp StatisticsDataPlane.cpp -o ../bin/GreenNode -lrt -pthread
run: all etc
	../bin/GreenNode
bin:
	mkdir -p ../bin
etc:
	mkdir -p ../etc
tmp:
	mkdir -p ../tmp

clean: 
	$(RM) ../etc/LogBook* 
	$(RM) __pycache__/*
	rm -rf __pycache__/
	$(RM) ../tmp/*
	mkdir -p ../tmp
