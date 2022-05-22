using UnityEngine;

public class House : MonoBehaviour {
    private int _moneyGeneratedPerTurn;
    private int _price;

    public static void PutOnCell(HexCell hexCell) {
        House house = Resources.Load<House>("House");
        hexCell.prefab = house;
    }
    
    private void Start() {
        _moneyGeneratedPerTurn = 4;
        _price = 10;
    }

    private void IncreasePrice() {
        if(_price < 1000 ) _price += 2;
    }
}
