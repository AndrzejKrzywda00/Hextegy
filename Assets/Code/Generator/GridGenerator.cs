using System;
using System.Collections.Generic;
using System.ComponentModel;
using Code.Generator.Util;
using UnityEngine;

namespace Code.Generator {
    public class GridGenerator {
        
        private MapGrid map;

        public float scale = 3f;                                            // good result in range [0.5, 10]
        public float fulfil = 0.5f;                                         // [0, 1]: 0 - empty map, 1 - full map
        public float treeRatio = 0.2f;                                      // [0, 1]: 0 - no trees, 1 - trees everywhere
        public List<PlayerField> playerFields = new List<PlayerField>();    // list of fields, one player id can occur many times

        
        public class PlayerField {
            public int playerId;        // player id
            public int radius;          // radius of field

            public PlayerField(int playerId, int radius) {
                this.playerId = playerId;
                this.radius = radius;
            }
        }
        
        public Cell[] GenerateMap(int height, int width) {
            map = new MapGrid(width, height);
            
            //Debug.Log("Generating perlin noise...");
            GenerateByPerlinNoise();
            
            //Debug.Log("Clearing unconnected islands...");
            ClearUnconnectedIslands();
            
            //Debug.Log("Generating players fields...");
            AddPlayersFields(playerFields);
            
            //Debug.Log("Generating trees...");
            GenerateTrees();

            //Debug.Log("Generation finished!");
            return map.cells;
        }

        public void GeneratePlayerFields(int numberOfPlayers, int radius) {
            playerFields = new List<PlayerField>();
            for (int playerId = 1; playerId <= numberOfPlayers; playerId++) {
                playerFields.Add(new PlayerField(playerId, radius));
            }
        }
        

        private void GenerateByPerlinNoise() {
            bool[,] perlinNoise = PerlinNoise.Generate(map.height, map.width, scale, fulfil);

            for (int x = 0; x < map.width; x++) {
                for (int y = 0; y < map.height; y++) {
                    Cell newCell = new Cell(x, y);
                    map.SetCell(perlinNoise[x, y] ? newCell : null);
                }
            }

            if (map.NumberOfNoEmptyCells() < map.width * map.height * 0.5 * fulfil + 1)
                throw new Exception(
                    "Map generator error: for this parameters generated map is mostly empty. Try one more time");
        }

        private void ClearUnconnectedIslands() {
            Cell center = map.GetClosestCell(map.GetCenterCoordinates());

            SetMainLandCellsRecursively(center.Coordinates);

            foreach (Cell cell in map.cells) {
                if (cell != null) {
                    if (cell.IsMainLand == false) {
                        map.ClearCell(cell.Coordinates);
                    }
                }
            }
        }

        private void SetMainLandCellsRecursively(Coordinates startingPoint) {
            Coordinates[] touchingCoordinates = Coordinates.getTouchingCoordinates(startingPoint);
            foreach (Coordinates coordinates in touchingCoordinates) {
                if(map.IsOutOfRange(coordinates)) continue;
                if(map.GetCell(coordinates) == null) continue;
                if (!map.GetCell(coordinates).IsMainLand) {
                    map.GetCell(coordinates).IsMainLand = true;
                    SetMainLandCellsRecursively(coordinates);
                }
            }
        }

        private void AddPlayersFields(List<PlayerField> playerFields) {
            int tries = 0;
            foreach (PlayerField playerField in playerFields) {
                AddPlayerField(playerField);
                tries++;

                if (tries > playerFields.Count * 10)
                    throw new Exception(
                        "Map generator error: it seems like there is not enough space for all players in generated map. Try to change parameters ;)");
            }
        }

        private void AddPlayerField(PlayerField playerField) {
            Cell startingCell = map.GetRandomCell();
            
            while(IsAnotherPlayerFieldInRange(startingCell.Coordinates, playerField)) startingCell = map.GetRandomCell();
            
            foreach (Cell cell in map.GetCircle(startingCell.Coordinates, playerField.radius)) {
                cell.PlayerId = playerField.playerId;
            }

            startingCell.Prefab = Prefabs.GetCapital(playerField.playerId);
        }

        private bool IsAnotherPlayerFieldInRange(Coordinates startingPoint, PlayerField playerField) {
            List<Cell> cellsInRange = map.GetCircle(startingPoint, playerField.radius);
            foreach (Cell cell in cellsInRange) {
                if(cell == null) continue;
                if (cell.PlayerId != 0) return true;
            }
            return false;
        }

        private void GenerateTrees() {
            foreach (Cell cell in map.cells) {
                if (cell != null && RandomNumber.getBoolByRatio(treeRatio)) {
                    if (cell.Prefab == Prefabs.GetNoElement()) {
                        cell.Prefab = Prefabs.GetTree();
                    }
                }
            }
        }
        
    }
}