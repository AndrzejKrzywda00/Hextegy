namespace Code.CellObjects {
    
    public abstract class ActiveObject : CellObject {
        public abstract bool Protects();
        public abstract int Price();
        public abstract int MaintenanceCost();
    }
}