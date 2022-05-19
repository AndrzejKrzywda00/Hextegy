using UnityEngine;

public class GridGenerator
{
    
    private House _house = Resources.Load<House>("House");
    private Tree _tree = Resources.Load<Tree>("Tree");
    private NormalTower _normalTower = Resources.Load<NormalTower>("NormalTower");
    private SuperTower _superTower = Resources.Load<SuperTower>("SuperTower");
    
    public Cell[] GenerateGrid(int height, int width)
    {
        Cell[] cells = new Cell[height * width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                cells[(i+1)*j] = CreateCell(i, j);
            }
        }
        return cells;
    }

    private Cell CreateCell(int i, int j)
    {
        return new Cell(_house);
    }
}
