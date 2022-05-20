using UnityEngine;

public class CommonKnight : MonoBehaviour {
    public static bool IsSelected = true;

    public static void PutCommonKnightOnCell(HexCell hexCell) {
        CommonKnight commonKnight = Resources.Load<CommonKnight>("CommonKnight");
        hexCell.prefab = commonKnight;
    }
}
