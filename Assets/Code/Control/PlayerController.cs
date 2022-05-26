using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public static int CurrentPlayerId = 1;
    public HexCell selectedCellWithUnit;
    public MonoBehaviour prefabFromUI;
    private HexGrid _grid;
        
    private int _coins;
    private int _balance;
    
    private void Start() {
        _grid = FindObjectOfType<HexGrid>();
        _coins = 10;
        _balance = 0;
    }

    public void Handle(HexCell hexCell) {
        if (IsItemFromUISelected()) {
            if (hexCell.IsFriendlyCell()) {
                if (hexCell.IsEmpty() && HasEnoughMoneyToBuyEntity()) {
                    HandleBuyingEntityOnFriendlyCell(hexCell);
                    return;
                }
            } else {
                if (IsObjectOnCellWeakEnoughToMoveUnitThere(hexCell) && IsCellBorderingFriendlyCell(hexCell)) {
                    HandleBuyingEntityOnNeutralOrEnemyCell(hexCell);
                    return;
                }
            }
            prefabFromUI = null;
        } else {
            if (IsSomeCellAlreadySelected()) {
                if (IsCellInUnitMovementRange(hexCell)) {
                    if (hexCell.IsFriendlyCell()) {
                        if (hexCell.IsEmpty() || hexCell.HasTree()) {
                            HandleMovingUnit(hexCell);
                            return;
                        }
                    } else {
                        if (IsObjectOnCellWeakEnoughToMoveUnitThere(hexCell)) {
                            HandleMovingUnit(hexCell);
                            return;
                        }
                    }
                }
            } else {
                if (hexCell.IsFriendlyCell() && hexCell.HasUnit()) {
                    selectedCellWithUnit = hexCell;
                    return;
                }
            }
            selectedCellWithUnit = null;
        }
    }
    
    private bool IsItemFromUISelected() {
        return prefabFromUI != null;
    }
    
    private bool HasEnoughMoneyToBuyEntity() {
        //TODO implement checking amount of money when buying
        return true;
    }
    
    private bool IsObjectOnCellWeakEnoughToMoveUnitThere(HexCell hexCell) {
        //TODO implement checking if object on non friendly cell is weak enough to move our unit there
        return true;
    }
    
    private bool IsCellBorderingFriendlyCell(HexCell hexCell) {
        HexCoordinates[] neighbors = hexCell.NeighborsCoordinates();
        foreach (HexCoordinates coordinates in neighbors)
        {
            HexCell neighborCell = _grid.CellAtCoordinates(coordinates);
            if (neighborCell == null) continue;
            if (neighborCell.playerId == CurrentPlayerId) return true;
        }
        return false;
    }
    
    private void HandleBuyingEntityOnFriendlyCell(HexCell hexCell) {
        Destroy(hexCell.prefabInstance.gameObject);
        hexCell.PutOnCell(prefabFromUI);
        prefabFromUI = null;
    }

    private void HandleBuyingEntityOnNeutralOrEnemyCell(HexCell hexCell) {
        HandleBuyingEntityOnFriendlyCell(hexCell);
        AdjustCellColor(hexCell);
    }

    private bool IsSomeCellAlreadySelected() {
        return selectedCellWithUnit != null;
    }

    private bool IsCellInUnitMovementRange(HexCell hexCell) {
        //TODO implement checking if cell is in unit movement range
        return true;
    }

    private void HandleMovingUnit(HexCell hexCell) {
        Destroy(hexCell.prefabInstance.gameObject);
        hexCell.prefabInstance = selectedCellWithUnit.prefabInstance;
        hexCell.AlignPrefabInstancePositionWithCellPosition();
        AdjustCellColor(hexCell);
        selectedCellWithUnit.PutOnCell(Resources.Load<NoElement>("NoElement"));
        selectedCellWithUnit.AlignPrefabInstancePositionWithCellPosition();
        selectedCellWithUnit = null;
    }

    private void AdjustCellColor(HexCell hexCell) {
        hexCell.playerId = selectedCellWithUnit.playerId;
    }
}
