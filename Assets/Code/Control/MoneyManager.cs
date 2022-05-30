using UnityEngine;

public class MoneyManager : MonoBehaviour {

    // current money should be recalculated after each turn
    private int _currentMoney = 2137;
    
    // balance should be recalculated after each change on the map
    private int _balance = 420;

    public int GetCurrentCoins() {
        return _currentMoney;
    }

    public void IncrementBalance() {
        _balance++;
    }

    public int GetBalance() {
        return _balance;
    }

    public void Buy(CellObject entity) {
        _currentMoney -= entity.GetPrice();
        _balance -= entity.GetMaintenanceCost();
    }

    public bool HasEnoughMoneyToBuy(CellObject entity) {
        return entity.GetPrice() <= _currentMoney;
    }

}
