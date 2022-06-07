using Code.CellObjects.Units;
using Code.Control.Game;
using Code.Hexagonal;
using UnityEngine;

namespace Code.Control.UI {
    public class NextTurnButtonControl : MonoBehaviour {
    
        private PlayerController _playerController;

        private void Start() {
            _playerController = FindObjectOfType<PlayerController>();
        }

        public void EndTurn() {
            MoneyManager.CalculateWalletOnTurnEnd();
            PlayerController.AddTreesOnEndOfTurnAfterAllPlayersMoved();
            _playerController.ClearNecessaryFieldsAfterEndOfTurn();
            
            ChangeCurrentPlayerUnitsMovementPossibilityTo(false);

            if (PlayerController.CurrentPlayerId >= HexGrid.NumberOfPlayers) PlayerController.CurrentPlayerId = 0;

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
