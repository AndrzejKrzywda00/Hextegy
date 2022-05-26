using System;

namespace Code.Generator {
    public class Coordinates {
        public int x;
        public int y;

        public Coordinates(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public String toString() {
            return "x: " + x + " " + "y: " + y;
        }
        
        
        public static int Distance(Coordinates c1, Coordinates c2) {
            Coordinates a1 = OddToAxial(c1);
            Coordinates a2 = OddToAxial(c2);
            return AxialDistance(a1, a2);
        }

        private static int AxialDistance(Coordinates a, Coordinates b) {
            return (Math.Abs(a.y - b.y)
                    + Math.Abs(a.y + a.x - b.y - b.x)
                    + Math.Abs(a.x - b.x)) / 2;
        }

        private static Coordinates OddToAxial(Coordinates point) {
            var q = point.y - (point.x - (point.x & 1)) / 2;
            var r = point.x;
            return new Coordinates(q, r);
        }
    }
}