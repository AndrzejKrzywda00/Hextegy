using UnityEngine;

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
}
