using UnityEngine;

public class UnitButtonControl : MonoBehaviour
{
    public void OnClick() {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.prefabFromUI = Resources.Load<CommonKnight>("CommonKnight");
    }
}
