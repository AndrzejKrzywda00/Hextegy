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

    public void PutOnCell(MonoBehaviour prefab) {
        prefabInstance = Instantiate(prefab);
        AlignPrefabInstancePositionWithCellPosition();
    }

    public void AlignPrefabInstancePositionWithCellPosition() {
        Vector3 position = transform.localPosition;
        position.y += 2; // Here content is raised above grid level to be visible
        prefabInstance.transform.position = position;
    }

    public Color GetCellColor() {
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

    public HexCoordinates[] NeighborsCoordinates()
    {
        return coordinates.Neighbors();
    }
    
    public bool IsEmpty() {
        return prefabInstance.name.Equals("NoElement(Clone)");
    }

    public bool IsNeutral() {
        return playerId == 0;
    }

    public bool HasTree() {
        return prefabInstance.name.Equals("Tree(Clone)");
    }

    public bool HasHouse() {
        return prefabInstance.name.Equals("House(Clone)");
    }

    public bool HasTower() {
        return prefabInstance.name.Equals("NormalTower(Clone)") || prefabInstance.name.Equals("SuperTower(Clone)");
    }
    
    public bool HasUnit() {
        return UnitNames.Contains(prefabInstance.name);
    }

    public bool IsEnemyCell() {
        // different than this and not neutral
        return playerId != PlayerController.CurrentPlayerId && playerId != 0;
    }

    public bool IsFriendlyCell() {
        return playerId == PlayerController.CurrentPlayerId;
    }

    // ------------------------ ACCESS TYPES ------------------------

    public bool NoConditionAccess() {
        return IsEmpty() || HasTree() || (HasHouse() && IsEnemyCell());
    }

    public bool EnemyUnitWeakerAccess(HexCell source) {
        if (!source.IsEnemyCell()) return false;
        
        // towers & units are treated likewise here
        CellObject sourceUnit = (CellObject) source.prefabInstance;
        CellObject thisUnit = (CellObject) prefabInstance;
        return thisUnit.IsWeakerThan(sourceUnit);
    }

    public bool SamePlayerUnitPromotionAccess(HexCell source) {
        if (source.IsFriendlyCell() && HasUnit()) {
            Debug.Log("Here perform promotion");
        }
        return false;
    }
}
