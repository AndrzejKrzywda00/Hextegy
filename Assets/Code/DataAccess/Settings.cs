using System.Collections.Generic;
using UnityEngine;

namespace Code.DataAccess {
    public static class Settings {
        public static int NumberOfPlayers = 2;
        public static int MapSize = 20;
        public static float Scale = 3f;
        public static float Fulfill = 0.5f;
        public static float TreeRatio = 0.2f;

        public static List<int> AlivePlayersId = new List<int>() {1, 2};
        public static int Winner = 2;
    }
}