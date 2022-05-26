using UnityEngine;

public class NormalTower : MonoBehaviour, IComparable {
    
    private int _price = 15;
    private int _maintainCost = 1;
    public int Price => _price;
    public int MaintainCost => _maintainCost;

    public bool IsWeakerThan(IComparable unit) {
        return Level() - unit.Level() < 0;
    }

    public int Level() {
        return 1;
    }
}
