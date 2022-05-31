namespace Code.CellObjects.Structures {
    public class Farm : ActiveObject {

        private int _currentPrice = 12;
        
        public override int Level() {
            return 1;
        }

        public override int Price() {
            return _currentPrice;
        }

        public void IncrementPrice() {
            _currentPrice += 4;
        }

        public override bool Protects() {
            return false;
        }

        public override int MaintenanceCost() {
            return -4;
        }
    }
}
