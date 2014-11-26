var audio;
var gameEngine, ws, gameController;
var assets;
var starfield;
var ap, p = [];
var gameInput = {
    a: 0,
    p: [100, 100],
    ap: [-10, -1]
};
var Player = function (type, id, sprite, data) {
    var player = new Noc.Entity(id, function (vp) {
        var ctx = vp.ctx;
        var radians = this.state.angle * Math.PI / 180;
        var rotation = (this.state.angle + 90) * Math.PI / 180;
        this.state.x += Math.cos(radians) * this.state.speed;
        this.state.y += Math.sin(radians) * this.state.speed;
        var w = this.state.sprite.width / 2;
        var h = this.state.sprite.height / 2;
        if (this.state.x <= w) {
            this.state.speed = -this.state.speed;
        } else if (this.state.x >= vp.width - w) {
            this.state.speed = -this.state.speed;
        };
        if (this.state.y <= h) {
            this.state.speed = -this.state.speed;
        } else if (this.state.y >= vp.height - h) {
            this.state.speed = -this.state.speed;
        };
        gameInput.p[0] = this.state.x;
        gameInput.p[1] = this.state.y;
        gameInput.ap = [(gameInput.p[0] - (vp.width / 2)) / 25, (gameInput.p[1] - (vp.height / 2)) / 25];
        ctx.save();
        ctx.translate(this.state.x, this.state.y);
        ctx.rotate(rotation);
        ctx.translate(-this.state.sprite.width / 2, -this.state.sprite.height / 2);
        ctx.drawImage(this.state.sprite, 0, 0);
        ctx.restore();
    }).init({
        sprite: sprite,
        x: data.x,
        y: data.y,
        speed: data.v,
        angle: data.a,
    });
    player.type = type;
    player.detectCollitions([], function (state) {
        return {
            x: state.x,
            y: state.y,
            r: player.type === "enemy" ? this.state.sprite.width / 2 : 0
        }
    });
    player.onCollition = function (a) {
        gameEngine.removeEntity(a);
        gameController.invoke("hit", {
            key: a
        });
    };
    return player;
};
var Ray = function (angle, speed, x, y) {
    var rayLifteime = 2000;
    var ray = new Noc.Entity(XSockets.Utils.randomString(5), function (surface) {
        var ctx = surface.ctx;
        this.state.x += Math.cos(this.state.rad) * this.state.speed;
        this.state.y += Math.sin(this.state.rad) * this.state.speed;
        ctx.beginPath();
        ctx.arc(this.state.x, this.state.y, this.state.radius, 0, 6.3);
        ctx.fillStyle = '#f00';
        ctx.fill();
    }, 0, rayLifteime).init((function (cx, cy, a, r) {
        var radians = a * Math.PI / 180;
        cx = cx + 30 * Math.cos(radians);
        cy = cy + 30 * Math.sin(radians);
        return {
            rad: radians,
            x: cx,
            y: cy,
            angle: a,
            speed: 5,
            radius: r
        }
    })(x, y, angle, 5));
    ray.type = "ray";
    ray.onlifetimeend = function (e) {
        gameEngine.removeEntity(e);
    };
    ray.detectCollitions(["enemy"], function (state) {
        return {
            x: state.x,
            y: state.y,
            r: state.radius / 3
        }
    });
    return ray;
}
var GameOver = (function () {
    var gameover = new Noc.Entity("gameover", function (vp) {
        var ctx = vp.ctx;
        var img = this.state.images[this.state.index];
        var w = img.width;
        var h = img.height;
        var x = (vp.width / 2) - w;
        var y = (vp.height / 2) - h;
        ctx.save();
        ctx.globalAlpha = this.state.fadePct / 100;
        ctx.drawImage(img, x, y, w * 2, h * 2);
        ctx.restore();
        if (this.state.fadePct >= 100 && this.state.index < this.state.images.length - 1) {
            this.state.fadePct = 0;
            this.state.index++;
        }
        this.state.fadePct += 0.5;
    }, 0, 5000).init({
        fadePct: 0,
        index: 0,
        images: [
        assets.getImage("/assets/images/game over.png")],
    });
    gameover.onlifetimeend = function (e) {
        gameEngine.removeEntity(e);
        gameController.invoke("respawn");
    }
    return gameover;
});
var startGame = function (evt) {
    switch (evt.charCode || evt.keyCode) {
        case 32:
            document.removeEventListener("keydown", startGame);
            gameEngine.removeEntity("intro");
            gameController.invoke("start");
            break;;
        default:
            break;
    };
};
$(function () {
    ws = new XSockets.WebSocket("ws://192.168.1.3:4502", ["game"]);
    gameEngine = new Noc.Engine("#surface", document.querySelector("selection"));
    audio = new Noc.Audio();
    gameController = ws.controller("game");
    // load some sprites, blue & red start ships.
    assets = new Noc.Assets({
        images: ['/assets/sprites/blue_ship.png', '/assets/sprites/red_ship.png', '/assets/images/xfighter.png', '/assets/images/xsockets.png', '/assets/images/presents.png', '/assets/images/game over.png', '/assets/images/press space.png']
    });
    audio.completed = function (key) {
        gameEngine.addEntity(launchGame);
        if (key === "music") audio.play("music", function (source) {
            source.loop = true;
        });
    };
    audio.load("music", "/assets/audio/CZ Tunes - Intro (Jeroen Tel) (extract).mp3");



    gameController.on("move", function (data) {
        gameEngine.entities[data.p].state.speed = data.v;
        gameEngine.entities[data.p].state.angle = data.a;
    });
    gameController.on("fire", function (data) {
        var o = gameEngine.entities[data.p].state;
        var ray = new Ray(data.a, 5, o.x, o.y);
        gameEngine.addEntity(ray);
    });
    gameController.on("hit", function (data) {
        gameEngine.removeEntity(data.key);
    });
    gameController.on("gameover", function () {
        gameEngine.removeEntity("friend", function () {
            gameEngine.addEntity(new GameOver());
        });
    });
    gameController.on("updateScore", function (stats) {
        document.querySelector("#score").innerText = stats.Score;
        document.querySelector("#respawns").innerText = stats.Respawns;
    });
    gameController.on("lostopponent", function (opponent) {
        gameEngine.removeEntity(opponent.ConnectionId);
    });
    // game is started, create a player, attach listeners for keys
    gameController.on("start", function (data) {
        gameInput.ap = [data.x, data.y];
        $(document).unbind("keydown");
        var player = new Player("friend", data.ConnectionId, assets.getImage("/assets/sprites/blue_ship.png"), data);
        player.onRender = function (state) {
            gameController.invoke("update", {
                a: state.angle,
                v: state.speed,
                x: state.x,
                y: state.y
            });
        };
        player.changeAngle = function () {
            this.state.angle = gameInput.a;
         
        };
        player.speedUp = function () {
            if (this.state.speed > 5) return;
            this.state.speed += 1;
         
        };
        player.slowDown = function () {
            if (this.state.speed < -5) return;
            this.state.speed -= 1;
          
        };
        player.fire = function () {
            var ray = new Ray(this.state.angle, 5, this.state.x, this.state.y);
            gameEngine.addEntity(ray);
            gameController.invoke("fire", {
                a: this.state.angle,
                v: 7
            });
        };
        // take control over self (starship)
        $(document).bind("keydown", function (evt) {
            switch (evt.charCode || evt.keyCode) {
                case 37:
                    gameInput.a -= (Math.PI / 3) + 10;
                    player.changeAngle();
                    break;
                case 39:
                    gameInput.a += (Math.PI / 3) + 10;
                    player.changeAngle();
                    break;
                case 65:
                    player.speedUp();
                    break;
                case 90:
                    player.slowDown();
                    break;
                case 32:
                    player.fire();
                    break;
            }
        });
        // add the entities to the surface
        gameEngine.addEntity(player);
    });

    gameController.on("opponents", function (opponents) {
        opponents.forEach(function (data) {
            var oppnent = new Player("enemy", data.ConnectionId, assets.getImage("/assets/sprites/red_ship.png"), data);
            gameEngine.addEntity(oppnent);
        });
    });

    // Create the starfield

    starfield = new Noc.Entity("startfield", function (surface) {
        var ctx = surface.ctx;
        ctx.fillStyle = "#FF0000";
        for (var s in this.state.stars) {
            ctx.fillStyle = Noc.Utils.blendColors("#FFFFFF", "#000000", 1 - this.state.stars[s][2] / 10 * 2);
            ctx.fillRect(this.state.stars[s][0], this.state.stars[s][1], this.state.stars[s][2], this.state.stars[s][2]);
        }
        for (var i in this.state.stars) {
            this.state.stars[i][0] += gameInput.ap[0] * this.state.stars[i][2] / 10;
            this.state.stars[i][1] += gameInput.ap[1] * this.state.stars[i][2] / 10;
            if (this.state.stars[i][0] >= surface.width) {
                this.state.stars[i][0] = -5;
            }
            if (this.state.stars[i][1] >= surface.height) {
                this.state.stars[i][1] = -5;
            }
            if (this.state.stars[i][0] < -6) {
                this.state.stars[i][0] = surface.width;
            }
            if (this.state.stars[i][1] < -6) {
                this.state.stars[i][1] = surface.height;
            }
        }
    }).init({
        stars: (function (w, h) {
            var stars = {};
            var totalStars = (Math.floor(w / 72)) * (Math.floor(h / 72)) * 1;
            var randomX, randomY, randomZ;
            var sortable = [];
            for (var i = 0; i < totalStars; i++) {
                randomX = Math.random() * (w - 1) + 1;
                randomY = Math.random() * (h - 1) + 1;
                randomZ = Math.random() * 5;
                stars[i] = [randomX, randomY, randomZ];
                sortable.push(randomZ);
            }
            sortable.sort();
            for (var s in stars) {
                stars[s][2] = sortable[s];
            }
            return stars;
        })(gameEngine.viewport.width, gameEngine.viewport.height)
    });


    // Create an into for the game....
    var launchGame = new Noc.Entity("intro", function (vp) {
        var ctx = vp.ctx;
        var img = this.state.images[this.state.index];
        var w = img.width;
        var h = img.height;
        var x = (vp.width / 2) - w;
        var y = (vp.height / 2) - h;
        ctx.save();
        ctx.globalAlpha = this.state.fadePct / 100;
        ctx.drawImage(img, x, y, w * 2, h * 2);
        ctx.restore();
        if (this.state.fadePct >= 100 && this.state.index < this.state.images.length - 1) {
            this.state.fadePct = 0;
            this.state.index++;
        }
        this.state.fadePct += 0.5;
    }, 0, 22000).init({
        fadePct: 0,
        index: 0,
        images: [
        assets.getImage("/assets/images/xsockets.png"), assets.getImage("/assets/images/presents.png"), assets.getImage("/assets/images/xfighter.png"), assets.getImage("/assets/images/press space.png")],
    });
    launchGame.onlifetimeend = function (e) {
        gameEngine.removeEntity(e);
        gameController.invoke("respawn");
    }
    // wait for game to be started?
    document.addEventListener("keydown", startGame);



    gameEngine.addEntity(starfield);
    gameEngine.addEntity(launchGame);

    gameEngine.start(true);
});