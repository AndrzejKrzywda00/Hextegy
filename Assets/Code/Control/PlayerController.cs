using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public static int CurrentPlayerId = 1;
    public HexCell selectedCellWithUnit;
    public CellObject prefabFromUI;
    private MoneyManager _moneyManager;
    private HexGrid _hexGrid;
    private Pathfinder _pf;

    public MoneyManager MoneyManager => _moneyManager;

    public HexGrid HexGrid => _hexGrid;

    private void Start() {
        _pf = FindObjectOfType<Pathfinder>();
        _hexGrid = FindObjectOfType<HexGrid>();
        InitializeMoneyManager();
    }

    private void InitializeMoneyManager() {
        _moneyManager = FindObjectOfType<MoneyManager>();
        _moneyManager.SetInitialBalanceOfPlayers(_hexGrid.MapCellsToInitialBalanceOfPlayers());
    }

    public void Handle(HexCell hexCell) {
        if (IsItemFromUISelected()) {
            if (HasEnoughMoneyToBuyEntity()) {
                if (hexCell.IsFriendlyCell()) {
                    if (hexCell.IsEmpty() || hexCell.HasTree()) {
                        HandleBuyingEntityOnFriendlyCell(hexCell);
                        return;
                    }
                } else {
                    if (IsObjectOnCellWeakEnoughToPlaceEntityThere(hexCell) && IsCellBorderingFriendlyCell(hexCell) && IsObjectFromUIUnit()) {
                        HandleBuyingEntityOnNeutralOrEnemyCell(hexCell);
                        return;
                    }
                }
            }
            prefabFromUI = null;
        } else {
            if (IsSomeCellAlreadySelected()) {
                if (CanSelectedObjectMoveInThisTurn() && IsCellInUnitMovementRange(hexCell)) {
                    if (hexCell.IsFriendlyCell()) {
                        if (hexCell.IsEmpty() || hexCell.HasTree()) {
                            HandleMovingUnit(hexCell);
                            return;
                        }
                    } else {
                        if (IsObjectOnCellWeakEnoughToPlaceEntityThere(hexCell) && IsCellBorderingFriendlyCell(hexCell)) {
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

    private bool IsObjectFromUIUnit() {
        return prefabFromUI.Range() > 0;
    }
    
    private bool HasEnoughMoneyToBuyEntity() {
        return _moneyManager.HasEnoughMoneyToBuy(prefabFromUI, CurrentPlayerId);
    }
    
    private bool IsObjectOnCellWeakEnoughToPlaceEntityThere(HexCell hexCell) {
        try {
            CellObject enemyUnit = hexCell.prefabInstance;
            CellObject selectedUnit;
            if (selectedCellWithUnit != null) selectedUnit = selectedCellWithUnit.prefabInstance;
            else selectedUnit = prefabFromUI;
            return enemyUnit.IsWeakerThan(selectedUnit);
        }
        catch (InvalidCastException) {return false;}
    }

    private bool IsCellBorderingFriendlyCell(HexCell hexCell) {
        HexCoordinates[] neighbors = hexCell.NeighborsCoordinates();
        foreach (HexCoordinates coordinates in neighbors) {
            HexCell neighborCell = _hexGrid.CellAtCoordinates(coordinates);
            if (neighborCell != null && neighborCell.IsFriendlyCell()) {
                return true;
            }
        }
        return false;
    }
    
    private void HandleBuyingEntityOnFriendlyCell(HexCell hexCell) {
        _moneyManager.Buy(prefabFromUI, CurrentPlayerId);
        if(hexCell.HasTree()) _moneyManager.IncrementBalance(CurrentPlayerId);
        Destroy(hexCell.prefabInstance.gameObject);
        hexCell.PutOnCell(prefabFromUI);
        prefabFromUI = null;
    }

    private void HandleBuyingEntityOnNeutralOrEnemyCell(HexCell hexCell) {
        HandleBuyingEntityOnFriendlyCell(hexCell);
        hexCell.prefabInstance.SetHasMoveLeftInThisTurn = false;
        _moneyManager.IncrementBalance(CurrentPlayerId);
        AdjustCellColor(hexCell);
    }

    private void AdjustCellColor(HexCell hexCell) {
        hexCell.playerId = CurrentPlayerId;
    }

    private bool IsSomeCellAlreadySelected() {
        return selectedCellWithUnit != null;
    }

    private bool CanSelectedObjectMoveInThisTurn() {
        return selectedCellWithUnit.prefabInstance.CanMoveInThisTurn();
    }
    
    private bool IsCellInUnitMovementRange(HexCell hexCell) {
        HexCoordinates[] path = _pf.OptionalPathFromTo(selectedCellWithUnit, hexCell);
        CellObject unit = (CellObject) selectedCellWithUnit.prefabInstance;
        if (path == null) return false;
        return path.Length - 1 <= unit.Range();
    }

    private void HandleMovingUnit(HexCell hexCell) {
        if(hexCell.IsFriendlyCell() && hexCell.HasTree()) _moneyManager.IncrementBalance(CurrentPlayerId);
        selectedCellWithUnit.prefabInstance.SetHasMoveLeftInThisTurn = false;
        Destroy(hexCell.prefabInstance.gameObject);
        hexCell.prefabInstance = selectedCellWithUnit.prefabInstance;
        hexCell.AlignPrefabInstancePositionWithCellPosition();
        _moneyManager.TransferBalanceOfFieldFromPlayerToPlayer(hexCell.playerId, CurrentPlayerId);
        AdjustCellColor(hexCell);
        selectedCellWithUnit.PutOnCell(Resources.Load<NoElement>("NoElement"));
        selectedCellWithUnit.AlignPrefabInstancePositionWithCellPosition();
        selectedCellWithUnit = null;
    }
}
