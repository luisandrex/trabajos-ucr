#include "../headers/LogBook.h"

LogBook::LogBook() {
    logBookNamepid = LOGBOOK_PATH;
    char buff[MAXBUFF];
    sprintf(buff, "%d.txt", getpid());
    logBookNamepid.append(buff);
    this->logBookName = logBookNamepid;
    this->logBook = fopen(logBookNamepid.c_str(), "w+");
    this->numEvent = 0;
    this->numError = 0;
    this->numWarning = 0;     
    this->mutexLogBook = new std::mutex();
}

LogBook::~LogBook(){
    delete this->mutexLogBook;
}   

void LogBook::writeToLogEvent(std::string event) {
    this->mutexLogBook->lock();
    char buffer[30];
    struct timeval tv;
    time_t curtime;
    gettimeofday(&tv, NULL); 
    curtime = tv.tv_sec;
    strftime(buffer,30,"%T.",localtime(&curtime));
    //printf("%s%ld\tEVENT #%d: %s.\n",buffer,tv.tv_usec/1000, this->numEvent, event.c_str());
    fprintf(logBook, "%s%ld\tEvent #%d: %s.\n",buffer,tv.tv_usec/1000, numEvent, event.c_str());
    fflush(this->logBook);
    this->numEvent++;
    this->mutexLogBook->unlock();
    return;
}

void LogBook::writeToLogError(std::string error) {
    this->mutexLogBook->lock();
    char buffer[30];
    struct timeval tv;
    time_t curtime;
    gettimeofday(&tv, NULL); 
    curtime = tv.tv_sec;
    strftime(buffer,30,"%T.",localtime(&curtime));
    //printf("%s%ld\tERROR #%d: %s.\n",buffer,tv.tv_usec/1000, this->numError, error.c_str());
    fprintf(this->logBook, "%s%ld\tError #%d: %s.\n",buffer,tv.tv_usec/1000, this->numError, error.c_str());
    fflush(this->logBook);
    this->numError++;
    this->mutexLogBook->unlock();
    return;
}

void LogBook::writeToLogWarning(std::string warning) {
    this->mutexLogBook->lock();
    char buffer[30];
    struct timeval tv;
    time_t curtime;
    gettimeofday(&tv, NULL); 
    curtime = tv.tv_sec;
    strftime(buffer,30,"%T.",localtime(&curtime));
    //printf("%s%ld\tWARNING #%d: %s.\n",buffer,tv.tv_usec/1000, this->numWarning, warning.c_str());
    fprintf(this->logBook, "%s%ld\tWarning #%d: %s.\n",buffer,tv.tv_usec/1000, this->numWarning, warning.c_str());
    fflush(this->logBook);
    this->numWarning++;
    this->mutexLogBook->unlock();
    return;
}

void LogBook::setNewName(std::string name) {
    logBookName = name;
}

void LogBook::close() {
    fclose(this->logBook);
    const char * logBookNewName = logBookName.c_str();
    const char * lobBookOldName = this->logBookNamepid.c_str();
    rename(lobBookOldName,logBookNewName);
} 