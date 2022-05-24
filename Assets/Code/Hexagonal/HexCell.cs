using System;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
    
    public HexCoordinates coordinates;
    public Color color;
    public MonoBehaviour prefab;
    public int playerId;

    public Vector3 Position => transform.localPosition;

    public void PutOnCell(MonoBehaviour prefab) {
        this.prefab = prefab;
    }

    public Color CellColor(HexCell cell) {
        int playerId = cell.playerId;
        return playerId switch {
            0 => Color.gray,
            1 => Color.blue,
            2 => Color.cyan,
            3 => Color.green,
            4 => Color.red,
            5 => Color.yellow,
            6 => Color.white,
            _ => throw new ArgumentException()
        };
    }
    
    public bool IsEmpty() {
        return prefab.name == "NoElement";
    }

    public bool IsNeutral() {
        return playerId == 0;
    }

    public bool HasTree() {
        return prefab.name == "Tree";
    }

    public bool HasUnit() {
        List<string> unitNames = new List<string>();
        unitNames.Add("CommonKnight");
        unitNames.Add("ExperiencedKnight");
        unitNames.Add("LegendaryKnight");
        return unitNames.Contains(prefab.name);
    }

    private bool IsEnemyCell(HexCell cell) {
        // different than this and not neutral
        return cell.playerId != playerId && cell.playerId != 0;
    }

    private bool IsFriendlyCell(HexCell cell) {
        return cell.playerId == playerId;
    }

    private bool HasHouse() {
        return prefab.name == "House";
    }
    
    // ------------------------ ACCESS TYPES ------------------------

    public bool NoConditionAccess(HexCell source) {
        return IsEmpty() || HasTree() || (HasHouse() && IsEnemyCell(source));
    }

    public bool EnemyUnitWeakerAccess(HexCell source) {
        if (!IsEnemyCell(source)) return false;
        
        // towers & units are treated likewise here
        IComparable sourceUnit = (IComparable) source.prefab;
        IComparable thisUnit = (IComparable) prefab;
        return thisUnit.IsWeakerThan(sourceUnit);
    }

    public bool SamePlayerUnitPromotionAccess(HexCell source) {
        if (IsFriendlyCell(source) && this.HasUnit())
        {
            Debug.Log("Here perform promotion");
        }
        return false;
    }
}
