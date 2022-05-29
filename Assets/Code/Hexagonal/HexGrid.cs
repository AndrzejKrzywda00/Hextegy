using System.Collections.Generic;
using Code.Generator;
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
        _cellPrototypes = GenerateMap();
        CreateCells();
    }

    private static Cell[] GenerateMap() {
        GridGenerator generator = new GridGenerator();

        generator.GeneratePlayerFields(4, 4);
        // you can change parameters here!!!
        //generator.scale = 1f;
        //generator.fulfil = 0.3f;
        //generator.playerFields.Add(new GridGenerator.PlayerField(1,2));
        //generator.treeRatio = 0.7f;
        
        return generator.GenerateMap(GridHeight, GridWidth);
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
        int prototypeIndex = GetCellIndexByPosition(x, z);
        if (_cellPrototypes[prototypeIndex] == null) return;

        // TODO -- refactor this to new method
        HexCell hexCell = _cells[i] = Instantiate(hexCellPrefab);
        Transform cellTransform = hexCell.transform;
        cellTransform.SetParent(transform, false);
        cellTransform.localPosition = position;
        hexCell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        MapPrototypeToHexCell(prototypeIndex, hexCell);
    }

    public int GetCellIndexByPosition(int x, int z) {
        return x * GridWidth + z;
    }

    public int GetCellIndexByHexCoordinates(HexCoordinates coordinates) {
        return coordinates.Z * GridWidth + coordinates.X + coordinates.Z / 2;
    }

    public HexCell CellAtCoordinates(HexCoordinates coordinates) {
        int index = GetCellIndexByHexCoordinates(coordinates);
        if (index < _cells.Length && index >= 0) return _cells[index];
        return null;
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
        _hexMesh.Triangulate(_cells);
    }

    private int GetCellIndex(Vector3 position) {
        position = transform.InverseTransformPoint(position);
        HexCoordinates hexCoordinates = HexCoordinates.FromPosition(position);
        int cellIndex = GetCellIndexByHexCoordinates(hexCoordinates);
        return cellIndex;
    }
}