namespace Code.CellObjects.Units.Implementations {
    public class UnitTier1 : Unit {

        public override bool IsProtectingNearbyFriendlyCells() {
            return true;
        }
    
        public override int Price() {
            return 10;
        }

        public override int MaintenanceCost() {
            return 2;
        }

        public override int Level() {
            return 1;
        }

        public override int MovementRange() {
            return 5;
        }
    }
}
