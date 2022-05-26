using System;
using UnityEngine;

public class UIButtonControl : MonoBehaviour {
    private PlayerController _playerController;

    private void Start() {
        _playerController = FindObjectOfType<PlayerController>();
    }

    public void OnUnitButtonClick() {
        _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Resources.Load<CommonKnight>("CommonKnight") : null;
        _playerController.selectedCellWithUnit = null;
    }
    
    public void OnBuildingButtonClick() {
        _playerController.prefabFromUI = _playerController.prefabFromUI == null ? Resources.Load<House>("House") : null;
        _playerController.selectedCellWithUnit = null;
    }
}
