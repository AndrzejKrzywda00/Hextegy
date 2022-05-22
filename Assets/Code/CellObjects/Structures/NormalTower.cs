using UnityEngine;

public class NormalTower : MonoBehaviour {
    private int _price;

    public static void PutOnCell(HexCell hexCell) {
        NormalTower normalTower = Resources.Load<NormalTower>("NormalTower");
        hexCell.prefab = normalTower;
    }
    
    private void Start() {
        _price = 15;
    }
    
}
