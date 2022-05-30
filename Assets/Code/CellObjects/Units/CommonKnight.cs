public class CommonKnight : Unit {
    public override int GetPrice() {
        return 10;
    }

    public override int GetMaintenanceCost() {
        return 2;
    }

    public override int Level() {
        return 1;
    }

    public override int Range() {
        return 4;
    }
}
