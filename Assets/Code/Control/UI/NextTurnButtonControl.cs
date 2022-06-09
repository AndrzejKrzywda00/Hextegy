using System.Linq;
using Code.CellObjects.Units;
using Code.Control.Game;
using Code.DataAccess;
using Code.Hexagonal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Control.UI {
    public class NextTurnButtonControl : MonoBehaviour {
    
        private PlayerController _playerController;

        private void Start() {
            _playerController = FindObjectOfType<PlayerController>();
        }

        public void EndTurnClick() {
            EndTurn();
            CheckWinConditions();
            SetNextPlayer();
            StartTurn();
            System.GC.Collect();
        }

        private void EndTurn() {
            MoneyManager.CalculateWalletOnTurnEnd();
            if (PlayerController.CurrentPlayerId == Settings.AlivePlayersId.Last()) {
                PlayerController.AddTrees();
            }
            _playerController.ClearNecessaryFieldsAfterEndOfTurn();
            _playerController.CheckAllUnitsForCutoff();
            ChangeCurrentPlayerUnitsMovementPossibilityTo(false);
        }

        private void SetNextPlayer() {
            if (PlayerController.CurrentPlayerId >= Settings.AlivePlayersId.Last()) PlayerController.CurrentPlayerId = 0; 
            int nextPlayerId = 0;
            int i = 0;
            while (nextPlayerId <= PlayerController.CurrentPlayerId) {
                nextPlayerId = Settings.AlivePlayersId[i];
                i++;
            }
            PlayerController.CurrentPlayerId = nextPlayerId;
        }

        private void StartTurn() {
            ChangeCurrentPlayerUnitsMovementPossibilityTo(true);
        }

        private void ChangeCurrentPlayerUnitsMovementPossibilityTo(bool boolean) {
            foreach (HexCell hexCell in _playerController.HexGrid.Cells) {
                if (hexCell == null || !(hexCell.prefabInstance is Unit unit) || !hexCell.IsFriendlyCell()) continue;
                unit.SetHasMoveLeftInThisTurn = boolean;
                hexCell.AlignPrefabInstancePositionWithCellPosition();
            }
        }

        private void CheckWinConditions() {
            if (Settings.AlivePlayersId.Count == 1) {
                Settings.Winner = Settings.AlivePlayersId[0];
                SceneManager.LoadScene("Scenes/Endgame");
            }
        }
    }
}
