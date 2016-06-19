/////////////////////////////////////////////////////
// ANIMAL SERVCIE
//
// We expect 'animal' to a controller on the server.
/////////////////////////////////////////////////////
var AnimalService = (function () {
    var service = function (url, controllers) {
        var socket = new xsockets.client(url || 'ws://127.0.0.1:4502', controllers || ['animal']);
        socket.setPersistentId(xsockets.utils.guid());

        this.AnimalController = socket.controller('animal');
        socket.open();
    }
    return service;
})();