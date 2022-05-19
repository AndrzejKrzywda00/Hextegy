using System;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    private Content _content;
    private Color _color;

    public void CreateFrom(Cell cell)
    {
        _content = cell.content;
        _color = MapPlayerIdToColor(cell.playerId);
    }

    private Color MapPlayerIdToColor(int playerId)
    {
        switch (playerId)
        {
            case 0:
                return Color.white;
            default:
                return Color.black;
        }
    }

}
