using Code.Generator;
using UnityEngine;

public class Cell {
    
    public readonly Coordinates Coordinates;
    public MonoBehaviour Prefab;
    public int PlayerId;
    public bool IsMainLand;

    public Cell(int x, int y) {
        Coordinates = new Coordinates(x, y);

        Prefab = Prefabs.getNoElement();
        PlayerId = 0;
        IsMainLand = false;
    }
    
    public Cell(Coordinates coordinates) {
        this.Coordinates = coordinates;

        Prefab = Prefabs.getNoElement();
        PlayerId = 0;
        IsMainLand = false;
    }
    
    
}
