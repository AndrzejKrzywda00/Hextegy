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
    
    public int Range() {
        return 5;
    }
}
