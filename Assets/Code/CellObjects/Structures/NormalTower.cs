public class NormalTower : CellObject {
    public override int GetPrice() {
        return 15;
    }

    public override int GetMaintenanceCost() {
        return 1;
    }

    public override int Level() {
        return 1;
    }
}
