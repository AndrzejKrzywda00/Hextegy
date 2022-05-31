namespace Code.CellObjects.Units {
    public class LegendaryKnight : Unit {
    
        public override int Price() {
            return 30;
        }

        public override bool Protects() {
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
