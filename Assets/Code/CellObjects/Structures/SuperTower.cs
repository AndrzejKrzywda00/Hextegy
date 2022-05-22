using UnityEngine;

public class SuperTower : MonoBehaviour {
    private int _price;

    public static void PutOnCell(HexCell hexCell) {
        SuperTower superTower = Resources.Load<SuperTower>("SuperTower");
        hexCell.prefab = superTower;
    }
    
    private void Start() {
        _price = 35;
    }
    
}
