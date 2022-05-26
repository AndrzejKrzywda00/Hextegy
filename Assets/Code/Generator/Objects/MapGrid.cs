using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Generator {
    public class MapGrid {
        public Cell[] cells;
        public int width;
        public int height;

        public MapGrid(int width, int height) {
            this.width = width;
            this.height = height;
            cells = new Cell[height * width];
        }

        private bool isCoordinateOk(Coordinates coordinates) {
            if (coordinates.x >= width || coordinates.x < 0 || coordinates.y >= height || coordinates.y < 0)
                return false;
            return true;
        }

        public Cell getCell(Coordinates coordinates) {
            if (!isCoordinateOk(coordinates)) throw new ArgumentOutOfRangeException("Out of range :) " + coordinates.toString());
            return cells[coordinates.x * width + coordinates.y];
        }

        public void setCell(Cell cell) {
            if (cell == null) return;
            if (!isCoordinateOk(cell.coordinates)) throw new ArgumentOutOfRangeException("Out of range :) " + cell.coordinates.toString());
            Debug.Log(cell.coordinates.toString() + " len: " + cells.Length + " i: " + (cell.coordinates.x * width + cell.coordinates.y));
            cells[cell.coordinates.x * width + cell.coordinates.y] = cell;
        }

        public Cell getClosestCell(Coordinates coordinates) {
            if (getCell(coordinates) != null) return getCell(coordinates);
            throw new Exception("XDD");
        }

        public List<Cell> getRing(Coordinates coordinates, int radius) {
            List<Cell> radiusCells = new List<Cell>();
            foreach (Cell cell in cells) {
                if(cell == null) continue;
                if (Coordinates.Distance(coordinates, cell.coordinates) == radius) {
                    radiusCells.Add(cell);
                }
            }

            return radiusCells;
        }
    }
}