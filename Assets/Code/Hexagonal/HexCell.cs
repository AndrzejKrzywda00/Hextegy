using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    private Content _content;

    public void CreateFrom(Content content)
    {
        _content = content;
    }
    

}
