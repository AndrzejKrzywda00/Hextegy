using Code.Control.Game;
using TMPro;
using UnityEngine;

namespace Code.Control.UI {
    public class UIMoneyDisplaysControl : MonoBehaviour {
    
        public TextMeshProUGUI currentMoneyText;
        public TextMeshProUGUI balanceText;

        private void FixedUpdate() {
            currentMoneyText.text = "Coins: " + MoneyManager.GetCurrentCoins(PlayerController.CurrentPlayerId);
            balanceText.text = "Balance: " + MoneyManager.GetBalance(PlayerController.CurrentPlayerId);
        }
    }
}
