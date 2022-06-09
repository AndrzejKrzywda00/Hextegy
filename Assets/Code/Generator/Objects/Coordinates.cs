using System;
using System.Linq;
using Code.Generator.Util;

namespace Code.Generator.Objects {
    public class Coordinates {
        public readonly int X;
        public readonly int Y;

        public Coordinates(int x, int y) {
            X = x;
            Y = y;
        }

        public override String ToString() {
            return "x: " + X + " " + "y: " + Y;
        }

        public static int Distance(Coordinates c1, Coordinates c2) {
            return new[] {
                Math.Abs(c2.Y - c1.Y),
                Math.Abs(((int) Math.Ceiling(c2.Y / -2f)) + c2.X - ((int) Math.Ceiling(c1.Y / -2f)) - c1.X),
                Math.Abs(-c2.Y - ((int) Math.Ceiling(c2.Y / -2f)) - c2.X + c1.Y + ((int) Math.Ceiling(c1.Y / -2f)) +
                         c1.X)
            }.Max();
        }

        public static Coordinates Random(int width, int height) {
            int x = RandomNumber.GetInt(0, width);
            int y = RandomNumber.GetInt(0, height);
            return new Coordinates(x, y);
        }

        public static Coordinates[] GetTouchingCoordinates(Coordinates coordinates) {
            if (coordinates.Y % 2 == 0)
                return new[] {
                    new Coordinates(coordinates.X -1, coordinates.Y -1),
                    new Coordinates(coordinates.X - 1, coordinates.Y),
                    new Coordinates(coordinates.X -1, coordinates.Y + 1),
                    new Coordinates(coordinates.X, coordinates.Y +1),
                    new Coordinates(coordinates.X +1, coordinates.Y ),
                    new Coordinates(coordinates.X , coordinates.Y - 1)
                };
            else
                return
                    new[] {
                        new Coordinates(coordinates.X , coordinates.Y-1),
                        new Coordinates(coordinates.X - 1, coordinates.Y),
                        new Coordinates(coordinates.X, coordinates.Y + 1),
                        new Coordinates(coordinates.X +1, coordinates.Y +1),
                        new Coordinates(coordinates.X+1, coordinates.Y ),
                        new Coordinates(coordinates.X + 1, coordinates.Y - 1)
                    };
        }
    }
}