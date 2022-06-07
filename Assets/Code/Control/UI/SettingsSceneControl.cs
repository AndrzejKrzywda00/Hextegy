using UnityEngine;
using UnityEngine.UI;
using Code.DataAccess;
using TMPro;
using UnityEngine.SceneManagement;

namespace Code.Control.UI {
    public class SettingsSceneControl : MonoBehaviour {

        [SerializeField] private Slider noOfPlayersSlider;
        [SerializeField] private Slider mapSizeSlider;
        [SerializeField] private Slider scaleSlider;
        [SerializeField] private Slider fulfillSlider;
        [SerializeField] private Slider treeRatioSlider;
        
        [SerializeField] private TextMeshProUGUI noOfPlayersText;
        [SerializeField] private TextMeshProUGUI mapSizeText;
        [SerializeField] private TextMeshProUGUI scaleText;
        [SerializeField] private TextMeshProUGUI fulfillText;
        [SerializeField] private TextMeshProUGUI treeRatioText;

        private void Start() {
            AddListeners();
            SetDefaults();
            RefreshText();
        }
        
        private void AddListeners() {
            noOfPlayersSlider.onValueChanged.AddListener((v) => {
                noOfPlayersText.text = v.ToString("0");
            });
            mapSizeSlider.onValueChanged.AddListener((v) => {
                mapSizeText.text = v.ToString("0") + "x" + v.ToString("0");
            });
            scaleSlider.onValueChanged.AddListener((v) => {
                scaleText.text = v.ToString("0.0");
            });
            fulfillSlider.onValueChanged.AddListener((v) => {
                fulfillText.text = (v*100).ToString("0") + "%";
            });
            treeRatioSlider.onValueChanged.AddListener((v) => {
                treeRatioText.text = (v*100).ToString("0") + "%";
            });
        }

        private void SetDefaults() {
            noOfPlayersSlider.value = Settings.NumberOfPlayers;
            mapSizeSlider.value = Settings.MapSize;
            scaleSlider.value = Settings.Scale;
            fulfillSlider.value = Settings.Fulfill;
            treeRatioSlider.value = Settings.TreeRatio;
        }

        private void RefreshText() {
            noOfPlayersText.text = noOfPlayersSlider.value.ToString("0");
            mapSizeText.text = mapSizeSlider.value.ToString("0") + "x" + mapSizeSlider.value.ToString("0");
            scaleText.text = scaleSlider.value.ToString("0.0");
            fulfillText.text = (fulfillSlider.value*100).ToString("0") + "%";
            treeRatioText.text = (treeRatioSlider.value*100).ToString("0") + "%";
        }

        public void SaveClick() {
              Settings.NumberOfPlayers = (int)noOfPlayersSlider.value;
              Settings.MapSize = (int)mapSizeSlider.value;
              Settings.Scale = scaleSlider.value;
              Settings.Fulfill = fulfillSlider.value;
              Settings.TreeRatio = treeRatioSlider.value;
              SceneManager.LoadScene("Scenes/Menu");
        }
    }
}