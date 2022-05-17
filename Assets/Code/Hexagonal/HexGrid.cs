using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public HexCell cellPrefab;
    private Canvas _gridCanvas;
    private HexMesh _hexMesh;
    public Text cellLabelPrefab;
    
    private HexCell[] _cells;

    private void Awake()
    {
        _gridCanvas = GetComponentInChildren<Canvas>();
        _hexMesh = GetComponentInChildren<HexMesh>();
        CreateCells();
        FillCells();
    }

    private void Start()
    {
        _hexMesh.Triangulate(_cells);
    }

    private void CreateCells()
    {
        _cells = new HexCell[height * width];
    }

    private void FillCells()
    {
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
        Vector3 position;
        position.x = (x + z * 0.5f - z/2) * (HexMetrics.InnerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.OuterRadius * 1.5f);
        
        HexCell cell = _cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(_gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }
    
}

