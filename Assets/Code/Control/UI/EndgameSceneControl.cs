using TMPro;
using UnityEngine;

namespace Code.Control.UI {
    public class EndgameSceneControl: MonoBehaviour {
    
        private TextMeshProUGUI _text;

        private void Start() {
            _text = GameObject.FindGameObjectWithTag("EndgameText").GetComponent<TextMeshProUGUI>();
            _text.text = "The winner is Player NOT SET";
        }
    }
}
