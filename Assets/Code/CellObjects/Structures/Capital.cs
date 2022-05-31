using Code.Control.Exceptions;

namespace Code.CellObjects.Structures {
    public class Capital : ActiveObject {
        
        private int _playerId;
        public int PlayerId => _playerId;

        public void SetPlayerId(int id) {
            _playerId = id;
        }
        
        public override int Level() {
            return 1;
        }

        public override bool IsProtectingNearbyFriendlyCells() {
            return true;
        }

        public override int Price() {
            return 0;
        }

        public override int MaintenanceCost() {
            return 0;
        }
    }
}
