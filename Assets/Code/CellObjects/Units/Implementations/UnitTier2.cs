namespace Code.CellObjects.Units.Implementations {
    public class UnitTier2 : Unit {
    
        public override int Price() {
            return 20;
        }

        public override bool IsProtectingNearbyFriendlyCells() {
            return true;
        }

        public override int MaintenanceCost() {
            return 6;
        }

        public override int Level() {
            return 2;
        }
    
        public override int MovementRange() {
            return 4;
        }
    }
}
