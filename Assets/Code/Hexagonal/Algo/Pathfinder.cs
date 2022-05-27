using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        InitializePathfindingProcess(from);
        return CalculatePathFromTo(from, to).Length > 2;        // from and to always inside
    }

    private void InitializePathfindingProcess(HexCell from) {
        _openList = new List<Node>();
        _openList.Add(new Node(from, CalculateMetricOfCell(from)));
    }

    private HexCoordinates[] CalculatePathFromTo(HexCell from, HexCell to) {
        
        while (_openList.Count > 0) {
            var lowestMetricNode = _openList[0];
            ExpandNode(lowestMetricNode);
        }

        return new HexCoordinates[2];
    }

    private void ExpandNode(Node node) {
        var neighborsCoordinates = node.FindNeighbors();
        /*
         * Access itd
         */
        _openList.Remove(node);
        _closedList.Add(node.GetCell);
    }

    private void FindNeighborCells(HexCoordinates[] coordinates) {
        List<HexCell> neighborCells = new List<HexCell>();
        foreach (HexCoordinates coordinate in coordinates) {
            var hexCell = _grid.CellAtCoordinates(coordinate);
            if(CellDoesntExistOrIsInClosedList(hexCell)) continue;
            _openList.Add(new Node(hexCell, CalculateMetricOfCell(hexCell)));
        }
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
