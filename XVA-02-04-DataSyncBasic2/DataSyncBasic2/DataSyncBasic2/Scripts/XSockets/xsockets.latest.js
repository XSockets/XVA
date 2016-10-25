/**
 * Client-side controller(s) for full duplex communication with the server-controller(s)
 */
var xsockets;
(function (xsockets) {
    var controller = (function () {
        /**
         * Ctor for client side controller.
         * @param itransport - the communication layer
         * @param _name - the name of the controller
         */
        function controller(itransport, _name) {
            this._isOpen = false;
            this._subscriptions = {};
            this.promises = {};
            /**
             * Will be fired when the controller is opened
             */
            this.onOpen = function (connInfo) { };
            /**
             * Will be fired when the controller is closed
             */
            this.onClose = function () { };
            /**
             * Will be fired when there is a message dispatched to the
             * controller and there is no promise/subscription for the topic
             */
            this.onMessage = function () { };
            this._transport = itransport;
            this.name = _name;
        }
        /**
         * Dispatches a message to the promise or subscription for the topic.
         * If no promise/subscription is found the onmessage event on the controller will be fired
         * @param message - the imessage object received from the server
         */
        controller.prototype.dispatchEvent = function (message) {
            // Acknowledge if qos equals atleastonce
            if (message.Q == xsockets.qos.atleastOnce) {
                if (this._transport.isConnected() && this._isOpen) {
                    //var m = new xsockets.message(message.C, events.ack);
                    //m.I = message.I;
                    //("{'T':'" + XSockets.Events.msgAck + "','I':'" + msg.id + "','C':'" + msg.controller + "','Q':'" + 1 + "'}")
                    this._transport.socket.send("{'T':'" + xsockets.events.ack + "','I':'" + message.I + "','C':'" + message.C + "','Q':'" + 1 + "'}");
                }
            }
            switch (message.T) {
                case xsockets.events.open:
                    this._isOpen = true;
                    var clientInfo = JSON.parse(message.D);
                    this._controllerId = clientInfo.CI;
                    this._transport.setPersistentId(clientInfo.PI);
                    this.onOpen(clientInfo);
                    break;
                case xsockets.events.close:
                    this._isOpen = false;
                    this.onClose();
                    break;
                default:
                    if (!this.firePromise(message) && !this.fireSubscription(message)) {
                        this.onMessage(message);
                    }
            }
        };
        /**
         * If there is a promise for the topic on the message it wil be fired.
         * Return true if a promise was fired, otherwise false
         * @param message - the received imessage
         */
        controller.prototype.firePromise = function (message) {
            //Check promises
            var cb = this.promises[message.T];
            if (cb !== undefined) {
                if (message.messageType === xsockets.messageType.text) {
                    cb(JSON.parse(message.D));
                }
                else {
                    cb(message.D);
                }
                delete this.promises[message.T];
                return true;
            }
            return false;
        };
        /**
         * If there is a subscription for the topic on the message it wil be fired.
         * Return true if a subscription was fired, otherwise false
         * @param message - the received imessage
         */
        controller.prototype.fireSubscription = function (message) {
            //Check pub/sub and rpc
            var cb = this._subscriptions[message.T];
            if (cb !== undefined) {
                if (message.messageType === xsockets.messageType.text) {
                    cb(JSON.parse(message.D));
                }
                else {
                    cb({ binary: message.B, metadata: message.D });
                }
                return true;
            }
            return false;
        };
        /**
         * Open up the controller server-side.
         *
         * If the transport/socket is open the controller will communicate to the server to open a instance of the server-side controller
         */
        controller.prototype.open = function () {
            if (this._transport.isConnected() && !this._isOpen) {
                this._transport.socket.send(new xsockets.message(this.name, xsockets.events.init));
            }
        };
        /**
         * Close the controller both server-side and client-side (opitonal)
         * @param dispose - if true the client-side controller will be disposed
         */
        controller.prototype.close = function (dispose) {
            if (dispose === void 0) { dispose = false; }
            //if socket open... send close message
            if (this._transport.isConnected() && this._isOpen) {
                this._transport.socket.send(new xsockets.message(this.name, xsockets.events.close));
            }
            this.onClose();
            if (dispose) {
                this._transport.disposeController(this);
            }
        };
        /**
         * Add a callback that will fire for a specific topic
         * @param topic - the topic to add a callback for
         * @param callback - the callback function to fire when the topic arrives
         */
        controller.prototype.on = function (topic, callback) {
            topic = topic.toLowerCase();
            if (typeof callback === "function") {
                this._subscriptions[topic] = callback;
            }
            if (typeof callback === "undefined") {
                delete this._subscriptions[topic];
            }
        };
        /**
         * Removes a callback for a specific topic
         * @param topic - the topic to remove the callback for
         */
        controller.prototype.off = function (topic) {
            topic = topic.toLowerCase();
            delete this._subscriptions[topic];
        };
        /**
         * Call a method on the server-side controller
         * @param topic - the method to call
         * @param data - the serializable data to pass into the method
         */
        controller.prototype.invoke = function (topic, data) {
            topic = topic.toLowerCase();
            if (this._transport.isConnected()) {
                if (data === undefined) {
                    data = "";
                }
                var m = new xsockets.message(this.name, topic, data);
                this._transport.socket.send(m);
            }
            return new xsockets.promise(this, topic);
        };
        /**
         * Send binary data to the XSockets controller
         * @param topic - topic/method to call
         * @param arrayBuffer - the binary data to send
         * @param data - metadata such as information about the binary data
         */
        controller.prototype.invokeBinary = function (topic, arrayBuffer, data) {
            if (data === void 0) { data = undefined; }
            topic = topic.toLowerCase();
            if (this._transport.isConnected()) {
                var bm = new xsockets.message(this.name, topic, data, arrayBuffer);
                this._transport.socket.send(bm.createBuffer());
            }
            return this;
        };
        /**
         * Send binary data to the XSockets controller
         * @param topic - topic/method to call
         * @param arrayBuffer - the binary data to send
         * @param data - metadata such as information about the binary data
         */
        controller.prototype.publishBinary = function (topic, arrayBuffer, data) {
            if (data === void 0) { data = undefined; }
            return this.invokeBinary(topic, arrayBuffer, data);
        };
        /**
         * Creates a subscription on the server for the specific topic
         * @param topic - the topic to subscribe to
         * @param callback - the callback to fire when a message with the topic is published
         */
        controller.prototype.subscribe = function (topic, callback) {
            topic = topic.toLowerCase();
            this.on(topic, callback);
            if (this._transport.isConnected() && typeof callback === "function") {
                var m = new xsockets.message(this.name, xsockets.events.subscribe, {
                    T: topic,
                    A: false //cb ? true : false
                });
                this._transport.socket.send(m);
            }
        };
        /**
         * Remove the subscription from the server
         * @param topic - the topic to cancel the subscription for
         */
        controller.prototype.unsubscribe = function (topic) {
            topic = topic.toLowerCase();
            delete this._subscriptions[topic];
            if (this._transport.isConnected()) {
                var m = new xsockets.message(this.name, xsockets.events.unsubscribe, {
                    T: topic,
                    A: false
                });
                this._transport.socket.send(m);
            }
        };
        /**
         * Publish a message for a specific topic.
         * @param topic - topic for publish message
         * @param data - data to publish
         */
        controller.prototype.publish = function (topic, data) {
            topic = topic.toLowerCase();
            return this.invoke(topic, data);
        };
        controller.prototype.setProperty = function (name, value) {
            return this.invoke('set_' + name, value);
        };
        controller.prototype.getProperty = function (name, callback) {
            var _this = this;
            this.on('get_' + name, function (d) {
                _this.off('get_' + name);
                callback(d);
            });
            return this.invoke('get_' + name, undefined);
        };
        return controller;
    }());
    xsockets.controller = controller;
})(xsockets || (xsockets = {}));
var xsockets;
(function (xsockets) {
    (function (messageType) {
        messageType[messageType["text"] = 0] = "text";
        messageType[messageType["binary"] = 1] = "binary";
    })(xsockets.messageType || (xsockets.messageType = {}));
    var messageType = xsockets.messageType;
    (function (qos) {
        qos[qos["fireAndForget"] = 0] = "fireAndForget";
        qos[qos["atleastOnce"] = 1] = "atleastOnce";
    })(xsockets.qos || (xsockets.qos = {}));
    var qos = xsockets.qos;
})(xsockets || (xsockets = {}));
var xsockets;
(function (xsockets) {
    var message = (function () {
        /**
         * Ctor for message
         * @param controller - the controller name
         * @param topic - the name of the server-side method
         * @param data - the object received (or to send)
         */
        function message(controller, topic, data, binary) {
            if (data === void 0) { data = undefined; }
            if (binary === void 0) { binary = undefined; }
            this.C = controller;
            this.T = topic;
            this.B = binary;
            if (!xsockets.utils.isJson(data)) {
                this.D = JSON.stringify(data);
            }
            else {
                this.D = data;
            }
            if (this.B == undefined) {
                this.messageType = xsockets.messageType.text;
            }
            else {
                this.messageType = xsockets.messageType.binary;
            }
        }
        /**
         * Use this to create a binary message to send to the server
         * The object need to have the arraybuffer (B) set for this to work
         */
        message.prototype.createBuffer = function () {
            var payload = this.toString();
            var header = new Uint8Array(this.longToByteArray(payload.length));
            return this.appendBuffer(this.appendBuffer(header, this.stringToBuffer(payload)), this.B);
        };
        /**
         * Extract a message from binary data received from the server
         */
        message.prototype.extractMessage = function () {
            var ab2str = function (buf) { return String.fromCharCode.apply(null, new Uint16Array(buf)); };
            var byteArrayToLong = function (byteArray) {
                var value = 0;
                for (var i = byteArray.byteLength - 1; i >= 0; i--) {
                    value = (value * 256) + byteArray[i];
                }
                return value;
            };
            var header = new Uint8Array(this.B, 0, 8);
            var payloadLength = byteArrayToLong(header);
            var offset = 8 + byteArrayToLong(header);
            var buffer = new Uint8Array(this.B, offset, this.B.byteLength - offset);
            var str = new Uint8Array(this.B, 8, payloadLength);
            var result = this.parse(ab2str(str), buffer);
            result.D = typeof result.D === "object" ? result.D : JSON.parse(result.D);
            return result;
        };
        message.prototype.parse = function (text, binary) {
            var data = JSON.parse(text);
            return new message(data.C, data.T, data.D || JSON.stringify({}), binary);
        };
        ;
        /**
         * Return the string representation of the imessage
         */
        message.prototype.toString = function () {
            return JSON.stringify({ C: this.C, D: this.D, T: this.T, Q: this.Q, R: this.R, I: this.I });
        };
        message.prototype.appendBuffer = function (a, b) {
            var c = new Uint8Array(a.byteLength + b.byteLength);
            c.set(new Uint8Array(a), 0);
            c.set(new Uint8Array(b), a.byteLength);
            return c.buffer;
        };
        message.prototype.stringToBuffer = function (str) {
            var len = str.length, arr = new Array(len);
            for (var i = 0; i < len; i++) {
                arr[i] = str.charCodeAt(i) & 0xFF;
            }
            return new Uint8Array(arr).buffer;
        };
        message.prototype.longToByteArray = function (size) {
            var byteArray = [0, 0, 0, 0, 0, 0, 0, 0];
            for (var index = 0; index < byteArray.length; index++) {
                var byte = size & 0xff;
                byteArray[index] = byte;
                size = (size - byte) / 256;
            }
            return byteArray;
        };
        return message;
    }());
    xsockets.message = message;
})(xsockets || (xsockets = {}));
/**
 * Static info about the xsockets client, such as events and version.
 */
var xsockets;
(function (xsockets) {
    xsockets.version = '6.2.0';
    var events = (function () {
        function events() {
        }
        events.authfailed = '0';
        events.init = '1';
        events.open = '2';
        events.close = '3';
        events.error = '4';
        events.subscribe = '5';
        events.unsubscribe = '6';
        events.ping = '7';
        events.pong = '8';
        events.ack = '9';
        return events;
    }());
    xsockets.events = events;
    /**
     * Will probably be removed in v6, not used rigth now.
     */
    var storage = (function () {
        function storage() {
        }
        storage.set = 's1';
        storage.get = 's2';
        storage.clear = 's3';
        storage.remove = 's4';
        return storage;
    }());
    xsockets.storage = storage;
    var utils = (function () {
        function utils() {
        }
        utils.guid = function () {
            var a, b;
            for (b = a = ''; a++ < 36; b += a * 51 & 52 ? (a ^ 15 ? 8 ^ Math.random() * (a ^ 20 ? 16 : 4) : 4).toString(16) : '-')
                ;
            return b;
        };
        utils.isJson = function (str) {
            try {
                JSON.parse(str);
            }
            catch (e) {
                return false;
            }
            return true;
        };
        return utils;
    }());
    xsockets.utils = utils;
})(xsockets || (xsockets = {}));
var xsockets;
(function (xsockets) {
    var promise = (function () {
        /**
         * Ctor for promise, that attaches the promise to a controller and a topic
         * @param controller - the controller instance that is communicating
         * @param name - the name of the method that expects to return a result
         */
        function promise(controller, name) {
            this._controller = controller;
            this._name = name;
        }
        /**
         * Adds a callback for a call the the server-side that is expected to return a result
         * @param fn
         */
        promise.prototype.then = function (fn) {
            this._controller.promises[this._name] = fn;
        };
        return promise;
    }());
    xsockets.promise = promise;
})(xsockets || (xsockets = {}));
/**
 * XSockets.NET - WebSocket-client transport
 */
var xsockets;
(function (xsockets) {
    var client = (function () {
        /**
         * Ctor for transport, example new xsocketsClient('ws://somehost.com:80',['foo', 'bar']);
         * @param server - uri to server, example ws://somehost.com:80
         * @param controllers - array of controllers use at startup, example ['foo','bar']
         */
        function client(server, controllers) {
            if (controllers === void 0) { controllers = []; }
            this._autoReconnect = false;
            this._autoReconnectTimeout = 5000;
            this._autoHeartbeat = false;
            this._autoHeartbeatTimeout = 30000;
            this._qos = false;
            this._readyState = WebSocket.CLOSED;
            this.onOpen = function (event) { };
            this.onAuthenticationFailed = function (event) { };
            this.onClose = function (event) { };
            this.onMessage = function (event) { };
            this.onError = function (event) { };
            this._parameters = {};
            this._persistentId = localStorage.getItem(server);
            this._server = server;
            this.subprotocol = "XSocketsNET";
            this._controllers = new Array();
            for (var c in controllers) {
                this._controllers.push(new xsockets.controller(this, controllers[c].toLowerCase()));
            }
        }
        /**
         * Enables/disables the autoreconnect feature
         * @param enabled - sets the current state, default = true
         * @param timeout - timeout in ms, default = 5000
         */
        client.prototype.autoReconnect = function (enabled, timeout) {
            if (enabled === void 0) { enabled = true; }
            if (timeout === void 0) { timeout = 5000; }
            this._autoReconnect = enabled;
            this._autoReconnectTimeout = timeout;
        };
        /**
         * Set the parameters that you want to pass in with the connection.
         * Do this before calling open
         * @param params - parameters to pass in, example: {foo:'bar',baz:123}
         */
        client.prototype.setParameters = function (params) {
            this._parameters = params;
        };
        /**
         * Call before calling open
         * @param guid - sets the persistentid for the connection
         */
        client.prototype.setPersistentId = function (guid) {
            this._persistentId = guid;
            localStorage.setItem(this._server, this._persistentId);
        };
        /**
         * Call before calling open.
         * If set the server will use QoS for this connection
         */
        client.prototype.enableQoS = function () {
            this._qos = true;
        };
        /**
         * Call before calling open.
         * @param enabled - enabled auto heartbeat when connected (default true)
         * @param timeout - the time to wait before disconnecting the client
         */
        client.prototype.autoHeartbeat = function (enabled, timeout) {
            if (enabled === void 0) { enabled = true; }
            if (timeout === void 0) { timeout = 30000; }
            this._autoHeartbeat = enabled;
            this._autoHeartbeatTimeout = timeout;
        };
        /**
         * Opens the transport (socket) and setup all basic events (open, close, onmessage, onerror)
         */
        client.prototype.open = function () {
            var _this = this;
            var that = this;
            if (this._persistentId)
                this._parameters["persistentid"] = this._persistentId;
            if (this._qos)
                this._parameters["qos"] = '1';
            if (this.socket !== undefined && this.socket.readyState === WebSocket.OPEN)
                return;
            this.socket = new WebSocket(this._server + this.querystring(), [this.subprotocol]);
            this.socket.binaryType = "arraybuffer";
            this.socket.onopen = function (event) {
                _this._readyState = WebSocket.OPEN;
                _this.onOpen(event);
                // Open all controllers
                for (var c in _this._controllers) {
                    _this.sendtext(new xsockets.message(_this._controllers[c].name, xsockets.events.init));
                }
                if (_this._autoHeartbeat) {
                    _this._autoHeartbeatIntervalTimer = setInterval(function () {
                        if (that._pongReceived === false) {
                            that.close();
                            clearInterval(that._autoHeartbeatIntervalTimer);
                            return;
                        }
                        that.sendtext(new xsockets.message('', xsockets.events.ping));
                        that._pongReceived = false;
                    }, _this._autoHeartbeatTimeout);
                    that.sendtext(new xsockets.message('', xsockets.events.ping));
                    that._pongReceived = false;
                }
            };
            this.socket.onclose = function (event) {
                _this.socket = undefined;
                //Fire close if it was ever opened
                if (_this._readyState === WebSocket.OPEN) {
                    _this.onClose(event);
                    //Close all controllers
                    for (var c in _this._controllers) {
                        _this._controllers[c].close();
                    }
                }
                _this._readyState = WebSocket.CLOSED;
                clearInterval(_this._autoHeartbeatIntervalTimer);
                //clearTimeout(this._autoHeartbeatTimeoutTimer);
                if (_this._autoReconnect) {
                    setTimeout(function () { that.open(); }, _this._autoReconnectTimeout);
                }
                ;
            };
            this.socket.onmessage = function (event) {
                if (typeof event.data === "string") {
                    // TextMessage                
                    var d = JSON.parse(event.data);
                    var m = new xsockets.message(d.C, d.T, d.D, undefined);
                    m.Q = d.Q;
                    m.I = d.I;
                    if (m.T === xsockets.events.ping) {
                        m.T = xsockets.events.pong;
                        _this.sendtext(m);
                        return;
                    }
                    if (m.T === xsockets.events.pong) {
                        _this._pongReceived = true;
                    }
                    if (m.T === xsockets.events.open) {
                        _this.setPersistentId(JSON.parse(m.D).PI);
                    }
                    if (m.T === xsockets.events.error) {
                        _this.onError(d);
                        return;
                    }
                    if (m.T === xsockets.events.authfailed) {
                        _this.onAuthenticationFailed(m.D);
                        _this.close(false);
                        return;
                    }
                    var c = _this.controller(m.C, false); // as xsockets.controller;
                    if (c == undefined) {
                        _this.onMessage(d);
                    }
                    else {
                        c.dispatchEvent(m);
                    }
                }
                else if (typeof (event.data) === "object") {
                    // BinaryMessage                
                    var bm = new xsockets.message("", "", "", event.data);
                    var bd = bm.extractMessage();
                    var c = _this.controller(bd.C, false); // as xsockets.controller;
                    if (c == undefined) {
                        _this.onMessage(event.data);
                    }
                    else {
                        c.dispatchEvent(bd);
                    }
                }
            };
            this.socket.onerror = function (event) {
                _this.onError(event);
            };
        };
        /**
         * Close the transport (socket)
         * @param autoReconnect - if true the transport will try to reconnect, default = false
         */
        client.prototype.close = function (autoReconnect) {
            if (autoReconnect === void 0) { autoReconnect = false; }
            this._autoReconnect = autoReconnect;
            if (this.socket != undefined)
                this.socket.close();
        };
        /**
         * Returns the instance of a specific controller
         * @param name - the name of the controller to fetch
         * @param createNewInstanceIfMissing - if true a new instance will be created, default = true
         */
        client.prototype.controller = function (name, createNewInstanceIfMissing) {
            if (createNewInstanceIfMissing === void 0) { createNewInstanceIfMissing = true; }
            var instance = undefined;
            for (var c in this._controllers) {
                if (this._controllers[c].name === name.toLowerCase()) {
                    instance = this._controllers[c];
                    break;
                }
            }
            if (instance === undefined && createNewInstanceIfMissing) {
                instance = new xsockets.controller(this, name.toLocaleLowerCase());
                this._controllers.push(instance);
                instance.open();
            }
            return instance;
        };
        /**
         * Removes a controller from the transport
         * @param controller - controller instance to dispose
         */
        client.prototype.disposeController = function (controller) {
            var index = this._controllers.indexOf(controller, 0);
            if (index > -1) {
                this._controllers.splice(index, 1);
            }
        };
        client.prototype.sendtext = function (data) {
            if (this.isConnected()) {
                this.socket.send(data);
            }
        };
        /**
         * Returns true if the socket is open
         */
        client.prototype.isConnected = function () {
            return this.socket !== undefined && this.socket.readyState === WebSocket.OPEN;
        };
        client.prototype.querystring = function () {
            var str = "?";
            for (var key in this._parameters) {
                str += key + "=" + encodeURIComponent(this._parameters[key]) + "&";
            }
            str = str.slice(0, str.length - 1);
            return str;
        };
        return client;
    }());
    xsockets.client = client;
})(xsockets || (xsockets = {}));
//# sourceMappingURL=xsockets.latest.js.map