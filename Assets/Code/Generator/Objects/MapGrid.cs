using System;
using System.Collections.Generic;

namespace Code.Generator.Objects {
    public class MapGrid {
        
        public readonly Cell[] Cells;
        public readonly int Width;
        public readonly int Height;

        public MapGrid(int width, int height) {
            Width = width;
            Height = height;
            Cells = new Cell[height * width];
        }

        public bool IsOutOfRange(Coordinates coordinates) {
            if (coordinates.X >= Width || coordinates.X < 0 || coordinates.Y >= Height || coordinates.Y < 0)
                return true;
            return false;
        }

        public Cell GetCell(Coordinates coordinates) {
            if (IsOutOfRange(coordinates))
                throw new ArgumentOutOfRangeException("Out of range :) " + coordinates);
            return Cells[coordinates.X * Width + coordinates.Y];
        }

        public void SetCell(Cell cell) {
            if (cell == null) return;
            if (IsOutOfRange(cell.Coordinates))
                throw new ArgumentOutOfRangeException("Out of range :) " + cell.Coordinates);
            Cells[cell.Coordinates.X * Width + cell.Coordinates.Y] = cell;
        }

        public void ClearCell(Coordinates coordinates) {
            if (IsOutOfRange(coordinates))
                throw new ArgumentOutOfRangeException("Out of range :) " + coordinates);
            Cells[coordinates.X * Width + coordinates.Y] = null;
        }

        public Cell GetRandomCell() {
            return GetClosestCell(Coordinates.Random(Width, Height));
        }

        public Cell GetClosestCell(Coordinates coordinates) {
            if (GetCell(coordinates) != null) return GetCell(coordinates);

            for (int r = 1; r < Math.Max(Width, Height); r++) {
                foreach (Cell cell in GetRing(coordinates, r)) {
                    if (cell != null) return cell;
                }
            }

            return null;
        }

        private List<Cell> GetRing(Coordinates coordinates, int radius) {
            List<Cell> radiusCells = new List<Cell>();
            
            foreach (Cell cell in Cells) {
                if (cell == null) continue;
                if (Coordinates.Distance(coordinates, cell.Coordinates) == radius) {
                    radiusCells.Add(cell);
                }
            }

            return radiusCells;
        }

        public List<Cell> GetCircle(Coordinates coordinates, int radius) {
            List<Cell> radiusCells = new List<Cell>();
            
            foreach (Cell cell in Cells) {
                if (cell == null) continue;
                if (Coordinates.Distance(coordinates, cell.Coordinates) <= radius) {
                    radiusCells.Add(cell);
                }
            }

            return radiusCells;
        }

        public List<Cell> GetAllNotEmptyCells() {
            List<Cell> allNotEmptyCell = new List<Cell>();
            foreach (Cell cell in Cells)
                if (cell != null)
                    allNotEmptyCell.Add(cell);

            return allNotEmptyCell;
        }

        public Coordinates GetCenterCoordinates() {
            return new Coordinates(Width / 2, Height / 2);
        }

        public List<Cell> GetTouchingCells(Coordinates coordinates) {
            List<Cell> touchingCells = new List<Cell>();
            Coordinates[] touchingCoordinates = Coordinates.GetTouchingCoordinates(coordinates);

            foreach (Coordinates touchingCoord in touchingCoordinates) {
                if(IsOutOfRange(touchingCoord)) continue;
                Cell touchingCell = GetCell(touchingCoord);
                if(touchingCell != null) touchingCells.Add(touchingCell);
            }
            return touchingCells;
        }

        public int NumberOfNoEmptyCells() {
            int number = 0;
            foreach (Cell cell in Cells) {
                if (cell != null) number++;
            }
            return number;
        }
    }
}