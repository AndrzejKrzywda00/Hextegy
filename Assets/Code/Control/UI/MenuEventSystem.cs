using Code.Audio;
using Code.DataAccess;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

namespace Code.Control.UI {
    public class MenuEventSystem : MonoBehaviour {
        public void StartClick() {
            SaveDataToSettings();
            SceneManager.LoadScene("Scenes/Main Scene");
        }

        private static void SaveDataToSettings() {
            Settings.AlivePlayersId.Clear();
            Settings.Winner = 0;
            for (int i = 1; i <= Settings.NumberOfPlayers; i++) {
                Settings.AlivePlayersId.Add(i);
            }
        }

        public void InstructionClick() {
            SceneManager.LoadScene("Scenes/Instruction");
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

        public void ToggleSoundsClick() {
            Sprite soundsOnSprite = Resources.Load<Sprite>("MenuButtons/Textures/icons/256x256/speaker");
            Sprite soundsOffSprite = Resources.Load<Sprite>("MenuButtons/Textures/icons/256x256/speakerCrossed");
            Image toggleSoundsImage = GameObject.FindGameObjectWithTag("ToggleSoundsButton").GetComponent<Image>();

            toggleSoundsImage.sprite = Settings.IsSoundEnabled ? soundsOffSprite : soundsOnSprite;
            AudioManager.ToggleSoundActivationStatus();
        }
    }
}
