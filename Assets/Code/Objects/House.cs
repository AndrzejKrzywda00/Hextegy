using UnityEngine;

public class House : MonoBehaviour {
    private int _moneyGeneratedPerTurn;
    private int _price;

    private void Start() {
        _moneyGeneratedPerTurn = 4;
        _price = 10;
    }

    private void IncreasePrice() {
        if(_price < 1000 ) _price += 2;
    }
}
