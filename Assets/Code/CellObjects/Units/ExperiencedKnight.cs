using UnityEngine;

public class ExperiencedKnight : MonoBehaviour, IComparable {
    
    private int _price = 20;
    private int _maintainCost = 6;

    public int Price => _price;
    public int MaintainCost => _maintainCost;

    public bool IsWeakerThan(IComparable unit) {
        return Level() - unit.Level() < 0;
    }

    public int Level() {
        return 2;
    }
}
