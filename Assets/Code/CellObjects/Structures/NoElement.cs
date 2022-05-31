using Code.Control.Exceptions;

public class NoElement : CellObject {
    public override int GetPrice() {
        throw new ThatShouldntBeUsedException();
    }

    public override int GetMaintenanceCost() {
        throw new ThatShouldntBeUsedException();
    }

    public override int Level() {
        return 0;
    }
    
    public override int Range() {
        return 0;
    }
}
