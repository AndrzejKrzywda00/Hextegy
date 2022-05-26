using Code.Generator;
using UnityEngine;

public class GridGenerator {
    
    private House _house = Resources.Load<House>("House");
    private Tree _tree = Resources.Load<Tree>("Tree");
    private NormalTower _normalTower = Resources.Load<NormalTower>("NormalTower");
    private SuperTower _superTower = Resources.Load<SuperTower>("SuperTower");
    private NoElement _noElement = Resources.Load<NoElement>("NoElement");
    private CommonKnight _commonKnight = Resources.Load<CommonKnight>("CommonKnight");
    
    // this is capital of the player, use SetPlayerId before adding
    private Capital _capital = Resources.Load<Capital>("Capital");

    
    
    public Cell[] GenerateMap(int height, int width) {
        MapGrid map = Code.Generator.GridGenerator.GenerateGrid(height, width);

        return map.cells;
    }
    
    
}
