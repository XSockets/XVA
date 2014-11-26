using System.Collections.Generic;
using System.Linq;
using XSockets.Core.XSocket.Helpers;

namespace GameSample
{
    public static class GameExtensions
    {
        public static IEnumerable<Player> Opponents(this GameController controller)
        {
            return controller.OpponentConnections().Select(s => s.Player);
        }

        public static IEnumerable<GameController> OpponentConnections(this GameController controller)
        {
            return controller.Find(p => p.Player.IsReady && p.ConnectionId != controller.ConnectionId);
        }
    }
}