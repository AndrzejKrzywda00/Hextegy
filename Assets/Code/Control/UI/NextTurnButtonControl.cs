using Code.CellObjects.Units;
using Code.Control.Game;
using Code.DataAccess;
using Code.Hexagonal;
using UnityEngine;

namespace Code.Control.UI {
    public class NextTurnButtonControl : MonoBehaviour {
    
        private PlayerController _playerController;

        private void Start() {
            _playerController = FindObjectOfType<PlayerController>();
        }

        public void EndTurnClick() {
            MoneyManager.CalculateWalletOnTurnEnd();
            PlayerController.AddTreesOnEndOfTurnAfterAllPlayersMoved();
            _playerController.ClearNecessaryFieldsAfterEndOfTurn();
            _playerController.CheckAllUnitsForCutoff();
            
            ChangeCurrentPlayerUnitsMovementPossibilityTo(false);

            if (PlayerController.CurrentPlayerId >= Settings.NumberOfPlayers) PlayerController.CurrentPlayerId = 0;

            PlayerController.CurrentPlayerId++;

            ChangeCurrentPlayerUnitsMovementPossibilityTo(true);
            
            System.GC.Collect();
            CheckWinConditions();
        }

        private void ChangeCurrentPlayerUnitsMovementPossibilityTo(bool boolean) {
            foreach (HexCell hexCell in _playerController.HexGrid.Cells) {
                if (hexCell == null || !(hexCell.prefabInstance is Unit unit) || !hexCell.IsFriendlyCell()) continue;
                unit.SetHasMoveLeftInThisTurn = boolean;
                hexCell.AlignPrefabInstancePositionWithCellPosition();
            }
        }

        private void CheckWinConditions() {
            // TODO implement
        }
    }
}
