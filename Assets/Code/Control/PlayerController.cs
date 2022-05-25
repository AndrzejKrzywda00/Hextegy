using UnityEngine;

public class PlayerController : MonoBehaviour {

    private int _coins;
    private int _balance;
    private HexCell _selectedCellWithUnit;

    public int Balance => _balance;
    public int Coins => _coins;

    public void Handle(HexCell cell) {
        // TODO refactor or sth to look better
        if (_selectedCellWithUnit == null) {
             if (cell.HasUnit()) {
                 //cell selected
                 _selectedCellWithUnit = cell;
             }
        } else {
            if (cell.Equals(_selectedCellWithUnit)) {
                //the same cell unselected
                _selectedCellWithUnit = null;
            } else if (cell.prefabInstance.name.Equals("NoElement(Clone)")) {
                //unit moved to the different empty cell
                (cell.prefabInstance, _selectedCellWithUnit.prefabInstance) = (_selectedCellWithUnit.prefabInstance, cell.prefabInstance);
                cell.AlignPrefabInstancePositionWithCellPosition();
                _selectedCellWithUnit.AlignPrefabInstancePositionWithCellPosition();
                _selectedCellWithUnit = null;
            }
        }
    }

    public void EndTurn() {
        _coins += _balance;
        if (_coins <= 0) throw new NoMoneyException();
    }
}
