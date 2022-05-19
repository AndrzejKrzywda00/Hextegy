public class GridGenerator
{

    public Cell[] GenerateGrid(int height, int width)
    {
        Cell[] cells = new Cell[height * width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                cells[(i+1)*j] = CreateCell();
            }
        }
        return cells;
    }

    private Cell CreateCell()
    {
        return new Cell();
    }
}
