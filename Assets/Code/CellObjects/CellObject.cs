using UnityEngine;

public abstract class CellObject : MonoBehaviour {
    public abstract int GetPrice();
    public abstract int GetMaintenanceCost();
    public abstract int Level();

    public bool IsWeakerThan(CellObject unit) {
        return Level() - unit.Level() < 0;
    }
}