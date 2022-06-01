using System;
using Code.CellObjects;
using Code.CellObjects.Units;
using Code.Hexagonal;
using Code.Hexagonal.Algo;
using UnityEngine;

namespace Code.Control.Game {
    public class PlayerController : MonoBehaviour {
    
        public static int CurrentPlayerId = 1;
        private static HexGrid _hexGrid;
        
        public HexCell selectedCellWithUnit;
        public ActiveObject prefabFromUI;
    
        private Pathfinder _pathfinder;
        private PlayerManager _playerManager;

        public HexGrid HexGrid => _hexGrid;

        private void Start() {
            _pathfinder = FindObjectOfType<Pathfinder>();
            _hexGrid = FindObjectOfType<HexGrid>();
            _playerManager = new PlayerManager(_hexGrid.Cells);
            InitializeMoneyManager();
        }

        private void InitializeMoneyManager() {
            MoneyManager.SetInitialBalanceOfPlayers(_hexGrid.MapCellsToInitialBalanceOfPlayers());
        }

        public void Handle(HexCell hexCell) {
            if (IsItemFromUISelected()) {
                if (HasEnoughMoneyToBuyObject()) {
                    if (hexCell.IsFriendlyCell()) {
                        if (IsFriendlyCellSuitableToPlateObjectThere(hexCell, prefabFromUI)) {
                            HandleBuyingObjectOnFriendlyCell(hexCell);
                            return;
                        }
                    } else {
                        if (IsObjectFromUIUnit() && IsObjectOnEnemyCellWeakEnoughToPlaceUnitThere(hexCell) && IsCellBorderingFriendlyCell(hexCell) && IsCellProtectionLevelLowerThanUnit(hexCell, prefabFromUI)) {
                            HandleBuyingObjectOnNeutralOrEnemyCell(hexCell);
                            return;
                        }
                    }
                }
                prefabFromUI = null;
            } else {
                if (IsSomeCellAlreadySelected()) {
                    if (CanSelectedObjectMoveInThisTurn() && IsCellInUnitMovementRange(hexCell)) {
                        if (hexCell.IsFriendlyCell()) {
                            if (IsFriendlyCellSuitableToPlateObjectThere(hexCell, selectedCellWithUnit.prefabInstance)) {
                                HandleMovingUnit(hexCell);
                                return;
                            }
                        } else {
                            if (IsCellProtectionLevelLowerThanUnit(hexCell, selectedCellWithUnit.prefabInstance) && IsObjectOnEnemyCellWeakEnoughToPlaceUnitThere(hexCell) && IsCellBorderingFriendlyCell(hexCell)) {
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

        private bool IsItemFromUISelected() {
            return prefabFromUI != null;
        }

        private bool HasEnoughMoneyToBuyObject() {
            return MoneyManager.HasEnoughMoneyToBuy(prefabFromUI, CurrentPlayerId);
        }

        private static bool IsFriendlyCellSuitableToPlateObjectThere(HexCell hexCell, CellObject unit) {
            return (hexCell.IsEmpty() || hexCell.HasTree()) && IsCellProtectionLevelLowerThanUnit(hexCell, unit);
        }

        private void HandleBuyingObjectOnFriendlyCell(HexCell hexCell) {
            MoneyManager.Buy(prefabFromUI, CurrentPlayerId);
            AdjustBalanceIfDestroyedTreeOnFriendlyCell(hexCell);
            Destroy(hexCell.prefabInstance.gameObject);
            hexCell.PutOnCell(prefabFromUI);
            prefabFromUI = null;
        }

        private bool IsObjectOnEnemyCellWeakEnoughToPlaceUnitThere(HexCell hexCell) {
            try {
                CellObject enemyObject = hexCell.prefabInstance;
                CellObject selectedUnit = selectedCellWithUnit != null ? selectedCellWithUnit.prefabInstance : prefabFromUI;
                return enemyObject.IsWeakerThan(selectedUnit);
            } catch (InvalidCastException) {
                return false;
            }
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

        private static bool IsCellProtectionLevelLowerThanUnit(HexCell hexCell, CellObject unit) {
            HexCell[] neighboringCells = _hexGrid.GetNeighborsOfCell(hexCell);
            foreach (HexCell neighbor in neighboringCells) {
                if (neighbor == null) continue;
                if (neighbor.IsEnemyCell() && neighbor.HasTower()) {
                    if (!neighbor.prefabInstance.IsWeakerThan(unit)) {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsObjectFromUIUnit() {
            return prefabFromUI is Unit;
        }

        private void HandleBuyingObjectOnNeutralOrEnemyCell(HexCell hexCell) {
            HandleBuyingObjectOnFriendlyCell(hexCell);
            MakeUnitNotAbleToMoveInThisTurn(hexCell);
            MoneyManager.IncrementBalance(CurrentPlayerId);
            AdjustCellColor(hexCell);
        }

        private void MakeUnitNotAbleToMoveInThisTurn(HexCell hexCell) {
            if (hexCell.prefabInstance is Unit unit) {
                unit.SetHasMoveLeftInThisTurn = false;
            }
        }

        private void AdjustCellColor(HexCell hexCell) {
            hexCell.playerId = CurrentPlayerId;
        }

        private bool IsSomeCellAlreadySelected() {
            return selectedCellWithUnit != null;
        }

        private bool CanSelectedObjectMoveInThisTurn() {
            return selectedCellWithUnit.prefabInstance is Unit unit && unit.CanMoveInThisTurn();
        }

        private bool IsCellInUnitMovementRange(HexCell hexCell) {
            HexCoordinates[] path = _pathfinder.OptionalPathFromTo(selectedCellWithUnit, hexCell);
            Unit unit = (Unit) selectedCellWithUnit.prefabInstance;
            if (path == null) return false;
            return path.Length - 1 <= unit.MovementRange();
        }

        private void HandleMovingUnit(HexCell hexCell) {
            AdjustBalanceIfDestroyedTreeOnFriendlyCell(hexCell);
            MakeUnitNotAbleToMoveInThisTurn(selectedCellWithUnit);
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

        public static void AddTreesOnEndOfTurnAfterAllPlayersMoved() {
            if (CurrentPlayerId.Equals(HexGrid.NumberOfPlayers))
                _hexGrid.GenerateTreesNextToExistingTrees();
        }
    }
}
