namespace Code.CellObjects.Units.Implementations {
    public class UnitTier3 : Unit {
    
        public override int Price() {
            return 30;
        }

        public override bool IsProtectingNearbyFriendlyCells() {
            return true;
        }

        public override int MaintenanceCost() {
            return 18;
        }

        public override int Level() {
            return 3;
        }

        public override int MovementRange() {
            return 3;
        }
    }
}
