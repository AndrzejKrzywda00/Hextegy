using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public HexCell cellPrefab;

    private Canvas _gridCanvas;
    private HexMesh _hexMesh;
    private HexCell[] _cells;

    void Awake()
    {
        _gridCanvas = GetComponentInChildren<Canvas>();
        _hexMesh = GetComponentInChildren<HexMesh>();
        CreateCells();
    }

    void Start()
    {
        _hexMesh.Triangulate(_cells);
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) HandleInput();
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) InteractWithCell(hit.point);
    }

    private void InteractWithCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        _hexMesh.Triangulate(_cells);
    }

    private void CreateCells()
    {
        _cells = new HexCell[height * width];
        FillCells();
    }

    private void FillCells()
    {
        for (int z=0, i=0; z<height; z++) 
        for (int x=0; x<width; x++) 
            CreateCell(x, z, i++);
    }

    void CreateCell(int x, int z, int i)
    {
        var position = CreateCellPosition(x, z);
        var cell = InstantiateCellOnGrid(x, z, i, position);
    }

    private HexCell InstantiateCellOnGrid(int x, int z, int i, Vector3 position)
    {
        HexCell cell = _cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        return cell;
    }

    private static Vector3 CreateCellPosition(int x, int z)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.OuterRadius * 1.5f);
        return position;
    }

}

