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
            1 => new Color32(95, 181, 94, 255),                 // olive green
            2 => new Color32(182, 92, 120, 255),                // pink
            3 => new Color32(93, 182, 176, 255),                // sky
            4 => new Color32(166, 78, 64, 255),                 // tomato
            5 => new Color32(180, 189, 100, 255),               // yellowish
            6 => new Color32(114, 91, 179, 255),                // violet
            
            101 => new Color32(95-40, 181-40, 94-40, 255),      // dark olive green
            102 => new Color32(182-40, 92-40, 120-40, 255),     // dark pink
            103 => new Color32(93-40, 182-40, 176-40, 255),     // dark sky
            104 => new Color32(166-40, 78-40, 64-40, 255),      // dark tomato
            105 => new Color32(180-40, 189-40, 100-40, 255),    // dark yellowish
            106 => new Color32(114-40, 91-40, 179-40, 255),     // dark violet
            
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
        return playerId != PlayerController.CurrentPlayerId && playerId != 0;
    }

    public int HexDistanceTo(HexCell hexCell) {
        return coordinates.HexDistanceTo(hexCell.coordinates);
    }

    public float GaussianDistanceTo(HexCell hexCell) {
        return coordinates.GaussianDistanceTo(hexCell.coordinates);
    }

    public bool IsFriendlyCell() {
        return playerId == PlayerController.CurrentPlayerId;
    }
}
