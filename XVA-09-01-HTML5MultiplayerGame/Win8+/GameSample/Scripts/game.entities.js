var Player = function (type, id, sprite, data) {
    var player = new Noc.Entity(id, function (vp) {
        var ctx = vp.ctx;
        var radians = this.state.angle * Math.PI / 180;
        var rotation = (this.state.angle + 90) * Math.PI / 180;
        this.state.x += Math.cos(radians) * this.state.speed;
        this.state.y += Math.sin(radians) * this.state.speed;
        var w = this.state.sprite.width / 2;
        var h = this.state.sprite.height / 2;

        if (this.state.x < w || this.state.x > vp.width - w) {
            this.state.angle = 180 - this.state.angle;
        }

        if (this.state.y < h || this.state.y > vp.height - h) {
            this.state.angle = 360 - this.state.angle;
        }
        gameInput.p[0] = this.state.x;
        gameInput.p[1] = this.state.y;
        gameInput.a = this.state.angle;

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