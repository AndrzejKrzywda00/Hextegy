namespace Code.CellObjects.Structures {
    public class NormalTower : ActiveObject {
        
        public override int Level() {
            return 1;
        }

        public override bool IsProtectingNearbyFriendlyCells() {
            return true;
        }

        public override int Price() {
            return 15;
        }

        public override int MaintenanceCost() {
            return 1;
        }
    }
}
