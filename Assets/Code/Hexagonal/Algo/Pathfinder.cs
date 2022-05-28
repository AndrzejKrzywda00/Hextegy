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
    
    private Node destination;
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
        return CalculatePathFromTo(from, to).Length > 2;
    }

    private void InitializePathfindingProcess(HexCell from) {
        _openList = new List<Node>();
        _openList.Add(new Node(from, Mathf.Infinity, null));
        _closedList = new List<HexCell>();
    }

    private HexCoordinates[] CalculatePathFromTo(HexCell from, HexCell to) {
        while (_openList.Count > 0) {
            var lowestMetricNode = _openList[0];
            ExpandNode(lowestMetricNode);
            SortOpenListByMetric();
        }
        return GeneratePathBasedOnLists();
    }

    private HexCoordinates[] GeneratePathBasedOnLists() {
        var node = destination;
        List<HexCoordinates> path = new List<HexCoordinates>();
        while (node.Parent != null) {
            path.Add(node.GetCell.coordinates);
            node = node.Parent;
        }

        return path.ToArray();
    }

    private void SortOpenListByMetric() {
        _openList.Sort((n1, n2) => Math.Sign(n1.Metric - n2.Metric));
    }

    private void ExpandNode(Node node) {
        _openList.Remove(node);
        var neighborsCoordinates = node.FindNeighbors();
        HandleNeighborCells(neighborsCoordinates, node);
        _closedList.Add(node.GetCell);
    }

    private void HandleNeighborCells(HexCoordinates[] coordinates, Node parentNode) {
        List<HexCell> neighborCells = GenerateNeighbors(coordinates);

        foreach (HexCell neighbor in neighborCells) {
            if (IsDestination(neighbor)) {
                destination.SetParent(parentNode);
                break;
            }
            Node node = CreateNode(parentNode, neighbor);
            if (OpenListContainsNodeWithLowerMetric(node)) continue;
            if (ClosedListContainsNode(node)) continue;
            _openList.Add(node);
        }
    }

    private bool ClosedListContainsNode(Node node) {
        HexCell cellFromList = _closedList.Find(cellInList => cellInList.Equals(node.GetCell));
        if (cellFromList != null) return true;
        return false;
    }

    private bool OpenListContainsNodeWithLowerMetric(Node node) {
        Node nodeFromList = _openList.Find(nodeInList => nodeInList.GetCell.Equals(node.GetCell));
        if (nodeFromList != null && nodeFromList.Metric < node.Metric) return true;
        return false;
    }

    private Node CreateNode(Node parentNode, HexCell neighbor) {
        return new Node(neighbor, CalculateMetricOfCell(parentNode, neighbor), parentNode);
    }

    private List<HexCell> GenerateNeighbors(HexCoordinates[] coordinates) {
        List<HexCell> neighborCells = new List<HexCell>();
        
        foreach (HexCoordinates coordinate in coordinates) {
            var hexCell = _grid.CellAtCoordinates(coordinate);
            if(CellDoesntExist(hexCell)) continue;
            neighborCells.Add(hexCell);
        }

        return neighborCells;
    }

    private bool IsDestination(HexCell hexCell) {
        return hexCell == destination.GetCell;
    }

    private bool CellDoesntExist(HexCell hexCell) {
        return hexCell == null;
    }

    private float CalculateMetricOfCell(Node parentNode, HexCell cell) {
        return _scaleOfDistanceMetric * GaussianDistanceBetweenCells(cell, destination.GetCell) + cell.HexDistanceTo(parentNode.GetCell);
    }

    private float GaussianDistanceBetweenCells(HexCell c1, HexCell c2) {
        return Mathf.Sqrt((c1.coordinates.X - c2.coordinates.X) ^ 2 + (c1.coordinates.Z - c2.coordinates.Z) ^ 2);
    }

}
