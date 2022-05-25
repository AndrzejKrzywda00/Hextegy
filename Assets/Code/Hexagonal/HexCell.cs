using System;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
    
    private static readonly List<string> UnitNames = new List<string> {
        "CommonKnight(Clone)",
        "ExperiencedKnight(Clone)",
        "LegendaryKnight(Clone)"
    };
    
    public HexCoordinates coordinates;
    public Color color;
    public MonoBehaviour prefabInstance;
    public int playerId;

    public Vector3 Position => transform.localPosition;

    public void PutOnCell(MonoBehaviour prefab) {
        prefabInstance = Instantiate(prefab);
        AlignPrefabInstancePositionWithCellPosition();
    }

    public void AlignPrefabInstancePositionWithCellPosition() {
        Vector3 position = transform.localPosition;
        position.y += 2; // Here content is raised above grid level to be visible
        prefabInstance.transform.position = position;
    }

    public Color CellColor(HexCell cell) {
        int playerId = cell.playerId;
        return playerId switch {
            0 => Color.gray,
            1 => new Color32(95, 181, 94, 255),         // olive green
            2 => new Color32(182, 92, 120, 255),        // pink
            3 => new Color32(93, 182, 176, 255),        // sky
            4 => new Color32(166, 78, 64, 255),         // tomato
            5 => new Color32(180, 189, 100, 255),       // yellowish
            6 => new Color32(114, 91, 179, 255),        // violet
            _ => throw new ArgumentException()
        };
    }
    
    public bool IsEmpty() {
        return prefabInstance.name == "NoElement";
    }

    public bool IsNeutral() {
        return playerId == 0;
    }

    public bool HasTree() {
        return prefabInstance.name == "Tree";
    }

    public bool HasUnit() {
        return UnitNames.Contains(prefabInstance.name);
    }

    private bool IsEnemyCell(HexCell cell) {
        // different than this and not neutral
        return cell.playerId != playerId && cell.playerId != 0;
    }

    private bool IsFriendlyCell(HexCell cell) {
        return cell.playerId == playerId;
    }

    private bool HasHouse() {
        return prefabInstance.name == "House";
    }
    
    // ------------------------ ACCESS TYPES ------------------------

    public bool NoConditionAccess(HexCell source) {
        return IsEmpty() || HasTree() || (HasHouse() && IsEnemyCell(source));
    }

    public bool EnemyUnitWeakerAccess(HexCell source) {
        if (!IsEnemyCell(source)) return false;
        
        // towers & units are treated likewise here
        IComparable sourceUnit = (IComparable) source.prefabInstance;
        IComparable thisUnit = (IComparable) prefabInstance;
        return thisUnit.IsWeakerThan(sourceUnit);
    }

    public bool SamePlayerUnitPromotionAccess(HexCell source) {
        if (IsFriendlyCell(source) && HasUnit()) {
            Debug.Log("Here perform promotion");
        }
        return false;
    }
}
