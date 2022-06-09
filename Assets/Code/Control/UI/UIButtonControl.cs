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
            _playerController.prefabFromUI = _playerController.prefabFromUI != Prefabs.GetUnitTier1() ? Prefabs.GetUnitTier1() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnUnitTier2ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI != Prefabs.GetUnitTier2() ? Prefabs.GetUnitTier2() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnUnitTier3ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI != Prefabs.GetUnitTier3() ? Prefabs.GetUnitTier3() : null;
            _playerController.selectedCellWithUnit = null;
        }
        
        public void OnUnitTier4ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI != Prefabs.GetUnitTier4() ? Prefabs.GetUnitTier4() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
    
        public void OnFarmButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI != Prefabs.GetFarm() ? Prefabs.GetFarm() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnTowerTier1ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI != Prefabs.GetTowerTier1() ? Prefabs.GetTowerTier1() : null;
            _playerController.selectedCellWithUnit = null;
        }
    
        public void OnTowerTier2ButtonClick() {
            _playerController.prefabFromUI = _playerController.prefabFromUI != Prefabs.GetTowerTier2() ? Prefabs.GetTowerTier2() : null;
            _playerController.selectedCellWithUnit = null;
        }
    }
}
