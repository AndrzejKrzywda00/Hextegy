using UnityEngine;

public class PlayerController : MonoBehaviour {

    private int _playerId;
    private int _coins;
    private int _balance;
    public HexCell selectedCellWithUnit;
    public MonoBehaviour prefabFromUI;

    private void Start() {
        _playerId = 1;
    }

    public void Handle(HexCell hexCell) {
        // TODO refactor or sth to look better
        if (prefabFromUI != null && hexCell.IsEmpty()) {
            HandleEntityBuying(hexCell);
        } else if (selectedCellWithUnit == null) {
            if (hexCell.HasUnit()) {
                //cell selected
                selectedCellWithUnit = hexCell;
            }
        } else {
            if (hexCell.Equals(selectedCellWithUnit)) {
                //the same cell unselected
                selectedCellWithUnit = null;
            } else if (hexCell.IsEmpty()) {
                //unit moved to the different empty cell
                HandleMovingUnitOnFriendlyCells(hexCell);
            }
        }
    }

    private void HandleEntityBuying(HexCell hexCell) {
        Destroy(hexCell.prefabInstance.gameObject);
        hexCell.PutOnCell(prefabFromUI);
        prefabFromUI = null;
    }

    private void HandleMovingUnitOnFriendlyCells(HexCell hexCell) {
        (hexCell.prefabInstance, selectedCellWithUnit.prefabInstance) = (selectedCellWithUnit.prefabInstance, hexCell.prefabInstance);
        hexCell.AlignPrefabInstancePositionWithCellPosition();
        hexCell.playerId = selectedCellWithUnit.playerId;       // adding color to the new cell
        selectedCellWithUnit.AlignPrefabInstancePositionWithCellPosition();
        selectedCellWithUnit = null;
    }

    public void EndTurn() {
        _coins += _balance;
        if (_coins <= 0) throw new NoMoneyException();
    }
}
