using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIMoneyDisplaysControl : MonoBehaviour {
    //todo do it better way
    public MoneyManager _moneyManager;
    public TextMeshProUGUI _currentMoneyText;
    public TextMeshProUGUI _balanceText;

    private void FixedUpdate() {
        _currentMoneyText.text = "Coins: " + _moneyManager.GetCurrentCoins(PlayerController.CurrentPlayerId);
        _balanceText.text = "Balance: " + _moneyManager.GetBalance(PlayerController.CurrentPlayerId);
    }
}
