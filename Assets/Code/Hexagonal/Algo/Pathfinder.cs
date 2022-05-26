using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Implementation of pathfinding algorithm adjusted for our kind of map
 * Generally operating on A* principles
 */
public class Pathfinder {
    
    private HexCell destination;
    private HexCell source;
    private SortedList<HexCell, float> openList;
    private List<HexCell> closedList;
    private float scaleOfDistanceMetric;

    public bool IsTherePathFromTo(HexCell from, HexCell to) {
        InitializePathfindingProcess(from);
        return CalculatePathFromTo(from, to).Length > 2;        // from and to always inside
    }

    private void InitializePathfindingProcess(HexCell from) {
        openList = new SortedList<HexCell, float>();
        openList.Add(from, CalculateMetricOfCell(from));
    }

    private HexCoordinates[] CalculatePathFromTo(HexCell from, HexCell to) {
        while (openList.Count > 0) {
            return new HexCoordinates[2];
        }

        return new HexCoordinates[2];
    }

    private float CalculateMetricOfCell(HexCell cell) {
        return scaleOfDistanceMetric * GaussianDistanceBetweenCells(cell, destination);
    }

    private float GaussianDistanceBetweenCells(HexCell c1, HexCell c2) {
        return Mathf.Sqrt((c1.coordinates.X - c2.coordinates.X) ^ 2 + (c1.coordinates.Z - c1.coordinates.Z) ^ 2);
    }

}
