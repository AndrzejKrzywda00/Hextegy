using System.Collections.Generic;
using Code.Hexagonal;

namespace Code.DataAccess {
    public static class PlayersInfo {
        
        public static int[] DeadPlayersIds = {};
        public static int Winner = 0;
        private static readonly Dictionary<int, HexCell> PlayersCapitals = new Dictionary<int, HexCell>();

        public static void SetPlayerCapital(int playerId, HexCell capital) {
            if (!PlayersCapitals.ContainsKey(playerId)) PlayersCapitals.Add(playerId, capital);
        }

        public static HexCell GetPlayerCapital(int playerId) {
            return PlayersCapitals.ContainsKey(playerId) ? PlayersCapitals[playerId] : null;
        }
        
    }
}