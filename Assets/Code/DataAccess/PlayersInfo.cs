using System.Collections.Generic;
using Code.Hexagonal;

namespace Code.DataAccess {
    public static class PlayersInfo {
        
        public static int[] DeadPlayersId = {};
        public static int Winner = 0;
        private static Dictionary<int, HexCell> PlayersCapitals = new Dictionary<int, HexCell>();

        public static void SetPlayerCapital(int playerId, HexCell capital) {
            PlayersCapitals.Add(playerId, capital);
        }

        public static HexCell GetPlayerCapital(int playerId) {
            return PlayersCapitals.ContainsKey(playerId) ? PlayersCapitals[playerId] : null;
        }
        
    }
}