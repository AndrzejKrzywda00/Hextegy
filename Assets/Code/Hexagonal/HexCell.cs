using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HexCell : MonoBehaviour {
    
    public HexCoordinates coordinates;
    private Color _color;
    public MonoBehaviour prefab;
    private int _playerId;

    public int PlayerOwnership => _playerId;

    public Vector3 Position => transform.localPosition;

    private Color MapPlayerIdToColor(int playerId) {
        return playerId switch {
            0 => Color.gray,
            _ => Color.red
        };
    }

    public void PutOnCell(MonoBehaviour prefab) {
        this.prefab = prefab;
    }
    
    public bool IsEmpty() {
        return prefab.name == "NoElement";
    }

    public bool HasTree()
    {
        return prefab.name == "Tree";
    }

    public bool HasUnit()
    {
        List<string> unitNames = new List<string>();
        unitNames.Add("CommonKnight");
        unitNames.Add("ExperiencedKnight");
        unitNames.Add("LegendaryKnight");
        return unitNames.Contains(prefab.name);
    }
    
    // ------------------------ ACCESS TYPES ------------------------

    public bool NoConditionAccess()
    {
        return IsEmpty() || HasTree();
    }

    public bool EnemyUnitWeakerAccess(HexCell cell)
    {
        return true;
    }
    
}
