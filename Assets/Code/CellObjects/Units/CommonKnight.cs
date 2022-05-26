using Code.CellObjects;
using UnityEngine;

public class CommonKnight : MonoBehaviour, IComparable, IBuyable {

    private int _price = 10;
    private int _maintainCost = 2;

    public bool IsWeakerThan(IComparable unit) {
        return Level() - unit.Level() < 0;
    }

    public int Level() {
        return 1;
    }
    
    public int GetPrice() {
        return _price;
    }

    public int GetMaintenanceCost() {
        return _maintainCost;
    }
    
}
