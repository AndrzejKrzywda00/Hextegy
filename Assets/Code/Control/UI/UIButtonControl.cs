using Code.CellObjects;
using Code.Control.Game;
using UnityEngine;

namespace Code.Control.UI {
    public class UIButtonControl : MonoBehaviour {
    
        private PlayerController _playerController;

        private void Start() {
            _playerController = FindObjectOfType<PlayerController>();
        }

        public void OnUnitTier1ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetUnitTier1() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnUnitTier2ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetUnitTier2() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnUnitTier3ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetUnitTier3() : null;
            _playerController.selectedCellWithUnit = null;
        }
        
        public void OnUnitTier4ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetUnitTier4() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
    
        public void OnBuildingButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetFarm() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnTowerTier1ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetTowerTier1() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnTowerTier2ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Prefabs.GetTowerTier2() : null;
            _playerController.selectedCellWithUnit = null;
        }
    }
}
