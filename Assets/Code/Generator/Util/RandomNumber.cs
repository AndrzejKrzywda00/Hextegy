using UnityEngine;
using Random = System.Random;

namespace Code.Generator.Util {
    public class RandomNumber {
        private static Random rand = new Random();
        
        public static int getInt() {
            return rand.Next();
        }
        
        public static int getInt(int min, int max) {
            return rand.Next(min, max);
        }

        public static bool getBoolByRatio(float ration) {
            int prob = (int)(100 * ration);
            return getInt(0, 100) < prob;
        }
    }
}