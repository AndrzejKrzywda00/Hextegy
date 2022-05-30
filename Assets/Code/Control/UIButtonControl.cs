using UnityEngine;

public class UIButtonControl : MonoBehaviour {
    
    private PlayerController _playerController;
    public string prefabName;

    private void Start() {
        _playerController = FindObjectOfType<PlayerController>();
    }

    public void OnUnitButtonClick() {
        _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Resources.Load<CellObject>(prefabName) : null;
        _playerController.selectedCellWithUnit = null;
    }
    
    public void OnBuildingButtonClick() {
        _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Resources.Load<CellObject>(prefabName) : null;
        _playerController.selectedCellWithUnit = null;
    }
}
