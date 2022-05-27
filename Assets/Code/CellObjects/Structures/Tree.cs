using UnityEngine;

public class Tree : CellObject {
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
