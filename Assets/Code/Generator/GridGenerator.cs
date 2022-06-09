using System;
using System.Collections.Generic;
using Code.CellObjects;
using Code.Generator.Objects;
using Code.Generator.Util;

namespace Code.Generator {
    public class GridGenerator {
        
        private MapGrid _map;

        public float Scale = 3f;                                            // good result in range [0.5, 10]
        public float Fulfil = 0.5f;                                         // [0, 1]: 0 - empty map, 1 - full map
        public float TreeRatio = 0.2f;                                      // [0, 1]: 0 - no trees, 1 - trees everywhere
        public List<PlayerField> PlayerFields = new List<PlayerField>();    // list of fields, one player id can occur many times

        
        public class PlayerField {
            public readonly int PlayerId;        // player id
            public readonly int Radius;          // radius of field

            public PlayerField(int playerId, int radius) {
                this.PlayerId = playerId;
                this.Radius = radius;
            }
        }
        
        public Cell[] GenerateMap(int height, int width) {
            _map = new MapGrid(width, height);
            
            //Debug.Log("Generating perlin noise...");
            GenerateByPerlinNoise();
            
            //Debug.Log("Clearing unconnected islands...");
            ClearUnconnectedIslands();
            
            //Debug.Log("Generating players fields...");
            AddPlayersFields(PlayerFields);
            
            //Debug.Log("Generating trees...");
            GenerateTrees();

            //Debug.Log("Generation finished!");
            return _map.Cells;
        }

        public void GeneratePlayerFields(int numberOfPlayers, int radius) {
            PlayerFields = new List<PlayerField>();
            for (int playerId = 1; playerId <= numberOfPlayers; playerId++) {
                PlayerFields.Add(new PlayerField(playerId, radius));
            }
        }
        

        private void GenerateByPerlinNoise() {
            bool[,] perlinNoise = PerlinNoise.Generate(_map.Height, _map.Width, Scale, Fulfil);

            for (int x = 0; x < _map.Width; x++) {
                for (int y = 0; y < _map.Height; y++) {
                    Cell newCell = new Cell(x, y);
                    _map.SetCell(perlinNoise[x, y] ? newCell : null);
                }
            }

            if (_map.NumberOfNoEmptyCells() < _map.Width * _map.Height * 0.5 * Fulfil + 1)
                throw new Exception(
                    "Map generator error: for this parameters generated map is mostly empty. Try one more time");
        }

        private void ClearUnconnectedIslands() {
            Cell center = _map.GetClosestCell(_map.GetCenterCoordinates());

            SetMainLandCellsRecursively(center.Coordinates);

            foreach (Cell cell in _map.Cells) {
                if (cell == null) continue;
                if (cell.IsMainLand == false) {
                    _map.ClearCell(cell.Coordinates);
                }
            }
        }

        private void SetMainLandCellsRecursively(Coordinates startingPoint) {
            Coordinates[] touchingCoordinates = Coordinates.GetTouchingCoordinates(startingPoint);
            foreach (Coordinates coordinates in touchingCoordinates) {
                if(_map.IsOutOfRange(coordinates)) continue;
                if(_map.GetCell(coordinates) == null) continue;
                if (!_map.GetCell(coordinates).IsMainLand) {
                    _map.GetCell(coordinates).IsMainLand = true;
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
            Cell startingCell = _map.GetRandomCell();
            
            while(IsAnotherPlayerFieldInRange(startingCell.Coordinates, playerField)) startingCell = _map.GetRandomCell();
            
            foreach (Cell cell in _map.GetCircle(startingCell.Coordinates, playerField.Radius)) {
                cell.PlayerId = playerField.PlayerId;
            }

            startingCell.Prefab = Prefabs.GetCapital(playerField.PlayerId);
        }

        private bool IsAnotherPlayerFieldInRange(Coordinates startingPoint, PlayerField playerField) {
            List<Cell> cellsInRange = _map.GetCircle(startingPoint, playerField.Radius);
            foreach (Cell cell in cellsInRange) {
                if (cell == null) continue;
                if (cell.PlayerId != 0) return true;
            }
            return false;
        }

        private void GenerateTrees() {
            foreach (Cell cell in _map.Cells) {
                if (cell != null && RandomNumber.GetBoolByRatio(TreeRatio)) {
                    if (cell.Prefab == Prefabs.GetNoElement()) {
                        cell.Prefab = Prefabs.GetTree();
                    }
                }
            }
        }
        
    }
}