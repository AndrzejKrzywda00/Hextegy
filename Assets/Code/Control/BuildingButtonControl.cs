using UnityEngine;

public class BuildingButtonControl : MonoBehaviour {
    public void OnClick() {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.prefabFromUI = Resources.Load<House>("House");
    }
}
