using Code.Control.Game;
using Code.DataAccess;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Control.UI {
    public class UIDisplaysControl : MonoBehaviour {
    
        public TextMeshProUGUI currentMoneyText;
        public TextMeshProUGUI balanceText;
        public Image playerIdentifierImage;

        private void Start() {
            currentMoneyText = GameObject.FindGameObjectWithTag("CurrentMoneyText").GetComponent<TextMeshProUGUI>();
            balanceText = GameObject.FindGameObjectWithTag("BalanceText").GetComponent<TextMeshProUGUI>();
            playerIdentifierImage = GameObject.FindGameObjectWithTag("PlayerIdentifierImage").GetComponent<Image>();
        }

        private void FixedUpdate() {
            currentMoneyText.text = "Coins: " + MoneyManager.GetCurrentCoins(PlayerController.CurrentPlayerId);
            balanceText.text = "Balance: " + MoneyManager.GetBalance(PlayerController.CurrentPlayerId);
            playerIdentifierImage.color = ColorPalette.GetColorOfPlayer(PlayerController.CurrentPlayerId);
        }
    }
}
