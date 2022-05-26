using UnityEngine;

public class HexGrid : MonoBehaviour {
    private const int GridWidth = 30;
    private const int GridHeight = 30;
    
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
        _cellPrototypes = new GridGenerator().GenerateMap(GridWidth, GridHeight);
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
        _cells = new HexCell[GridHeight * GridWidth];
        FillCells();
    }

    private void FillCells() {
        for (int z = 0, i = 0; z < GridHeight; z++) {
            for (int x = 0; x < GridWidth; x++) {
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
        int prototypeIndex = x * GridWidth + z;
        if (_cellPrototypes[prototypeIndex] == null) return;
        
        HexCell hexCell = _cells[i] = Instantiate(hexCellPrefab);
        Transform cellTransform = hexCell.transform;
        cellTransform.SetParent(transform, false); 
        cellTransform.localPosition = position; 
        hexCell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        MapPrototypeToHexCell(prototypeIndex, hexCell);
    }

    private void MapPrototypeToHexCell(int prototypeIndex, HexCell hexCell) {
        hexCell.PutOnCell(_cellPrototypes[prototypeIndex].Prefab); 
        hexCell.playerId = _cellPrototypes[prototypeIndex].PlayerId;
    }

    private void HandleInput() {
        Ray inputRay = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(inputRay, out RaycastHit hit)) InteractWithCell(hit.point);
    }

    private void InteractWithCell(Vector3 position) {
        var cellIndex = GetCellIndex(position);
        if (_cells[cellIndex] != null) _playerController.Handle(_cells[cellIndex]);
    }

    private int GetCellIndex(Vector3 position) {
        position = transform.InverseTransformPoint(position);
        HexCoordinates hexCoordinates = HexCoordinates.FromPosition(position);
        int cellIndex = hexCoordinates.X + hexCoordinates.Z * GridWidth + hexCoordinates.Z / 2;
        Debug.Log(hexCoordinates.ToString());
        return cellIndex;
    }
}
