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

    var host = location.hostname;
    var port = location.port === "" ? "80" : location.port;
    var wsUrl = host + ":" + port;

    ws = new XSockets.WebSocket("ws://" + wsUrl, ["game"]);
    gameEngine = new Noc.Engine("#surface", document.querySelector("selection"));
    audio = new Noc.Audio();
    gameController = ws.controller("game");
    // load some sprites, blue & red starships.
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
        var ray = new Ray(data.a, 1, o.x, o.y);
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
            if (Math.round(gameEngine.timeElapsed) % 3 === 0) return;
            gameController.invoke("update", {
                a: state.angle,
                v: state.speed,
                x: state.x,
                y: state.y
            });
        };
        player.changeAngle = function () {
            this.state.angle = gameInput.a;
            gameController.invoke("move", { a: this.state.angle, v: this.state.speed });

        };
        player.speedUp = function () {
            if (this.state.speed > 3) return;
            this.state.speed += 1;
            gameController.invoke("move", { a: this.state.angle, v: this.state.speed });

        };
        player.slowDown = function () {
            if (this.state.speed < -3) return;
            this.state.speed -= 1;
            gameController.invoke("move", { a: this.state.angle, v: this.state.speed });

        };
        player.fire = function () {
            var ray = new Ray(this.state.angle, 1, this.state.x, this.state.y);

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