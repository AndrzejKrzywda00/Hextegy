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
        _currentMoneyText.text = _moneyManager.GetCurrentCoins().ToString();
        _balanceText.text = _moneyManager.GetBalance().ToString();
    }
}
