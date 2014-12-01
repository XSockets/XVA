#include "Arduino.h"
#include <Ethernet.h>
#include <SPI.h>
#include <XSocketClient.h>

int currentDistance = 0;
int threshold = 20;

//TCP/IP
byte mac[] = { 
  0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
XSocketClient client;

void setup() {
  Serial.begin(9600);
  Ethernet.begin(mac);
  Serial.println("Start to connect");
  if(client.connect("192.168.1.3",4502)){
    Serial.println("Connected!");
  }
  else{
    Serial.println("Error connecting");
  }

  client.setOnMessageDelegate(onMessage);
  delay(2000);
  client.send("sensor","set_hardware","Arduino");
}

void loop() {
  client.receiveData();
  
  int distance = random(1,51);
  if(distance <= threshold && distance > 0 && distance < 50)
  {
    if(currentDistance != distance)
    {
      currentDistance = distance;
      char outBuf[6];   
      sprintf(outBuf,"%d",distance);
      Serial.println(outBuf);
      client.send("sensor","change",outBuf);        
    }
  }
  delay(1000);
}

//Message received
void onMessage(XSocketClient client, String data) {
  if (client.getValueAtIx(data,1) == "threshold")
  {    
    threshold = client.getValueAtIx(data,2).toInt();	  
    Serial.println("New threshold: " + threshold);
  }
}