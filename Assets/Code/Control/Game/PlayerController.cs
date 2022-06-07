using System;
using System.Linq;
using Code.CellObjects;
using Code.CellObjects.Units;
using Code.Hexagonal;
using Code.Hexagonal.Algo;
using UnityEngine;
using Tree = Code.CellObjects.Structures.Trees.Tree;

namespace Code.Control.Game {
    public class PlayerController : MonoBehaviour {
    
        public static int CurrentPlayerId = 1;

        public HexCell selectedCellWithUnit;
        public ActiveObject prefabFromUI;
    
        private Pathfinder _pathfinder;

        public static HexGrid HexGrid { get; private set; }

        private void Start() {
            _pathfinder = FindObjectOfType<Pathfinder>();
            HexGrid = FindObjectOfType<HexGrid>();
            PlayerInformation.LoadGrid(HexGrid.Cells);
            InitializeMoneyManager();
        }

        public void ClearNecessaryFieldsAfterEndOfTurn() {
            prefabFromUI = null;
            selectedCellWithUnit = null;
        }
        
        private static void InitializeMoneyManager() {
            MoneyManager.SetInitialBalanceOfPlayers(HexGrid.MapCellsToInitialBalanceOfPlayers());
        }

        public void Handle(HexCell hexCell) {
            if (IsItemFromUISelected()) {
                HandleSomethingIsSelectedFromUI(hexCell);
            } else {
                HandleNothingIsSelectedFromUI(hexCell);
            }
        }

        private void HandleSomethingIsSelectedFromUI(HexCell hexCell) {
            if (HasEnoughMoneyToBuyObject()) {
                if (hexCell.IsFriendlyCell()) {
                    if (IsFriendlyCellSuitableToPlateObjectThere(hexCell, prefabFromUI)) {
                        HandleBuyingObjectOnFriendlyCell(hexCell);
                        return;
                    }
                } else {
                    if (IsObjectFromUIUnit() && IsObjectOnEnemyCellWeakEnoughToPlaceUnitThere(hexCell) &&
                        IsCellBorderingFriendlyCell(hexCell) && IsCellProtectionLevelLowerThanUnit(hexCell, prefabFromUI)) {
                        HandleBuyingObjectOnNeutralOrEnemyCell(hexCell);
                        return;
                    }
                }
            }
            
            prefabFromUI = null;
        }

        private void HandleNothingIsSelectedFromUI(HexCell hexCell) {
            if (IsSomeCellAlreadySelected()) {
                if (HandleWhenCellWithUnitIsSelectedAndNothingFromUI(hexCell))
                    return;
            } else {
                if (IsCellWithPlayersUnitClicked(hexCell)) {
                    selectedCellWithUnit = hexCell;
                    return;
                }
            }

            selectedCellWithUnit = null;
        }

        private bool HandleWhenCellWithUnitIsSelectedAndNothingFromUI(HexCell hexCell) {
            if (!CanSelectedObjectMoveInThisTurn() || !IsCellInUnitMovementRange(hexCell)) 
                return false;
            
            if (hexCell.IsFriendlyCell()) {
                if (!IsFriendlyCellSuitableToPlateObjectThere(hexCell, selectedCellWithUnit.prefabInstance)) 
                    return false;
                HandleMovingUnit(hexCell);
                return true;
            }

            if (!IsObjectOnEnemyCellWeakEnoughToPlaceUnitThere(hexCell) ||
                !IsCellProtectionLevelLowerThanUnit(hexCell, selectedCellWithUnit.prefabInstance) ||
                !IsCellBorderingFriendlyCell(hexCell)) 
                return false;
            HandleMovingUnit(hexCell);
            return true;
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
            CellObject prefabInstance = hexCell.prefabInstance;
            MoneyManager.Buy(prefabFromUI, CurrentPlayerId);
            AdjustBalanceIfDestroyedTreeOnFriendlyCell(hexCell);
            Destroy(hexCell.prefabInstance.gameObject);
            hexCell.PutOnCell(prefabFromUI);
            if (prefabInstance is Tree) MakeUnitNotAbleToMoveInThisTurn(hexCell);
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

        private static bool IsCellBorderingFriendlyCell(HexCell hexCell) {
            HexCoordinates[] neighbors = hexCell.NeighborsCoordinates();
            return neighbors
                .Select(coordinates => HexGrid.CellAtCoordinates(coordinates))
                .Any(neighborCell => neighborCell != null && neighborCell.IsFriendlyCell());
        }

        private static bool IsCellProtectionLevelLowerThanUnit(HexCell hexCell, CellObject unit) {
            HexCell[] neighboringCells = HexGrid.GetNeighborsOfCell(hexCell);
            return neighboringCells
                .Where(neighbor => neighbor != null)
                .Where(neighbor => neighbor.IsEnemyCell() && neighbor.HasProtectiveInstance())
                .All(neighbor => neighbor.prefabInstance.IsWeakerThan(unit));
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

        private static void MakeUnitNotAbleToMoveInThisTurn(HexCell hexCell) {
            if (hexCell.prefabInstance is Unit unit) {
                unit.SetHasMoveLeftInThisTurn = false;
            }
        }

        private static void AdjustCellColor(HexCell hexCell) {
            hexCell.playerId = CurrentPlayerId;
        }

        private bool IsSomeCellAlreadySelected() {
            return selectedCellWithUnit != null;
        }

        private bool CanSelectedObjectMoveInThisTurn() {
            return selectedCellWithUnit.prefabInstance is Unit unit && unit.CanMoveInThisTurn();
        }

        private bool IsCellInUnitMovementRange(HexCell hexCell) {
            Unit unit = (Unit) selectedCellWithUnit.prefabInstance;
            if (selectedCellWithUnit.HexDistanceTo(hexCell) > unit.MovementRange()) return false;
            
            HexCoordinates[] path = _pathfinder.OptionalPathFromTo(selectedCellWithUnit, hexCell);
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

        private static bool IsCellWithPlayersUnitClicked(HexCell hexCell) {
            return hexCell.IsFriendlyCell() && hexCell.HasUnit();
        }

        public static void AddTreesOnEndOfTurnAfterAllPlayersMoved() {
            if (CurrentPlayerId.Equals(HexGrid.NumberOfPlayers))
                HexGrid.GenerateTreesNextToExistingTrees();
        }
    }
}
