using UnityEngine;

public class ExperiencedKnight : MonoBehaviour {
    
    public static void PutOnCell(HexCell cell) {
        ExperiencedKnight experiencedKnight = Resources.Load<ExperiencedKnight>("ExperiencedKnight");
        cell.prefab = experiencedKnight;
    }
}
