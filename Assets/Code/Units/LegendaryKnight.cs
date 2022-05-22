using UnityEngine;

public class LegendaryKnight : MonoBehaviour {
    
    public static void PutLegendaryKnightOnCell(HexCell cell) {
        LegendaryKnight legendaryKnight = Resources.Load<LegendaryKnight>("LegendaryKnight");
        cell.prefab = legendaryKnight;
    }
}
