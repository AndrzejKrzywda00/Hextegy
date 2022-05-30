using System;
using System.Collections.Generic;

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

        public bool IsOutOfRange(Coordinates coordinates) {
            if (coordinates.x >= width || coordinates.x < 0 || coordinates.y >= height || coordinates.y < 0)
                return true;
            return false;
        }

        public Cell GetCell(Coordinates coordinates) {
            if (IsOutOfRange(coordinates))
                throw new ArgumentOutOfRangeException("Out of range :) " + coordinates.toString());
            return cells[coordinates.x * width + coordinates.y];
        }

        public void SetCell(Cell cell) {
            if (cell == null) return;
            if (IsOutOfRange(cell.Coordinates))
                throw new ArgumentOutOfRangeException("Out of range :) " + cell.Coordinates.toString());
            cells[cell.Coordinates.x * width + cell.Coordinates.y] = cell;
        }

        public void ClearCell(Coordinates coordinates) {
            if (IsOutOfRange(coordinates))
                throw new ArgumentOutOfRangeException("Out of range :) " + coordinates.toString());
            cells[coordinates.x * width + coordinates.y] = null;
        }

        public Cell GetRandomCell() {
            return GetClosestCell(Coordinates.Random(width, height));
        }

        public Cell GetClosestCell(Coordinates coordinates) {
            if (GetCell(coordinates) != null) return GetCell(coordinates);

            for (int r = 1; r < Math.Max(width, height); r++) {
                foreach (Cell cell in GetRing(coordinates, r)) {
                    if (cell != null) return cell;
                }
            }

            return null;
        }

        public List<Cell> GetRing(Coordinates coordinates, int radius) {
            List<Cell> radiusCells = new List<Cell>();
            foreach (Cell cell in cells) {
                if (cell == null) continue;
                if (Coordinates.Distance(coordinates, cell.Coordinates) == radius) {
                    radiusCells.Add(cell);
                }
            }

            return radiusCells;
        }

        public List<Cell> GetCircle(Coordinates coordinates, int radius) {
            List<Cell> radiusCells = new List<Cell>();
            foreach (Cell cell in cells) {
                if (cell == null) continue;
                if (Coordinates.Distance(coordinates, cell.Coordinates) <= radius) {
                    radiusCells.Add(cell);
                }
            }

            return radiusCells;
        }

        public List<Cell> GetAllNotEmptyCells() {
            List<Cell> allNotEmptyCell = new List<Cell>();
            foreach (Cell cell in cells)
                if (cell != null)
                    allNotEmptyCell.Add(cell);

            return allNotEmptyCell;
        }

        public Coordinates GetCenterCoordinates() {
            return new Coordinates(width / 2, height / 2);
        }

        public List<Cell> GetTouchingCells(Coordinates coordinates) {
            List<Cell> touchingCells = new List<Cell>();
            Coordinates[] touchingCoordinates = Coordinates.getTouchingCoordinates(coordinates);

            foreach (Coordinates touchingCoord in touchingCoordinates) {
                if(IsOutOfRange(touchingCoord)) continue;
                Cell touchingCell = GetCell(touchingCoord);
                if(touchingCell != null) touchingCells.Add(touchingCell);
            }
            return touchingCells;
        }

        public int NumberOfNoEmptyCells() {
            int number = 0;
            foreach (Cell cell in cells) {
                if (cell != null) number++;
            }
            return number;
        }
    }
}