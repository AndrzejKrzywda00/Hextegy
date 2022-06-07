using System.Collections.Generic;
using System.Linq;
using Code.CellObjects;
using Code.Hexagonal;

namespace Code.Control.Game {
    
    public static class PlayerInformation {

        private static HexCell[] _gridCells;
        private static List<HexCell> _playerCells = new List<HexCell>();
        private static int _playerId;

        public static void SetGridCells(HexCell[] cells) {
            _gridCells = cells;
        }

        private static void InstantiatePlayerCells() {
            _playerCells = new List<HexCell>();
        }

        public static void LoadPlayer(int playerId) {
            _playerId = playerId;
            InstantiatePlayerCells();
            CalculatePlayerCells();
        }

        private static void CalculatePlayerCells() {
            IEnumerable<HexCell> playerIdQuery = from cell in _gridCells where cell.playerId.Equals(_playerId) select cell;
            _playerCells.AddRange(playerIdQuery);
        }

        private static IEnumerable<ActiveObject> GetCellsWithActiveObjects() {
            return from cell in _playerCells where cell.HasActiveInstance() select (ActiveObject) cell.prefabInstance;
        }

        private static int GetMaintenanceCostsOfActiveObjects() {
            IEnumerable<int> sumSequence = from activeObject in GetCellsWithActiveObjects() select activeObject.MaintenanceCost();
            return sumSequence.ToList().Sum();
        }

        public static int GetBalance() {
            CalculatePlayerCells();
            return GetNumberOfEmptyCells() - GetMaintenanceCostsOfActiveObjects();
        }

        private static int GetNumberOfEmptyCells() {
            IEnumerable<HexCell> emptyCellsQuery = from cell in _playerCells where cell.IsProfitable() select cell;
            return emptyCellsQuery.Count();
        }

        public static int GetNumberOfFarms() {
            IEnumerable<HexCell> farmsQuery = from cell in _playerCells where cell.HasFarm() select cell;
            return farmsQuery.Count();
        }

        public static bool HasCapital() {
            IEnumerable<HexCell> hasCapital = from cell in _playerCells where cell.HasCapital() select cell;
            return hasCapital.Any();
        }
    }
    
}