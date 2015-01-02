var FileResult = (function () {
    var fileResult = function (shareId, filename, size, buffer) {
        this.shareId = shareId;
        this.fileName = filename;
        this.size = size;
        this.buffer = new ArrayBuffer();
        this.appendChunk(buffer);
    };
    fileResult.prototype.appendChunk = function (arr) {
        var tmp = new Uint8Array(this.buffer.byteLength + arr.byteLength);
        tmp.set(new Uint8Array(this.buffer), 0);
        tmp.set(new Uint8Array(arr), this.buffer.byteLength);
        this.buffer = tmp.buffer;
        return this;
    }
    return fileResult;
})();
var FileShare = (function () {
    var buffers = {};
    var fileshare = function () {
    };
    fileshare.prototype.receive = function (result) {
        var self = this;
        if (buffers.hasOwnProperty(result.data.shareId)) {
            buffers[result.data.shareId].appendChunk(result.binary);
            this.onreceive(result.data.shareId, buffers[result.data.shareId].buffer.byteLength);


        } else {
            buffers[result.data.shareId] = new FileResult(result.data.shareId,
               result.data.name, result.data.size, result.binary);
            self.onstarted(result.data);
        }


        if (result.data.fin) {

            self.oncomplete(result.data.shareId);
        }


    };
    fileshare.prototype.oncomplete = function () {

    };
    fileshare.prototype.onreceive = function (f) {

    };
    fileshare.prototype.onstarted = function () {
    }
    fileshare.prototype.getFileShares = function () {
        return buffers;
    }
    fileshare.prototype.getFileShareById = function (shareId) {
        return buffers[shareId];
    }
    return fileshare;
})();