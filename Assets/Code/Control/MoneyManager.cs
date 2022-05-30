using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour {
    
    private readonly int _initialMoney = 1000;
    private Dictionary<int, int> _playersBalances;
    private Dictionary<int, int> _playersWallets;

    public int GetCurrentCoins(int playerId) {
        return _playersWallets[playerId];
    }

    public void IncrementBalance(int playerId) {
        _playersBalances[playerId] += 1;
    }

    public void IncrementBalanceOfPlayer(int playerId) {
        if(_playersBalances.ContainsKey(playerId)) _playersBalances[playerId] += 1;
    }

    public void DecrementBalanceOfPlayer(int playerId) {
        if(_playersBalances.ContainsKey(playerId)) _playersBalances[playerId] -= 1;
    }

    public void TransferBalanceOfFieldFromPlayerToPlayer(int playerId1, int playerId2) {
        DecrementBalanceOfPlayer(playerId1);
        IncrementBalanceOfPlayer(playerId2);
    }

    public void SetInitialBalanceOfPlayers(Dictionary<int, int> initialBalanceOfPlayers) {
        _playersBalances = initialBalanceOfPlayers;
        InitializeWallets();
    }

    private void InitializeWallets() {
        _playersWallets = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> item in _playersBalances) {
            _playersWallets.Add(item.Key, _initialMoney);
        }
    }

    public void CalculateWalletOnTurnEnd() {
        int currentPlayerBalance = _playersBalances[PlayerController.CurrentPlayerId];
        _playersWallets[PlayerController.CurrentPlayerId] += currentPlayerBalance;
    }

    public int GetBalance(int playerId) {
        return _playersBalances[playerId];
    }

    public void Buy(CellObject entity, int playerId) {
        _playersBalances[playerId] -= entity.GetMaintenanceCost();
        _playersWallets[playerId] -= entity.GetPrice();
    }

    public bool HasEnoughMoneyToBuy(CellObject entity, int playerId) {
        return entity.GetPrice() <= _playersWallets[playerId];
    }

}
