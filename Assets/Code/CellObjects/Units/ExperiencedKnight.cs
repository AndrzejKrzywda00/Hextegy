using Code.CellObjects.Units;

public class ExperiencedKnight : Unit {
    
    public override int Price() {
        return 20;
    }

    public override bool Protects() {
        return true;
    }

    public override int MaintenanceCost() {
        return 6;
    }

    public override int Level() {
        return 2;
    }
    
    public override int MovementRange() {
        return 4;
    }
}
