using System;
using Code.CellObjects;
using UnityEngine;

namespace Code.Control.Game {
    public class PlayerController : MonoBehaviour {
    
        public static int CurrentPlayerId = 1;
        public HexCell selectedCellWithUnit;
        public CellObject prefabFromUI;
    
        private Pathfinder _pathfinder;
        private static HexGrid _hexGrid;
    
        public HexGrid HexGrid => _hexGrid;

        private void Start() {
            _pathfinder = FindObjectOfType<Pathfinder>();
            _hexGrid = FindObjectOfType<HexGrid>();
            InitializeMoneyManager();
        }

        private void InitializeMoneyManager() {
            MoneyManager.SetInitialBalanceOfPlayers(_hexGrid.MapCellsToInitialBalanceOfPlayers());
        }

        public void Handle(HexCell hexCell) {
            if (IsItemFromUISelected()) {
                if (HasEnoughMoneyToBuyEntity()) {
                    if (hexCell.IsFriendlyCell()) {
                        if (IsFriendlyCellSuitableToPlateObjectThere(hexCell)) {
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
                            if (IsFriendlyCellSuitableToPlateObjectThere(hexCell)) {
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
                    if (IsCellWithPlayersUnitClicked(hexCell)) {
                        selectedCellWithUnit = hexCell;
                        return;
                    }
                }
                selectedCellWithUnit = null;
            }
        }

        public static void AddTreesOnEndOfTurn() {
            _hexGrid.GenerateTreesNextToExistingTrees();
        }

        private bool IsItemFromUISelected() {
            return prefabFromUI != null;
        }

        private bool HasEnoughMoneyToBuyEntity() {
            return MoneyManager.HasEnoughMoneyToBuy(prefabFromUI, CurrentPlayerId);
        }

        private static bool IsFriendlyCellSuitableToPlateObjectThere(HexCell hexCell) {
            return hexCell.IsEmpty() || hexCell.HasTree();
        }

        private void HandleBuyingEntityOnFriendlyCell(HexCell hexCell) {
            MoneyManager.Buy(prefabFromUI, CurrentPlayerId);
            if(hexCell.HasTree()) MoneyManager.IncrementBalance(CurrentPlayerId);
            Destroy(hexCell.prefabInstance.gameObject);
            hexCell.PutOnCell(prefabFromUI);
            prefabFromUI = null;
        }

        private bool IsObjectOnCellWeakEnoughToPlaceEntityThere(HexCell hexCell) {
            try {
                CellObject enemyUnit = hexCell.prefabInstance;
                CellObject selectedUnit = selectedCellWithUnit != null ? selectedCellWithUnit.prefabInstance : prefabFromUI;
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

        private bool IsObjectFromUIUnit() {
            return prefabFromUI is Unit;
        }

        private void HandleBuyingEntityOnNeutralOrEnemyCell(HexCell hexCell) {
            HandleBuyingEntityOnFriendlyCell(hexCell);
            hexCell.prefabInstance.SetHasMoveLeftInThisTurn = false;
            MoneyManager.IncrementBalance(CurrentPlayerId);
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
            HexCoordinates[] path = _pathfinder.OptionalPathFromTo(selectedCellWithUnit, hexCell);
            CellObject unit = selectedCellWithUnit.prefabInstance;
            if (path == null) return false;
            return path.Length - 1 <= unit.Range();
        }

        private void HandleMovingUnit(HexCell hexCell) {
            AdjustBalanceIfDestroyedTreeOnFriendlyCell(hexCell);
            selectedCellWithUnit.prefabInstance.SetHasMoveLeftInThisTurn = false;
            Destroy(hexCell.prefabInstance.gameObject);
            hexCell.prefabInstance = selectedCellWithUnit.prefabInstance;
            hexCell.AlignPrefabInstancePositionWithCellPosition();
            MoneyManager.TransferBalanceOfFieldFromPlayerToPlayer(hexCell.playerId, CurrentPlayerId);
            AdjustCellColor(hexCell);
            selectedCellWithUnit.PutOnCell(Prefabs.GetNoElement());
            selectedCellWithUnit.AlignPrefabInstancePositionWithCellPosition();
            selectedCellWithUnit = null;
        }

        private static void AdjustBalanceIfDestroyedTreeOnFriendlyCell(HexCell hexCell) {
            if (hexCell.IsFriendlyCell() && hexCell.HasTree()) MoneyManager.IncrementBalance(CurrentPlayerId);
        }

        private bool IsCellWithPlayersUnitClicked(HexCell hexCell) {
            return hexCell.IsFriendlyCell() && hexCell.HasUnit();
        }
    }
}
