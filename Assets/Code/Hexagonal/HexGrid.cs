using UnityEngine;

public class HexGrid : MonoBehaviour {
    
    public int gridWidth = 100;
    public int gridHeight = 100;
    public HexCell hexCellPrefab;

    private PlayerController _playerController;
    private Camera _cam;
    private HexMesh _hexMesh;
    private HexCell[] _cells;
    private Cell[] _cellPrototypes;

    private void Awake() {
        _playerController = gameObject.AddComponent<PlayerController>();
        _cam = Camera.main;
        _hexMesh = GetComponentInChildren<HexMesh>();
        _cellPrototypes = new GridGenerator().GenerateGrid(gridWidth, gridHeight);
        CreateCells();
    }

    private void Start() {
        _hexMesh.Triangulate(_cells);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HandleInput();
        }
    }

    private void CreateCells() {
        _cells = new HexCell[gridHeight * gridWidth];
        FillCells();
    }

    private void FillCells() {
        for (int z = 0, i = 0; z < gridHeight; z++) {
            for (int x = 0; x < gridWidth; x++) {
                CreateCell(x, z, i++);
            }
        }
    }

    private void CreateCell(int x, int z, int i) {
        var position = CreateCellPosition(x, z);
        InstantiateCellOnGrid(x, z, i, position);
    }

    private Vector3 CreateCellPosition(int x, int z) {
        return new Vector3 {
            x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2f),
            y = 0f,
            z = z * (HexMetrics.OuterRadius * 1.5f)
        };
    }

    private void InstantiateCellOnGrid(int x, int z, int i, Vector3 position) {
        
        int prototypeIndex = x * gridWidth + z;
        if (_cellPrototypes[prototypeIndex] != null)
        {
            HexCell hexCell = _cells[i] = Instantiate(hexCellPrefab);
            Transform cellTransform = hexCell.transform;
            cellTransform.SetParent(transform, false); 
            cellTransform.localPosition = position; 
            hexCell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
            MapPrototypeToHexCell(prototypeIndex, hexCell);
        }
    }

    private void MapPrototypeToHexCell(int prototypeIndex, HexCell hexCell)
    {
        hexCell.prefab = _cellPrototypes[prototypeIndex].Prefab; 
        hexCell.playerId = _cellPrototypes[prototypeIndex].PlayerId;
    }

    private void HandleInput() {
        Ray inputRay = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(inputRay, out RaycastHit hit)) InteractWithCell(hit.point);
    }

    private void InteractWithCell(Vector3 position) {
        var cellIndex = GetCellIndex(position);
        _playerController.Handle(_cells[cellIndex], _hexMesh);
    }

    private int GetCellIndex(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates hexCoordinates = HexCoordinates.FromPosition(position);
        int cellIndex = hexCoordinates.X + hexCoordinates.Z * gridWidth + hexCoordinates.Z / 2;
        return cellIndex;
    }
}
