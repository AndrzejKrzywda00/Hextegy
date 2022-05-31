using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Control {
    public class MenuEventSystem : MonoBehaviour {
        public void StartClick() {
            SceneManager.LoadScene("Scenes/Main Scene");
        }

        public void QuitClick() {
            Application.Quit();
        }

        public void BackToMenuClick() {
            SceneManager.LoadScene("Scenes/Menu");
        }
    }
}
