using Code.Generator;
using UnityEngine;

namespace Code.Generator {
    public class MapGenerator {
        

        public Cell[] GenerateMap(int height, int width) {
            MapGrid map = GridGenerator.GenerateGrid(height, width);

            return map.cells;
        }


    }
}
