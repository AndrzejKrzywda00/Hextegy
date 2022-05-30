using Code.Generator;

public class Cell {
    
    public readonly Coordinates Coordinates;
    public CellObject Prefab;
    public int PlayerId;
    public bool IsMainLand;

    public Cell(int x, int y) {
        Coordinates = new Coordinates(x, y);

        Prefab = Prefabs.GetNoElement();
        PlayerId = 0;
        IsMainLand = false;
    }
    
    public Cell(Coordinates coordinates) {
        Coordinates = coordinates;

        Prefab = Prefabs.GetNoElement();
        PlayerId = 0;
        IsMainLand = false;
    }

    public bool HasTree() {
        return Prefab is Tree;
    }
}
