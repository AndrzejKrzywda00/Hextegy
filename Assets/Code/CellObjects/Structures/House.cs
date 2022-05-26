using UnityEngine;

public class House : MonoBehaviour, IBuyable {
    
    private int _moneyGeneratedPerTurn;
    private int _price;

    private void Start() {
        _moneyGeneratedPerTurn = 4;
        _price = 12;
    }
    
    public int GetPrice() {
        return _price;
    }

    public int GetMaintenanceCost() {
        return -_moneyGeneratedPerTurn;
    }
}
