using System;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public HexCell cellPrefab;

    private HexCell[] _cells;

    private void Awake()
    {
        _cells = new HexCell[height * width];

        for (int z=0, i=0; z<height; z++)
        {
            for (int x=0; x<width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position = new Vector3(x * 10f, 0f, z * 10f);
        HexCell cell = _cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
    }
    
}

