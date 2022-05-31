namespace Code.CellObjects {
    
    public abstract class ActiveObject : CellObject {
        public abstract bool IsProtectingNearbyFriendlyCells();
        public abstract int Price();
        public abstract int MaintenanceCost();
    }
}