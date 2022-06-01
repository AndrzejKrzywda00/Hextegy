using System.Collections.Generic;
using System.Linq;

namespace Code.Control.Game {
    
    public class PlayerManager {

        private static HexCell[] _gridCells;
        private static List<HexCell> _playerCells = new List<HexCell>();

        public PlayerManager(HexCell[] gridCells) {
            _gridCells = gridCells;
            _playerCells = new List<HexCell>();
        }
        
        public void LoadPlayer(int playerId) {
            IEnumerable<HexCell> playerIdQuery = from cell in _gridCells where cell.playerId.Equals(playerId) select cell;
            _playerCells.AddRange(playerIdQuery);
        }
        
        public int GetNumberOfUnits() {
            IEnumerable<HexCell> unitsQuery = from cell in _playerCells where cell.HasUnit() select cell;
            return unitsQuery.Count();
        }

        public int GetNumberOfEmptyCells() {
            IEnumerable<HexCell> emptyCellsQuery = from cell in _playerCells where cell.IsEmpty() select cell;
            return emptyCellsQuery.Count();
        }

        public int GetNumberOfFarms() {
            IEnumerable<HexCell> farmsQuery = from cell in _playerCells where cell.HasTower() select cell;
            return farmsQuery.Count();
        }
    }
    
}