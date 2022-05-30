using UnityEngine;

public class UIButtonControl : MonoBehaviour {
    
    private PlayerController _playerController;

    private void Start() {
        _playerController = FindObjectOfType<PlayerController>();
    }

    public void OnUnit1ButtonClick() {
        _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Resources.Load<CellObject>("CommonKnight") : null;
        _playerController.selectedCellWithUnit = null;
    }
    
    public void OnUnit2ButtonClick() {
        _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Resources.Load<CellObject>("ExperiencedKnight") : null;
        _playerController.selectedCellWithUnit = null;
    }
    
    public void OnUnit3ButtonClick() {
        _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Resources.Load<CellObject>("LegendaryKnight") : null;
        _playerController.selectedCellWithUnit = null;
    }
    
    
    public void OnBuildingButtonClick() {
        _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Resources.Load<CellObject>("House") : null;
        _playerController.selectedCellWithUnit = null;
    }
    
    public void OnTower1ButtonClick() {
        _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Resources.Load<CellObject>("NormalTower") : null;
        _playerController.selectedCellWithUnit = null;
    }
    
    public void OnTower2ButtonClick() {
        _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Resources.Load<CellObject>("SuperTower") : null;
        _playerController.selectedCellWithUnit = null;
    }
}
