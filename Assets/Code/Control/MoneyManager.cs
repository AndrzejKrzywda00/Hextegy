using System;
using System.Collections;
using System.Collections.Generic;
using Code.CellObjects;
using UnityEngine;

public class MoneyManager : MonoBehaviour {

    private int _currentCoins = 2137;
    private int _balance = 420;

    public int GetCurrentCoins() {
        return _currentCoins;
    }

    public int GetBalance() {
        return _balance;
    }

    public void Buy(IBuyable entity) {
        _currentCoins -= entity.GetPrice();
        _balance -= entity.GetMaintenanceCost();
    }
    
}
