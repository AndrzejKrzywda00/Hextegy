namespace Code.CellObjects.Units.Implementations {
    public class UnitTier4 : Unit {
        
        public override int Price() {
            return 40;
        }

        public override bool IsProtectingNearbyFriendlyCells() {
            return true;
        }

        public override int MaintenanceCost() {
            return 26;
        }

        protected override int Level() {
            return 4;
        }

        public override int MovementRange() {
            return 2;
        }
    }
}
