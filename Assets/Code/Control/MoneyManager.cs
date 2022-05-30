using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour {

    // current money should be recalculated after each turn
    private int _currentMoney = 2137;
    
    // balance should be recalculated after each change on the map
    private int _balance = 420;

    private Dictionary<int, int> _playersBalances;
    private Dictionary<int, int> _playersWallets;

    public int GetCurrentCoins() {
        return _currentMoney;
    }

    public void IncrementBalance() {
        _balance++;
    }

    public void IncrementBalanceOfPlayer(int playerId) {
        if(_playersBalances.ContainsKey(playerId)) _playersBalances[playerId] += 1;
    }

    public void DecrementBalanceOfPlayer(int playerId) {
        if(_playersBalances.ContainsKey(playerId)) _playersBalances[playerId] -= 1;
    }

    public void TransferBalanceOfFieldFromPlayerToPlayer(int pid1, int pid2) {
        DecrementBalanceOfPlayer(pid1);
        IncrementBalanceOfPlayer(pid2);
    }

    public void SetInitialBalanceOfPlayers(Dictionary<int, int> initialBalanceOfPlayers) {
        _playersBalances = initialBalanceOfPlayers;
        foreach (KeyValuePair<int, int> item in _playersBalances) {
            Debug.Log(item.Key + " => " + item.Value);
        }
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
