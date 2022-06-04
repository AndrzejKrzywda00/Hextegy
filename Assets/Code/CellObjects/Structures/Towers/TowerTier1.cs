namespace Code.CellObjects.Structures.Towers {
    public class TowerTier1 : ActiveObject {
        
        public override int Level() {
            return 2;
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
