namespace Code.CellObjects.Structures.StateBuildings {
    public class Farm : ActiveObject {

        private int _startingPrice = 12;

        protected override int Level() {
            return 0;
        }

        public override int Price() {
            return _startingPrice;
        }

        public override bool IsProtectingNearbyFriendlyCells() {
            return false;
        }

        public override int MaintenanceCost() {
            return -4;
        }
    }
}
