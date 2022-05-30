using UnityEngine;

public class NextTurnButtonControl : MonoBehaviour {

    public void OnClick() {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.MoneyManager.CalculateWalletOnTurnEnd();

        if (PlayerController.CurrentPlayerId >= HexGrid.NumberOfPlayers)
            PlayerController.CurrentPlayerId = 0;
        
        PlayerController.CurrentPlayerId++;

        foreach (HexCell hexCell in playerController.HexGrid.Cells) {
            if (hexCell != null && hexCell.prefabInstance is Unit unit)
                unit.SetHasMoveLeftInThisTurn = true;
        }

        PlayerController.AddTreesOnEndOfTurn();
    }
}
