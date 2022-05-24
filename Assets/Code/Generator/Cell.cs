using UnityEngine;

public class Cell {
    
    public MonoBehaviour Prefab;
    public int PlayerId;

    public Cell(MonoBehaviour prefab) {
        Prefab = prefab;
        PlayerId = Random.Range(0, 7);
    }
}
