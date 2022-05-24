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

    public bool IsNeutral()
    {
        return _playerId == 0;
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

    private bool IsEnemyCell(HexCell cell)
    {
        // different than this and not neutral
        return cell.PlayerOwnership != _playerId && cell.PlayerOwnership != 0;
    }

    private bool IsFriendlyCell(HexCell cell)
    {
        return cell.PlayerOwnership == _playerId;
    }

    private bool HasHouse()
    {
        return prefab.name == "House";
    }
    
    // ------------------------ ACCESS TYPES ------------------------

    public bool NoConditionAccess(HexCell source)
    {
        return IsEmpty() || HasTree() || (HasHouse() && IsEnemyCell(source));
    }

    public bool EnemyUnitWeakerAccess(HexCell source)
    {
        if (IsEnemyCell(source))
        {
            // towers & units are treated likewise here
             IComparable sourceUnit = (IComparable) source.prefab;
             IComparable thisUnit = (IComparable) prefab;
             return thisUnit.IsWeakerThan(sourceUnit);
        }

        return false;
    }

    public bool SamePlayerUnitPromotionAccess(HexCell source)
    {
        if (IsFriendlyCell(source) && this.HasUnit())
        {
            Debug.Log("Here perform promotion");
        }

        return false;
    }

}
