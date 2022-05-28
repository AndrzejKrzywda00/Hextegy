using System;
using System.Collections.Generic;
using System.IO;
using Code.Generator.Util;
using UnityEngine;


namespace Code.Generator {
    public class GridGenerator {
        private static MapGrid map;

        public static Cell[] GenerateMap(int height, int width) {
            map = new MapGrid(width, height);
            
            GenerateByPerlinNoise();
            ClearUnconnectedIslands();
            AddPlayersFields(new[] {1, 1, 2, 3, 4}, 2);
            GenerateTrees(0.2f);

            return map.cells;
        }
        

        private static void GenerateByPerlinNoise() {
            bool[,] perlinNoise = PerlinNoise.Generate(map.height, map.width, 5, 0.4f);

            for (int x = 0; x < map.width; x++) {
                for (int y = 0; y < map.height; y++) {
                    Cell newCell = new Cell(x, y);
                    map.setCell(perlinNoise[x, y] ? newCell : null);
                }
            }
        }

        private static void ClearUnconnectedIslands() {
            Cell center = map.getClosestCell(map.getCenterCoordinates());

            SetMainLandCellsRecursively(center.coordinates);

            foreach (Cell cell in map.cells) {
                if (cell != null) {
                    if (cell.isMainLand == false) {
                        map.clearCell(cell.coordinates);
                    }
                }
            }
        }

        private static void SetMainLandCellsRecursively(Coordinates startingPoint) {
            Coordinates[] touchingCoordinates = Coordinates.getTouchingCoordinates(startingPoint);
            foreach (Coordinates coordinates in touchingCoordinates) {
                if(map.isOutOfRange(coordinates)) continue;
                if(map.getCell(coordinates) == null) continue;
                if (!map.getCell(coordinates).isMainLand) {
                    map.getCell(coordinates).isMainLand = true;
                    SetMainLandCellsRecursively(coordinates);
                }
            }
        }

        private static void AddPlayersFields(int[] playersIds, int radius) {
            foreach (int playerId in playersIds) {
                Cell startingCell = map.getRandomCell();
                foreach (Cell cell in map.getCircle(startingCell.coordinates, radius)) {
                    cell.PlayerId = playerId;
                }
                startingCell.Prefab = Prefabs.getCapital(playerId);
            }
        }

        private static void GenerateTrees(float ratio) {
            foreach (Cell cell in map.cells) {
                if (cell != null && RandomNumber.getBoolByRatio(ratio)) {
                    if (cell.Prefab == Prefabs.getNoElement()) {
                        cell.Prefab = Prefabs.getTree();
                    }
                }
            }
        }
    }
}