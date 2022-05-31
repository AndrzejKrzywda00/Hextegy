using TMPro;
using UnityEngine;

public class UIMoneyDisplaysControl : MonoBehaviour {
    public TextMeshProUGUI currentMoneyText;
    public TextMeshProUGUI balanceText;

    private void FixedUpdate() {
        currentMoneyText.text = "Coins: " + MoneyManager.GetCurrentCoins(PlayerController.CurrentPlayerId);
        balanceText.text = "Balance: " + MoneyManager.GetBalance(PlayerController.CurrentPlayerId);
    }
}
