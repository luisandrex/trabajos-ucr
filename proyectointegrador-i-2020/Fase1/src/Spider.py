#!/usr/bin/env python3
import os 
import socket 
import sys   
import shutil
import posix_ipc
import time

# Author: Fernando Morales  
# Function: ask the sandbox via mailbox certain request and, when received, write the result to the luggage
# Modifies: the luggage, writing the result of the request onto it
# Requirements: Both mailboxes open, the luggage file open, the number of spider and the number of request
# Returns: void
def ask_for_message(my_mailbox_send, my_mailbox_receive,luggage, spider_num, request_number):
    requests_to_sandbox = [int(spider_num),int(request_number)]
    my_mailbox_send.send(bytes(requests_to_sandbox))
    if int(request_number) != 0:
        respuesta = my_mailbox_receive.receive()
        respuesta_message = respuesta[0].decode()
        luggage.write(f"{respuesta_message}\n")

# Function: read arguments, read the luggage for the stats required and opening everything that it needs
# Modifies: the luggage, writing the result of the stats 
# Requirements: the spider number, the node in which it is in, the luggage name, and both mailboxes
# Returns: void
def main():
    spider_num = str(sys.argv[1])
    node_num = str(sys.argv[2])
    luggage_name = str(sys.argv[3])
    mailbox = str(sys.argv[4])
    mailbox2 = str(sys.argv[5])

    my_mailbox_send = posix_ipc.MessageQueue(mailbox) 
    my_mailbox_receive = posix_ipc.MessageQueue(mailbox2) 

    luggage = open(f"{luggage_name}", "r+")

    luggage.flush()
    os.fsync(luggage.fileno())
    
    physical_stats = luggage.readline()
    physical_stats_v = physical_stats.split(",")

    green_node_stats = luggage.readline()
    green_node_stats_v = green_node_stats.split(",")

    luggage.write(f"{node_num}\n")

    mem_T = shutil.disk_usage('/')[0]
    mem_F = shutil.disk_usage('/')[2]

    statvfs = os.statvfs('/home')
    # System info
    if "1" in physical_stats_v:
        # total free space
        mem_T_MB = mem_T/1000000
        luggage.write(f"{mem_T_MB}\n")
    if "2" in physical_stats_v:
        percent_memory_free = str(round(1 - (int(float(mem_F)) / int(float(mem_T))),2))
        luggage.write(f"{percent_memory_free}\n")
    if "3" in physical_stats_v:
        # total space in file system
        total_file_system_space_bytes = statvfs.f_frsize * statvfs.f_blocks
        total_file_system_space_MB = str(round(total_file_system_space_bytes / 1000000000))
        luggage.write(f"{total_file_system_space_MB}\n")
    if "4" in physical_stats_v:
        # free space % in file system
        total_file_system_space_bytes = statvfs.f_frsize * statvfs.f_blocks
        free_file_system_space_bytes = statvfs.f_frsize * statvfs.f_bfree
        percent_file_system_free = str(round(1 - (free_file_system_space_bytes / total_file_system_space_bytes),2))
        luggage.write(f"{percent_file_system_free}\n")

    #Green Node Info requests
    possible_requests = [1,2,3,4,5,6,7,0]
    for x in possible_requests: 
        if str(x) in green_node_stats_v:
            ask_for_message(my_mailbox_send, my_mailbox_receive,luggage, int(spider_num), x)
        elif x == 0:
            luggage.flush()
            luggage.close()
            ask_for_message(my_mailbox_send, my_mailbox_receive,luggage, int(spider_num), 0)

    my_mailbox_receive.close()
    my_mailbox_send.close()
    # my_mailbox_receive.unlink()

main()