using UnityEngine;

public class LegendaryKnight : MonoBehaviour, IComparable {
    
    private int _price = 30;
    private int _maintainCost = 18;

    public int Price => _price;
    public int MaintainCost => _maintainCost;
    
    public bool IsWeakerThan(IComparable unit) {
        return Level() - unit.Level() < 0;
    }

    public int Level() {
        return 3;
    }
}
