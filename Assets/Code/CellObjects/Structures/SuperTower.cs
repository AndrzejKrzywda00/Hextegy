using UnityEngine;

public class SuperTower : MonoBehaviour, IComparable {
    
    private int _price;
    private int _maintainCost = 6;
    public int Price => _price;
    public int MaintainCost => _maintainCost;

    private void Start() {
        _price = 35;
    }

    public bool IsWeakerThan(IComparable unit) {
        return Level() - unit.Level() < 0;
    }

    public int Level() {
        return 2;
    }
}
