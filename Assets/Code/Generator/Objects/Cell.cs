using System.Drawing;
using Code.Generator;
using UnityEngine;

public class Cell {
    public Coordinates coordinates;

    public MonoBehaviour Prefab;
    public int PlayerId;
    

    public Cell(int x, int y) {
        coordinates = new Coordinates(x, y);
        
        Prefab = Resources.Load<NoElement>("NoElement");
        PlayerId = 0;
    }
    
    public Cell(Coordinates coordinates, int playerId) {
        this.coordinates = coordinates;
        this.PlayerId = playerId;
        
        Prefab = Resources.Load<NoElement>("NoElement");
        PlayerId = 0;
    }
    
}
