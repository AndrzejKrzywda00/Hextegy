using UnityEngine;

public abstract class CellObject : MonoBehaviour {
    private bool _hasMoveLeftInThisTurn = true;

    public bool SetHasMoveLeftInThisTurn {
        set => _hasMoveLeftInThisTurn = value;
    }

    public abstract int GetPrice();
    public abstract int GetMaintenanceCost();
    public abstract int Level();
    public abstract int Range();

    public bool CanMoveInThisTurn() {
        return Range() > 0 && _hasMoveLeftInThisTurn;
    }

    public bool IsWeakerThan(CellObject unit) {
        if (unit == null) return true;
        return Level() - unit.Level() < 0;
    }
}
