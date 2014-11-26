using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;

namespace GameSample
{
    /// <summary>
    /// Simple game controller - Enables the multiplayer functinallity 
    /// Look at the GameExtensions.cs where you find extensions that filters opponents..
    /// </summary>
    public class GameController : XSocketController
    {
        public Player Player { get; set; }
        public override void OnClosed()
        {
            // Someone gave up, tell others that an opponent is lost... 
            this.InvokeToOthers(new { this.ConnectionId }, "lostOpponent");
        }
        public void Update(double a, double v, int x, int y)
        {
            this.Player.x = x;
            this.Player.y = y;
            this.Player.a = a;
            this.Player.v = v;
            this.InvokeToOthers(new { a = this.Player.a, v = this.Player.v, p = this.ConnectionId }, "move");
        }
        public void Fire(double a, double v)
        {
            // Thell the others i fired the cannon ;-)
            this.InvokeTo(this.OpponentConnections(), new { a = a, v = v, p = this.ConnectionId }, "fire");
        }

        public void Hit(Guid key)
        {
            this.Player.Score ++;
            this.InvokeTo(this.OpponentConnections(), new { key, opponent = this.ConnectionId }, "hit");
            this.InvokeTo(o => o.ConnectionId == key, this.Player,"gameover");
            this.Find(p => p.ConnectionId == key).Single().Player.IsReady = false;
            // Pass back the current stats (score etc...)
            this.Invoke(this.Player, "updateScore");

        }

        public void Respawn()
        {
            var random = new Random();

            this.Player.x = random.Next(100, 1100);
            this.Player.y = random.Next(100, 500);
            this.Player.v = 0;
            this.Player.a = 0;
            

            this.Player.IsReady = true;
            this.Player.Respawns++;

            // Restart the game
            this.Invoke(this.Player, "start");
            // Who is still alive?
            this.Invoke(this.Opponents(), "opponents");

            // Notify others you respawned
            this.InvokeToOthers(new List<Player> { this.Player }, "opponents");   

        }

        public void Start()
        {
            var random = new Random();
            this.Player.x = random.Next(100, 1100);
            this.Player.y = random.Next(100, 500);
            this.Player.IsReady = true;

            // Start the game
            this.Invoke(this.Player, "start"); 
            // Get the opponents
            this.Invoke(this.Opponents(), "opponents");
            // Tell others i joind the game
            this.InvokeToOthers(new List<Player> { this.Player }, "opponents");
        }

        public override void OnOpened()
        {
            var random = new Random();
            this.Player  = new Player
            {
                x = random.Next(100, 500),
                y = random.Next(100, 300),
                a = 0,
                v = 0,
                ConnectionId = this.ConnectionId,
                Respawns= 0,
                IsReady =  false
            };
        }
    }

    public class Player
    {
        public bool IsReady { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public double a { get; set; }
        public double v { get; set; }
        public int Respawns { get; set; }
        public int Score { get; set; }
        public Guid ConnectionId { get; set; }
    }
}
