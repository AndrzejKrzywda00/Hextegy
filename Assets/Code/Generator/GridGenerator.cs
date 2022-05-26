using System.Collections.Generic;
using UnityEngine;

namespace Code.Generator {
    public class GridGenerator {
        public static MapGrid GenerateGrid(int height, int width) {
            MapGrid map = new MapGrid(width, height);
            
            
            bool[,] perlinNoise = PerlinNoise.Generate(height, width, 5, 0.4f);
        
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Cell newCell = new Cell(x, y);
                    map.setCell(perlinNoise[x, y] ? newCell : null);
                }
            }


            List<Cell> cells = map.getRing(new Coordinates(10, 10), 5);
            //map.getCell(new Coordinates(10, 10)).PlayerId = 3;
            foreach (Cell c in cells) {
                map.getCell(c.coordinates).PlayerId = 1;
            }


            return map;
        }
        
    }
}