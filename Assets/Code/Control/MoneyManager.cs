using UnityEngine;

public class MoneyManager : MonoBehaviour {

    private int _currentMoney = 2137;
    private int _balance = 420;

    public int GetCurrentCoins() {
        return _currentMoney;
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
