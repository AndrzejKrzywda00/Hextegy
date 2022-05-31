namespace Code.CellObjects.Structures.Towers {
    public class TowerTier2 : ActiveObject {
        public override int Level() {
            return 2;
        }

        public override bool IsProtectingNearbyFriendlyCells() {
            return true;
        }

        public override int Price() {
            return 35;
        }

        public override int MaintenanceCost() {
            return 5;
        }
    }
}
