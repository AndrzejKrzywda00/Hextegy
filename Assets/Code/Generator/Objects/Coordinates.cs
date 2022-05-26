using System;
using System.Linq;
using Code.Generator.Util;

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
            return new[] {
                Math.Abs(c2.y - c1.y),
                Math.Abs(((int) Math.Ceiling(c2.y / -2f)) + c2.x - ((int) Math.Ceiling(c1.y / -2f)) - c1.x),
                Math.Abs(-c2.y - ((int) Math.Ceiling(c2.y / -2f)) - c2.x + c1.y + ((int) Math.Ceiling(c1.y / -2f)) +
                         c1.x)
            }.Max();
        }

        public static Coordinates Random(int width, int height) {
            int x = RandomNumber.getInt(0, width);
            int y = RandomNumber.getInt(0, height);
            return new Coordinates(x, y);
        }
    }
}