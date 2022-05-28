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

        public bool isOutOfRange(Coordinates coordinates) {
            if (coordinates.x >= width || coordinates.x < 0 || coordinates.y >= height || coordinates.y < 0)
                return true;
            return false;
        }

        public Cell getCell(Coordinates coordinates) {
            if (isOutOfRange(coordinates))
                throw new ArgumentOutOfRangeException("Out of range :) " + coordinates.toString());
            return cells[coordinates.x * width + coordinates.y];
        }

        public void setCell(Cell cell) {
            if (cell == null) return;
            if (isOutOfRange(cell.coordinates))
                throw new ArgumentOutOfRangeException("Out of range :) " + cell.coordinates.toString());
            cells[cell.coordinates.x * width + cell.coordinates.y] = cell;
        }

        public void clearCell(Coordinates coordinates) {
            if (isOutOfRange(coordinates))
                throw new ArgumentOutOfRangeException("Out of range :) " + coordinates.toString());
            cells[coordinates.x * width + coordinates.y] = null;
        }

        public Cell getRandomCell() {
            return getClosestCell(Coordinates.Random(width, height));
        }

        public Cell getClosestCell(Coordinates coordinates) {
            if (getCell(coordinates) != null) return getCell(coordinates);

            for (int r = 1; r < Math.Max(width, height); r++) {
                foreach (Cell cell in getRing(coordinates, r)) {
                    if (cell != null) return cell;
                }
            }

            return null;
        }

        public List<Cell> getRing(Coordinates coordinates, int radius) {
            List<Cell> radiusCells = new List<Cell>();
            foreach (Cell cell in cells) {
                if (cell == null) continue;
                if (Coordinates.Distance(coordinates, cell.coordinates) == radius) {
                    radiusCells.Add(cell);
                }
            }

            return radiusCells;
        }

        public List<Cell> getCircle(Coordinates coordinates, int radius) {
            List<Cell> radiusCells = new List<Cell>();
            foreach (Cell cell in cells) {
                if (cell == null) continue;
                if (Coordinates.Distance(coordinates, cell.coordinates) <= radius) {
                    radiusCells.Add(cell);
                }
            }

            return radiusCells;
        }

        public List<Cell> getAllNotEmptyCells() {
            List<Cell> allNotEmptyCell = new List<Cell>();
            foreach (Cell cell in cells)
                if (cell != null)
                    allNotEmptyCell.Add(cell);

            return allNotEmptyCell;
        }

        public Coordinates getCenterCoordinates() {
            return new Coordinates(width / 2, height / 2);
        }

        public List<Cell> getTouchingCells(Coordinates coordinates) {
            List<Cell> touchingCells = new List<Cell>();
            Coordinates[] touchingCoordinates = Coordinates.getTouchingCoordinates(coordinates);

            foreach (Coordinates touchingCoord in touchingCoordinates) {
                if(isOutOfRange(touchingCoord)) continue;
                Cell touchingCell = getCell(touchingCoord);
                if(touchingCell != null) touchingCells.Add(touchingCell);
            }
            return touchingCells;
        }

        public int numberOfNoEmptyCells() {
            int number = 0;
            foreach (Cell cell in cells) {
                if (cell != null) number++;
            }
            return number;
        }
    }
}