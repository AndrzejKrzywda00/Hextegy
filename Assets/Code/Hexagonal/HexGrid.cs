using System.Collections.Generic;
using Code.CellObjects;
using Code.Control;
using Code.Control.Game;
using Code.Generator;
using Code.Generator.Objects;
using UnityEngine;

public class HexGrid : MonoBehaviour {
    
    public static int NumberOfPlayers = 4;
    private const int GridWidth = 30;
    private const int GridHeight = 30;

    public HexCell hexCellPrefab;

    private PlayerController _playerController;
    private Camera _cam;
    private HexMesh _hexMesh;
    private HexCell[] _cells;
    private Cell[] _cellPrototypes;

    public HexCell[] Cells => _cells;

    private void Awake() {
        _playerController = FindObjectOfType<PlayerController>();
        _cam = Camera.main;
        _hexMesh = GetComponentInChildren<HexMesh>();
        _cellPrototypes = GenerateMap();
        CreateCells();
    }

    private static Cell[] GenerateMap() {
        GridGenerator generator = new GridGenerator();

        generator.GeneratePlayerFields(NumberOfPlayers, 4);
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
        
        HexCell hexCell = _cells[i] = Instantiate(hexCellPrefab);
        PositionTheCellOnGrid(x, z, position, hexCell);
        MapPrototypeToHexCell(prototypeIndex, hexCell);
    }

    private void PositionTheCellOnGrid(int x, int z, Vector3 position, HexCell hexCell) {
        Transform cellTransform = hexCell.transform;
        cellTransform.SetParent(transform, false);
        cellTransform.localPosition = position;
        hexCell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
    }

    public Dictionary<int, int> MapCellsToInitialBalanceOfPlayers() {

        Dictionary<int, int> playersInitialBalances = new Dictionary<int, int>();
        
        foreach (Cell cellPrototype in _cellPrototypes) {
            if (cellPrototype == null) continue;
            if (!playersInitialBalances.ContainsKey(cellPrototype.PlayerId)) playersInitialBalances.Add(cellPrototype.PlayerId, 0);
            if (!cellPrototype.HasTree()) playersInitialBalances[cellPrototype.PlayerId] += 1;
        }

        return playersInitialBalances;
    }

    private int GetCellIndexByPosition(int x, int z) {
        return x * GridWidth + z;
    }

    private int GetCellIndexByHexCoordinates(HexCoordinates coordinates) {
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
        var cellIndex = GetCellIndexFromPosition(position);
        if (_cells[cellIndex] != null) _playerController.Handle(_cells[cellIndex]);
        _hexMesh.Triangulate(_cells);
    }

    private int GetCellIndexFromPosition(Vector3 position) {
        position = transform.InverseTransformPoint(position);
        HexCoordinates hexCoordinates = HexCoordinates.FromPosition(position);
        int cellIndex = GetCellIndexByHexCoordinates(hexCoordinates);
        return cellIndex;
    }

    public void GenerateTreesNextToExistingTrees() {
        foreach (HexCell cell in _cells) if(cell != null && cell.HasTree()) GenerateTreeInRandomAdjacentCell(cell);
    }

    private void GenerateTreeInRandomAdjacentCell(HexCell hexCell) {
        int randomIndex = Random.Range(0, HexMetrics.NumOfTrianglesInHexagon);
        int index = 0;
        HexCoordinates[] neighborsCoordinates = hexCell.NeighborsCoordinates();
        foreach (HexCoordinates coordinates in neighborsCoordinates) {
            HexCell neighbor = CellAtCoordinates(coordinates);
            if (neighbor != null && index++ == randomIndex && neighbor.IsEmpty()) {
                HandleAddingTreeToCell(neighbor);
            } 
        }
    }

    private void HandleAddingTreeToCell(HexCell neighbor) {
        neighbor.PutOnCell(Prefabs.GetTree());
        MoneyManager.DecrementBalanceOfPlayer(neighbor.playerId);
    }
}