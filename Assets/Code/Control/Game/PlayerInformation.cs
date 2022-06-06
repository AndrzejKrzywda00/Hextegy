using System.Collections.Generic;
using System.Linq;
using Code.CellObjects;
using Code.Hexagonal;

namespace Code.Control.Game {
    
    public class PlayerInformation {

        private static HexCell[] _gridCells;
        private static List<HexCell> _playerCells = new List<HexCell>();
        private static int _playerId;

        public PlayerInformation(HexCell[] gridCells) {
            _gridCells = gridCells;
            _playerCells = new List<HexCell>();
        }
        
        public void LoadPlayer(int playerId) {
            _playerId = playerId;
            CalculatePlayerCells();
        }

        private static void CalculatePlayerCells() {
            var playerIdQuery = from cell in _gridCells where cell.playerId.Equals(_playerId) select cell;
            _playerCells.AddRange(playerIdQuery);
        }

        private IEnumerable<ActiveObject> GetCellsWithActiveObjects() {
            return from cell in _playerCells where cell.HasActiveInstance() select (ActiveObject) cell.prefabInstance;
        }

        private int GetMaintenanceCostsOfActiveObjects() {
            var sumSequence = from activeObject in GetCellsWithActiveObjects() select activeObject.MaintenanceCost();
            return sumSequence.ToList().Sum();
        }

        public int GetBalance() {
            CalculatePlayerCells();
            return GetNumberOfEmptyCells() - GetMaintenanceCostsOfActiveObjects();
        }

        private int GetNumberOfEmptyCells() {
            IEnumerable<HexCell> emptyCellsQuery = from cell in _playerCells where cell.IsProfitable() select cell;
            return emptyCellsQuery.Count();
        }

        public int GetNumberOfFarms() {
            IEnumerable<HexCell> farmsQuery = from cell in _playerCells where cell.HasFarm() select cell;
            return farmsQuery.Count();
        }
    }
    
}