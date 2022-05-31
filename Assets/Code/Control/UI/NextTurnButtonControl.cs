using Code.CellObjects.Units;
using Code.Control.Game;
using UnityEngine;

namespace Code.Control.UI {
    public class NextTurnButtonControl : MonoBehaviour {
    
        private PlayerController _playerController;

        private void Start() {
            _playerController = FindObjectOfType<PlayerController>();
        }

        public void OnClick() {
            MoneyManager.CalculateWalletOnTurnEnd();
            PlayerController.AddTreesOnEndOfTurnAfterAllPlayersMoved();

            ChangeCurrentPlayerUnitsMovementPossibilityTo(false);

            if (PlayerController.CurrentPlayerId >= HexGrid.NumberOfPlayers)
                PlayerController.CurrentPlayerId = 0;

            PlayerController.CurrentPlayerId++;

            ChangeCurrentPlayerUnitsMovementPossibilityTo(true);
        }

        private void ChangeCurrentPlayerUnitsMovementPossibilityTo(bool boolean) {
            foreach (HexCell hexCell in _playerController.HexGrid.Cells) {
                if (hexCell == null || !(hexCell.prefabInstance is Unit unit) || !hexCell.IsFriendlyCell()) continue;
                unit.SetHasMoveLeftInThisTurn = boolean;
                hexCell.AlignPrefabInstancePositionWithCellPosition();
            }
        }
    }
}
