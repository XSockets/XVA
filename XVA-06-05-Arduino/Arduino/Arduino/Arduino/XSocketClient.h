#ifndef XSOCKETCLIENT_H
#define XSOCKETCLIENT_H_

#include <string.h>
#include <stdlib.h>
#include <WString.h>
#include <Ethernet.h>
#include "Arduino.h"

class XSocketClient {
	public:
		typedef void (*OnMessageDelegate)(XSocketClient client, String data);
		bool connect(char hostname[], int port = 80);
        bool connected();
        void disconnect();
		void receiveData();
		void setOnMessageDelegate(OnMessageDelegate onMessageDelegate);
		void send(String controller, String topic, String data);
		void addListener(String controller, String topic);
		void removeListener(String controller, String topic);
		String getValueAtIx(String data, int index);
	private:        
        void sendHandshake();
        EthernetClient _client;
        OnMessageDelegate _onMessageDelegate;
        bool readHandshake();
        String readHandshakeLine();
};


#endif
