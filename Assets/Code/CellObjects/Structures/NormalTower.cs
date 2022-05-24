using UnityEngine;

public class NormalTower : MonoBehaviour, IComparable {
    
    private int _price;
    public int Price => _price;

    public static void PutOnCell(HexCell hexCell) {
        NormalTower normalTower = Resources.Load<NormalTower>("NormalTower");
        hexCell.prefab = normalTower;
    }

    private void Start() {
        _price = 15;
    }

    public bool IsWeakerThan(IComparable unit) {
        return Level() - unit.Level() < 0;
    }

    public int Level() {
        return 1;
    }
}
