using Random = System.Random;

namespace Code.Generator.Util {
    public abstract class RandomNumber {
        
        private static readonly Random Rand = new Random();
        
        public static int GetInt() {
            return Rand.Next();
        }
        
        public static int GetInt(int min, int max) {
            return Rand.Next(min, max);
        }

        public static bool GetBoolByRatio(float ration) {
            int prob = (int)(100 * ration);
            return GetInt(0, 100) < prob;
        }
    }
}