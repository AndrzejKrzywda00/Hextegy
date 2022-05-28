using UnityEngine;

public abstract class CellObject : MonoBehaviour {
    public abstract int GetPrice();
    public abstract int GetMaintenanceCost();
    public abstract int Level();

    public bool IsWeakerThan(CellObject unit) {
        if (unit == null) return true;
        return Level() - unit.Level() < 0;
    }
}
