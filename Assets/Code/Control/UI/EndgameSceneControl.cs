using Code.DataAccess;
using TMPro;
using UnityEngine;

namespace Code.Control.UI {
    public class EndgameSceneControl: MonoBehaviour {
    
        public TextMeshProUGUI _text;

        private void Start() {
            _text = GameObject.FindGameObjectWithTag("EndgameText").GetComponent<TextMeshProUGUI>();
            if (Settings.Winner > 0) {
                _text.text = "The winner is player " + Settings.Winner + "!";
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().backgroundColor =
                    ColorPalette.GetColorOfPlayer(Settings.Winner);
            }
            else {
                _text.text = "You are all loosers.";
            }
            
        }
    }
}
