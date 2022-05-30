public class ExperiencedKnight : CellObject {
    public override int GetPrice() {
        return 20;
    }

    public override int GetMaintenanceCost() {
        return 6;
    }

    public override int Level() {
        return 2;
    }
    
    public override int Range() {
        return 4;
    }
}
