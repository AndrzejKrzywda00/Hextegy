using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

/*
 * Implementation of pathfinding algorithm adjusted for our kind of map
 * Generally operating on A* principles
 */
public class Pathfinder : MonoBehaviour {
    
    private HexCell destination;
    private HexCell source;
    private List<Node> _openList;
    private List<HexCell> _closedList;
    private float _scaleOfDistanceMetric;
    private HexGrid _grid;

    private void Awake() {
        _grid = FindObjectOfType<HexGrid>();
    }

    public bool IsTherePathFromTo(HexCell from, HexCell to) {
        _scaleOfDistanceMetric = 1f;
        InitializePathfindingProcess(from);
        return CalculatePathFromTo(from, to).Length > 2;        // from and to always inside
    }

    private void InitializePathfindingProcess(HexCell from) {
        _openList = new List<Node>();
        _openList.Add(new Node(from, CalculateMetricOfCell(from)));
        _closedList = new List<HexCell>();
    }

    private HexCoordinates[] CalculatePathFromTo(HexCell from, HexCell to) {
        
        while (_openList.Count > 0) {
            var lowestMetricNode = _openList[0];
            ExpandNode(lowestMetricNode);
            SortOpenListByMetric();
        }

        return new HexCoordinates[2];
    }

    private void SortOpenListByMetric() {
        _openList.Sort((n1, n2) => Math.Sign(n1.Metric - n2.Metric));
    }

    private void ExpandNode(Node node) {
        _openList.Remove(node);
        var neighborsCoordinates = node.FindNeighbors();
        HandleAddingNeighborCellsToOpenList(neighborsCoordinates);
        _closedList.Add(node.GetCell);
    }

    private void HandleAddingNeighborCellsToOpenList(HexCoordinates[] coordinates) {
        List<HexCell> neighborCells = new List<HexCell>();
        foreach (HexCoordinates coordinate in coordinates) {
            var hexCell = _grid.CellAtCoordinates(coordinate);
            if(CellDoesntExistOrIsInClosedList(hexCell)) continue;
            _openList.Add(new Node(hexCell, CalculateMetricOfCell(hexCell)));
            if (IsDestination(hexCell)) break;
        }
    }

    private bool IsDestination(HexCell hexCell) {
        return hexCell == destination;
    }

    private bool CellDoesntExistOrIsInClosedList(HexCell hexCell) {
        return hexCell == null || _closedList.Contains(hexCell);
    }

    private float CalculateMetricOfCell(HexCell cell) {
        return _scaleOfDistanceMetric * GaussianDistanceBetweenCells(cell, destination);
    }

    private float GaussianDistanceBetweenCells(HexCell c1, HexCell c2) {
        return Mathf.Sqrt((c1.coordinates.X - c2.coordinates.X) ^ 2 + (c1.coordinates.Z - c1.coordinates.Z) ^ 2);
    }

}
