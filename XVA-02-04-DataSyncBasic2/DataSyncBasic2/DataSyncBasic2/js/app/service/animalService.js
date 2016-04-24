/////////////////////////////////////////////////////
// ANIMAL SERVCIE
//
// We expect 'animal' to a controller on the server.
/////////////////////////////////////////////////////
var AnimalService = (function () {
    var service = function (url, controllers) {
        var socket = new XSockets.WebSocket(url || 'ws://127.0.0.1:4502', controllers || ['animal'], {persistentid:XSockets.Utils.guid()});

        this.AnimalController = socket.controller('animal');
    }
    return service;
})();