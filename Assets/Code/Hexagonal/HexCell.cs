using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    private Color _color;

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    private Color MapPlayerIdToColor(int playerId)
    {
        switch (playerId)
        {
            case 0:
                return Color.gray;
            default:
                return Color.red;
        }
    }

}
