using Code.CellObjects;
using Code.Control.Game;
using UnityEngine;

namespace Code.Control.UI {
    public class UIButtonControl : MonoBehaviour {
    
        private PlayerController _playerController;

        private void Start() {
            _playerController = FindObjectOfType<PlayerController>();
        }

        public void OnUnit1ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetCommonKnight() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnUnit2ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetExperiencedKnight() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnUnit3ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetLegendaryKnight() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
    
        public void OnBuildingButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetHouse() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnTower1ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetNormalTower() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnTower2ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetSuperTower() : null;
            _playerController.selectedCellWithUnit = null;
        }
    }
}
