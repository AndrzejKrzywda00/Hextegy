public class House : CellObject {
    public override int GetPrice() {
        return 12;
    }

    public override int GetMaintenanceCost() {
        return -4;
    }

    public override int Level() {
        return 0;
    }
}
