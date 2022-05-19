using UnityEngine;

public class HexCell : MonoBehaviour {
    public HexCoordinates coordinates;
    private Color _color;
    public MonoBehaviour prefab;

    public Vector3 Position => transform.localPosition;

    private Color MapPlayerIdToColor(int playerId) {
        return playerId switch {
            0 => Color.gray,
            _ => Color.red
        };
    }
}
