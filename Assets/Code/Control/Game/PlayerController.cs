using System;
using Code.Audio;
using Code.CellObjects;
using Code.CellObjects.Units;
using Code.DataAccess;
using Code.Hexagonal;
using Code.Hexagonal.Algo;
using UnityEngine;
using Tree = Code.CellObjects.Structures.Trees.Tree;

namespace Code.Control.Game {
    public class PlayerController : MonoBehaviour {
    
        public static int CurrentPlayerId = 1;
        private static HexGrid _hexGrid;
        
        public HexCell selectedCellWithUnit;
        public ActiveObject prefabFromUI;
    
        private Pathfinder _pathfinder;

        public HexGrid HexGrid => _hexGrid;

        private void Start() {
            CurrentPlayerId = 1;
            FindComponentsOnScene();
            InitializeMoneyManager();
        }

        private void FindComponentsOnScene() {
            _pathfinder = FindObjectOfType<Pathfinder>();
            _hexGrid = FindObjectOfType<HexGrid>();
        }
        
        private static void InitializeMoneyManager() {
            MoneyManager.SetInitialBalanceOfPlayers(_hexGrid.MapCellsToInitialBalanceOfPlayers()); 
        }

        public void ClearNecessaryFieldsAfterEndOfTurn() {
            prefabFromUI = null;
            selectedCellWithUnit = null;
        }

        public void CheckAllUnitsForCutoff() {
            HexCell[] allUnitsOnGrid = _hexGrid.GetAllUnits();
            foreach (HexCell cellWithUnit in allUnitsOnGrid) {
                if (!PathExistsFromUnitToItsCapital(cellWithUnit, PlayersInfo.GetPlayerCapital(cellWithUnit.playerId))) {
                    HexGrid.DestroyUnitAndPlaceGrave(cellWithUnit);
                }
            }
        }

        public static void DestroyAllUnitsOfPlayer() {
            _hexGrid.DestroyUnitsOfActivePlayer();
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
                    if (IsFriendlyCellSuitableToPlaceObjectThere(hexCell)) {
                        AudioManager.PlaySoundWhenBuyingOnFriendlyCell(hexCell, prefabFromUI);
                        HandleBuyingObjectOnFriendlyCell(hexCell);
                        return;
                    }
                } else {
                    if (IsObjectFromUIUnit() && IsObjectOnEnemyOrNeutralCellWeakEnoughToPlaceUnitThere(hexCell) &&
                        IsCellBorderingFriendlyCell(hexCell) && IsCellProtectionLevelLowerThanUnit(hexCell, prefabFromUI)) {
                        AudioManager.PlaySoundWhenBuyingOnEnemyOrNeutralCell(hexCell, prefabFromUI);
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
                    AudioManager.PlaySoundWhenSelectingUnit(selectedCellWithUnit);
                    return;
                }
            }

            selectedCellWithUnit = null;
        }

        private bool HandleWhenCellWithUnitIsSelectedAndNothingFromUI(HexCell hexCell) {
            if (!CanSelectedObjectMoveInThisTurn() || !IsCellInUnitMovementRange(hexCell)) 
                return false;
            
            if (hexCell.IsFriendlyCell()) {
                if (!IsFriendlyCellSuitableToPlaceObjectThere(hexCell)) 
                    return false;
                AudioManager.PlaySoundWhenMovingOnFriendlyCells(hexCell);
                HandleMovingUnit(hexCell);
                return true;
            }

            if (!IsObjectOnEnemyOrNeutralCellWeakEnoughToPlaceUnitThere(hexCell) ||
                !IsCellProtectionLevelLowerThanUnit(hexCell, selectedCellWithUnit.prefabInstance) ||
                !IsCellBorderingFriendlyCell(hexCell)) 
                return false;
            AudioManager.PlaySoundWhenMovingOnEnemyOrNeutralCell(hexCell);
            HandleMovingUnit(hexCell);
            return true;
        }

        private bool IsItemFromUISelected() {
            return prefabFromUI != null;
        }

        private bool HasEnoughMoneyToBuyObject() {
            return MoneyManager.HasEnoughMoneyToBuy(prefabFromUI, CurrentPlayerId);
        }

        private static bool IsFriendlyCellSuitableToPlaceObjectThere(HexCell hexCell) {
            return hexCell.IsEmpty() || hexCell.HasTree();
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

        private bool IsObjectOnEnemyOrNeutralCellWeakEnoughToPlaceUnitThere(HexCell hexCell) {
            try {
                if (hexCell.IsNeutral()) return true;
                CellObject enemyObject = hexCell.prefabInstance;
                CellObject selectedUnit = selectedCellWithUnit != null ? selectedCellWithUnit.prefabInstance : prefabFromUI;
                return enemyObject.IsWeakerThan(selectedUnit);
            } catch (InvalidCastException) {
                return false;
            }
        }

        private static bool IsCellBorderingFriendlyCell(HexCell hexCell) {
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
            if (hexCell.IsNeutral()) return true;
            HexCell[] neighboringCells = _hexGrid.GetNeighborsOfCell(hexCell);
            foreach (HexCell neighbor in neighboringCells) {
                if (neighbor == null) continue;
                if (!neighbor.IsEnemyCell() || !neighbor.HasProtectiveInstance()) continue;
                if (!neighbor.prefabInstance.IsWeakerThan(unit)) {
                    return false;
                }
            }

            return true;
        }

        private bool IsObjectFromUIUnit() {
            return prefabFromUI is Unit;
        }

        private void HandleBuyingObjectOnNeutralOrEnemyCell(HexCell hexCell) {
            if (hexCell.prefabInstance is ActiveObject && hexCell.IsEnemyCell()) {
                HandleBalanceChanges(hexCell);
            } else {
                MoneyManager.IncrementBalance(CurrentPlayerId);
            }
            HandleBuyingObjectOnFriendlyCell(hexCell);
            MakeUnitNotAbleToMoveInThisTurn(hexCell);
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
            HandleBalanceChanges(hexCell);
            MakeUnitNotAbleToMoveInThisTurn(selectedCellWithUnit);
            Destroy(hexCell.prefabInstance.gameObject);
            hexCell.prefabInstance = selectedCellWithUnit.prefabInstance;
            hexCell.AlignPrefabInstancePositionWithCellPosition();
            AdjustCellColor(hexCell);
            PutEmptyElementOnGrid();
        }

        private static void HandleBalanceChanges(HexCell hexCell) {
            MoneyManager.TransferBalanceOfFieldFromPlayerToPlayer(hexCell.playerId, CurrentPlayerId);
            HandleChangingBalanceWhenActiveObjectDestroyed(hexCell);
        }

        private static void HandleChangingBalanceWhenActiveObjectDestroyed(HexCell hexCell) {
            if (!hexCell.IsEnemyCell() || !hexCell.HasActiveInstance()) return;
            ActiveObject unit = (ActiveObject) hexCell.prefabInstance;
            MoneyManager.IncrementBalanceOfPlayerByAmount(hexCell.playerId, unit.MaintenanceCost());
            if (hexCell.HasFarm()) MoneyManager.DecrementPlayerFarms(hexCell.playerId);
        }

        private void PutEmptyElementOnGrid() {
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

        public static void AddTrees() {
            _hexGrid.GenerateTreesNextToExistingTrees();
        }

        private bool PathExistsFromUnitToItsCapital(HexCell unitCell, HexCell capitalCell) {
            HexCoordinates[] path = _pathfinder.ConsistentPathFromTo(unitCell, capitalCell);
            return path != null;
        }
    }
}
