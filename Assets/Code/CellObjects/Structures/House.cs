using UnityEngine;

public class House : MonoBehaviour {
    
    private int _moneyGeneratedPerTurn;
    private int _price;

    public int Price => _price;
    public int MoneyPerTurn => _moneyGeneratedPerTurn;

    public static void PutOnCell(HexCell hexCell) {
        House house = Resources.Load<House>("House");
        hexCell.prefab = house;
    }
    
    private void Start() {
        _moneyGeneratedPerTurn = 4;
        _price = 12;
    }

    private void IncreasePrice() { 
        _price += 2;
    }
}
