/////////////////////////////////////////////////////
// ANIMAL SERVCIE
//
// We expect 'animal' to a controller on the server.
/////////////////////////////////////////////////////
var AnimalService = (function () {
    var service = function (url, controllers) {
        var socket = new XSockets.WebSocket(url || 'ws://'+location.host, controllers || ['animal']);

        this.AnimalController = socket.controller('animal');
    }
    return service;
})();