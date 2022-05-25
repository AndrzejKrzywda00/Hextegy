using UnityEngine;

public class SuperTower : MonoBehaviour, IComparable {
    
    private int _price;
    public int Price => _price;

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
