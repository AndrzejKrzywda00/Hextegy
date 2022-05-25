using UnityEngine;

public class GridGenerator {
    
    private House _house = Resources.Load<House>("House");
    private Tree _tree = Resources.Load<Tree>("Tree");
    private NormalTower _normalTower = Resources.Load<NormalTower>("NormalTower");
    private SuperTower _superTower = Resources.Load<SuperTower>("SuperTower");
    private NoElement _noElement = Resources.Load<NoElement>("NoElement");
    private CommonKnight _commonKnight = Resources.Load<CommonKnight>("CommonKnight");

    public Cell[] GenerateGrid(int height, int width) {
        Cell[] cells = new Cell[height * width];

        float[,] perlinNoise = GenerateNoiseMap(height, width, 10);
        
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                cells[i * width + j] = CreateCell(perlinNoise[i, j]);
            }
        }
        return cells;
    }

    public float[,] GenerateNoiseMap(int height, int width, float scale) {
        float[,] noiseMap = new float[height, width];
        for (int zIndex = 0; zIndex < height; zIndex ++) {
            for (int xIndex = 0; xIndex < width; xIndex++) {
                // calculate sample indices based on the coordinates and the scale
                float sampleX = xIndex / scale;
                float sampleZ = zIndex / scale;

                Debug.Log(sampleX + " "+ sampleZ);

                // generate noise value using PerlinNoise
                float noise = Mathf.PerlinNoise(sampleX, sampleZ);
                noiseMap [zIndex, xIndex] = noise;
            }
        }
        return noiseMap;
    }
    
    private Cell CreateCell(float height) {
        Debug.Log(height);
        return height > 0.4 ? new Cell(_noElement) : null;
    }
}
