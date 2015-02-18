
var AudioBufferChunksPlayer = (function () {
    var ctor = function () {
        var activeBuffer;
        var numOfChunks = 0;
        var context;
        var audioBuffer;
        var audioSource;
        var appendBuffer = function (buffer1, buffer2) {
            var tmp = new Uint8Array(buffer1.byteLength + buffer2.byteLength);
            tmp.set(new Uint8Array(buffer1), 0);
            tmp.set(new Uint8Array(buffer2), buffer1.byteLength);
            return tmp.buffer;
        };
        var initializeWebAudio = function () {
            context = new AudioContext();
        };

        var play = function () {
            var scheduledTime = 0.015;
            try {
                audioSource.stop(scheduledTime);
            } catch (e) { }
            audioSource = context.createBufferSource();
            audioSource.buffer = audioBuffer;
            audioSource.connect(context.destination);
            var currentTime = context.currentTime + 0.010 || 0;
            audioSource.start(scheduledTime - 0.005, currentTime, audioBuffer.duration - currentTime);
            audioSource.playbackRate.value = 1;
        };


        this.addChunk= function (buffer) {
            if (numOfChunks === 0) {
                initializeWebAudio();
                activeBuffer = buffer;
            } else {
                activeBuffer = appendBuffer(activeBuffer, buffer);
            }
            context.decodeAudioData(activeBuffer, function (buf) {
                audioBuffer = buf;
                play();
            });
            numOfChunks++;
        };

    };
    return ctor;
})();