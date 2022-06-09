using Code.DataAccess;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Control.UI {
    public class MenuEventSystem : MonoBehaviour {
        public void StartClick() {
            Settings.AlivePlayersId.Clear();
            for (int i = 1; i <= Settings.NumberOfPlayers; i++) {
                Settings.AlivePlayersId.Add(i);
            }
            SceneManager.LoadScene("Scenes/Main Scene");
        }

        public void QuitClick() {
            Application.Quit();
        }

        public void BackToMenuClick() {
            SceneManager.LoadScene("Scenes/Menu");
        }

        public void EndgameClick() {
            SceneManager.LoadScene("Scenes/Endgame");
        }

        public void SettingsClick() {
            SceneManager.LoadScene("Scenes/Settings");
        }
    }
}
