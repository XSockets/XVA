window.requestAnimFrame = (function () {
    return window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame ||
    function (callback) {
        window.setTimeout(callback, 1000 / 60);
    };
})();
var Noc = {
    Timer: {},
    Utils: {},
    Collision: {}
};
// just a simple circular collition detector
Noc.Collision.circularDetection = function (a, b) {
    var dx = a.x - b.x;
    var dy = a.y - b.y;
    var k = a.r + b.r;
    return (dx * dx + dy * dy <= k * k);
};
Noc.Audio = (function () {
    function audio() {
        var that = this;
        this.audioBuffers = {};
        this.keys = [];
        this.context = new webkitAudioContext();
        this.load = function (key, url) {
            this.keys.unshift(key);
            var request = new XMLHttpRequest();
            request.open('GET', url, true);
            request.responseType = 'arraybuffer';
            request.onload = function () {
                that.context.decodeAudioData(request.response, function (buffer) {
                    that.audioBuffers[key] = buffer;
                    if (that.completed) that.completed(key);
                }, function (err) {
                    console.log(err);
                });
            };
            request.send();
            return this;
        };
    }
    audio.prototype.completed = function () { };
    audio.prototype.play = function (key, cb) {
        var source = this.context.createBufferSource();
        source.buffer = this.audioBuffers[key];
        source.connect(this.context.destination);
        source.start();
        if (cb) {
            cb(source);
        }
        return this;
    };
    audio.prototype.pause = function () { };
    return audio;
})();

Noc.Utils = {
    blendColors: function (c0, c1, p) {
        var f = parseInt(c0.slice(1), 16),
            t = parseInt(c1.slice(1), 16),
            R1 = f >> 16,
            G1 = f >> 8 & 0x00FF,
            B1 = f & 0x0000FF,
            R2 = t >> 16,
            G2 = t >> 8 & 0x00FF,
            B2 = t & 0x0000FF;
        return "#" + (0x1000000 + (Math.round((R2 - R1) * p) + R1) * 0x10000 + (Math.round((G2 - G1) * p) + G1) * 0x100 + (Math.round((B2 - B1) * p) + B1)).toString(16).slice(1);
    }
};
Noc.Timer.Fps = (function () {
    var timer = function () {
        this.elapsed = 0;
        this.last = null;
    }
    timer.prototype.tick = function (now) {
        this.elapsed = (now - (this.last || now) / 1000);
        this.last = now;
    }
    timer.prototype.current = function () {
        return Math.round(1 / this.elapsed);
    };
    return timer;
})();
Noc.Sprite = (function () {
    function sprite(image, pos, size, speed, frames, dir, once) {
        this.image = image;
        this.pos = pos;
        this.size = size;
        this.speed = typeof speed === 'number' ? speed : 0;
        this.frames = frames;
        this._index = 0;
        this.dir = dir || 'horizontal';
        this.once = once;
    }
    sprite.prototype.update = function (dt) {
        this._index += this.speed * dt;
    };
    sprite.prototype.render = function (ctx) {
        var frame;
        if (this.speed > 0) {
            var max = this.frames.length;
            var idx = Math.floor(this._index);
            frame = this.frames[idx % max];
            if (this.once && idx >= max) {
                this.done = true;
                return;
            }
        }
        else {
            frame = 0;
        }
        var x = this.pos[0];
        var y = this.pos[1];
        if (this.dir == 'vertical') {
            y += frame * this.size[1];
        }
        else {
            x += frame * this.size[0];
        }
        ctx.drawImage(this.image, x, y, this.size[0], this.size[1], 0, 0, this.size[0], this.size[1]);
    };
    return sprite;
})();
Noc.Images = (function () {
    var images = function () {
        this.downloadQueue = [];
        this.successCount = 0;
        this.errorCount = 0;
        this.cache = {};
    };
    images.prototype.downloadAll = function (downloadCallback) {
        if (this.downloadQueue.length === 0) {
            downloadCallback();
        }
        for (var i = 0; i < this.downloadQueue.length; i++) {
            var path = this.downloadQueue[i];
            var img = new Image();
            var that = this;
            img.addEventListener("load", function () {
                that.successCount += 1;
                if (that.isDone()) {
                    downloadCallback();
                }
            }, false);
            img.addEventListener("error", function () {
                that.errorCount += 1;
                if (that.isDone()) {
                    downloadCallback();
                }
            }, false);
            img.src = path;
            this.cache[path] = img;
        }
    };
    images.prototype.getAsset = function (path) {
        return this.cache[path];
    };
    images.prototype.isDone = function () {
        return (this.downloadQueue.length == this.successCount + this.errorCount);
    };
    return images;
})();
Noc.Assets = (function () {
    var assets = function (resources, cb) {
        var self = this;
        this.imageAssets = new Noc.Images();
        this.audioAssets = new Noc.Audio();
        resources.images.forEach(function (image) {
            self.imageAssets.downloadQueue.push(image);
        });
        this.imageAssets.downloadAll(function () {
            if (cb) cb();
        });
    };
    assets.prototype.getImage = function (path) {
        return this.imageAssets.cache[path];
    };
    return assets;
})();
Noc.Engine = (function () {
    var engine = function (target, el) {
        this.parent = el;
        this.entities = {};
        var self = this;
        this.isReady = false;
        this.viewport = {
            canvas: undefined,
            height: 0,
            width: 0,
            ctx: undefined
        };
        this.viewport.canvas = document.querySelector(target);
        this.viewport.width = el ? el.clientWidth : window.innerWidth;
        this.viewport.height = el ? el.clientHeight : window.innerHeight;
        this.viewport.ctx = this.viewport.canvas.getContext("2d");
        this.viewport.mouse = undefined;
        this.clear = function () {
            self.viewport.ctx.clearRect(0, 0, this.viewport.width, this.viewport.height);
        };
        var lt = Date.now();
        var render = function (time) {
            self.clear();
            var now = Date.now();
            var dt = (now - lt) / 1000.0;
            Object.keys(self.entities).forEach(function (key) {
                var entity = self.entities[key]; // get the entity to render
                if (entity.onRender) entity.onRender(entity.state, key);
                entity.collidesWith.forEach(function (collKey) { // does this entity have any collitions?
                    var entities = [];
                    for (var _entity in self.entities) {
                        if (self.entities[_entity].type === collKey) entities.push(self.entities[_entity].key);
                    };
                    entities.forEach(function (k) {
                        var a = self.entities[k].collitionExpression(self.entities[k].state);
                        var b = entity.collitionExpression(entity.state);
                        var check = Noc.Collision.circularDetection(a, b);
                        if (check) {
                            self.entities[k].onCollition(self.entities[k].key, entity.key);
                        }
                    });
                });
                entity.draw(self.viewport, dt);
                if (entity.lifetime) {
                    if (time > entity.lifetime && entity.onlifetimeend) {
                        entity.active = false;
                        entity.onlifetimeend(entity.key, time);
                    }
                }
            });
            lt = now;
        };
        window.addEventListener('resize', function (evt) {
            if (!self.parent) {
                self.viewport.height = evt.target.innerHeight;
                self.viewport.width = evt.target.innerWidth;
            } else {
                self.viewport.height = self.parent.clientHeight;
                self.viewport.width = self.parent.clientWidth;
            }
            self.viewport.canvas.height = self.viewport.height;
            self.viewport.canvas.width = self.viewport.width;
        }, false);
        (function animloop(a) {
            requestAnimFrame(animloop);
            if (self.isReady) render(a);
            self.timeElapsed = a;
        })();
        this.resize = function () {
            var evt = document.createEvent('UIEvents');
            evt.initUIEvent('resize', true, false, window, 0);
            window.dispatchEvent(evt);
        };
        self.resize();
        this.viewport.canvas.addEventListener("mousemove", function (evt) {
            self.viewport.mouse = evt;
        });
    };
    engine.prototype.addEntity = function (entity, cb) {
        if (!this.entities.hasOwnProperty(entity.key)) this.entities[entity.key] = entity;
        this.entities[entity.key].lifetime += this.timeElapsed || 0;
        if (cb) cb();
        return this;
    };
    engine.prototype.start = function () {
        this.isReady = !this.isReady;
    };
    engine.prototype.removeEntity = function (key, cb) {
        if (this.entities.hasOwnProperty(key)) delete this.entities[key];
        if (cb) cb();
        return this;
    };
    return engine;
})();
Noc.Entity = (function () {
    var entity = function (key, fn, push, lifetime) {
        this.fps = new Noc.Timer.Fps();
        this.type = "";
        this.active = true;
        this.state = {};
        this.render = fn;
        this.key = key;
        this.visible = true;
        this.push = push | 0;
        this.lifetime = lifetime;
        this.onRender = undefined;
        this.toggle = function () {
            this.visible = !this.visible;
        };
        this.init = function (settings) {
            this.state = settings;
            return this;
        };
        this.onlifetimeend = undefined;
        this.collidesWith = [];
        this.collitionExpression = undefined;
        this.detectCollitions = function (entityTypes, expr) {
            this.collitionExpression = expr;
            this.collidesWith = entityTypes;
        };
        this.onCollition = undefined;
    };
    entity.prototype.draw = function (ctx, tm) {
        this.render(ctx, tm);
        this.fps.tick(tm);
        if (this.fps.current() % 2 === 1) if (this.capture) this.capture(this.state);
    };
    return entity;
})();