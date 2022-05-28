using Code.Generator;
using UnityEngine;

public class Cell {
    public Coordinates coordinates;

    public MonoBehaviour Prefab;
    public int PlayerId;
    

    public Cell(int x, int y) {
        this.coordinates = new Coordinates(x, y);

        Prefab = Prefabs.getNoElement();
        PlayerId = 0;
    }
    
    public Cell(Coordinates coordinates) {
        this.coordinates = coordinates;

        Prefab = Prefabs.getNoElement();
        PlayerId = 0;
    }
    
    
}
