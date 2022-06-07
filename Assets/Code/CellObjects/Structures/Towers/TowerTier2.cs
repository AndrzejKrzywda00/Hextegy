namespace Code.CellObjects.Structures.Towers {
    public class TowerTier2 : ActiveObject {
        protected override int Level() {
            return 3;
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
