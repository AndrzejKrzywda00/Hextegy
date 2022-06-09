using System;
using Code.Audio;
using Code.CellObjects;
using Code.CellObjects.Structures.StateBuildings;
using Code.CellObjects.Structures.Towers;
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
        private AudioManager _audioManager;

        public HexGrid HexGrid => _hexGrid;

        private void Start() {
            FindComponentsOnScene();
            InitializeMoneyManager();
        }

        private void FindComponentsOnScene() {
            _audioManager = FindObjectOfType<AudioManager>();
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
                    if (IsFriendlyCellSuitableToPlateObjectThere(hexCell, prefabFromUI)) {
                        PlaySoundWhenBuyingOnFriendlyCell(hexCell);
                        HandleBuyingObjectOnFriendlyCell(hexCell);
                        return;
                    }
                } else {
                    if (IsObjectFromUIUnit() && IsObjectOnEnemyCellWeakEnoughToPlaceUnitThere(hexCell) &&
                        IsCellBorderingFriendlyCell(hexCell) && IsCellProtectionLevelLowerThanUnit(hexCell, prefabFromUI)) {
                        PlaySoundWhenBuyingOrMovingOnEnemyOrNeutralCell(hexCell);
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
                _audioManager.Play(hexCell.prefabInstance is Tree ? SoundNames.DestroyTree.ToString() : SoundNames.Move.ToString());
                HandleMovingUnit(hexCell);
                return true;
            }

            if (!IsObjectOnEnemyCellWeakEnoughToPlaceUnitThere(hexCell) ||
                !IsCellProtectionLevelLowerThanUnit(hexCell, selectedCellWithUnit.prefabInstance) ||
                !IsCellBorderingFriendlyCell(hexCell)) 
                return false;
            PlaySoundWhenBuyingOrMovingOnEnemyOrNeutralCell(hexCell);
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

        private void PlaySoundWhenBuyingOnFriendlyCell(HexCell hexCell) {
            if (hexCell.prefabInstance is Tree) _audioManager.Play(SoundNames.DestroyTree.ToString());
            _audioManager.Play(prefabFromUI is Unit ? SoundNames.ReadyToFight.ToString() : SoundNames.Building.ToString());
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

        private void PlaySoundWhenBuyingOrMovingOnEnemyOrNeutralCell(HexCell hexCell) {
            
            // TODO -- move this to some Sound Manager
            
            switch (hexCell.prefabInstance) {
                case Farm _:
                case TowerTier1 _:
                case TowerTier2 _:
                    _audioManager.Play(SoundNames.DestroyBuilding.ToString());
                    break;
                case Tree _:
                    _audioManager.Play(SoundNames.DestroyTree.ToString());
                    break;
                case Unit _:
                    _audioManager.Play(SoundNames.Death.ToString());
                    break;
                case Capital _:
                    _audioManager.Play(SoundNames.CapitalLost.ToString());
                    break;
                default:
                    _audioManager.Play(SoundNames.Conquest.ToString());
                    break;
            }
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
