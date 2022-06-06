using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Implementation of pathfinding algorithm adjusted for our kind of map
 * Generally operating on A* principles
 */
namespace Code.Hexagonal.Algo {
    public class Pathfinder : MonoBehaviour {
        private const float MaximumMetric = 100000f;
        
        private Node _destination;
        private Node _source;
        private List<Node> _openList;
        private List<Node> _closedList;
        private HexGrid _grid;
        private bool _searchEnded;
    
        private void Awake() {
            _grid = FindObjectOfType<HexGrid>();
        }
    
        public HexCoordinates[] OptionalPathFromTo(HexCell from, HexCell to) {
            HexCoordinates[] path = PathFromTo(from, to);
            if (path.Contains(from.coordinates) && path.Contains(to.coordinates) && PathConsistsOfNonFriendlyCells(path)) return path;
            return null;
        }

        private HexCoordinates[] PathFromTo(HexCell from, HexCell to) {
            InitializePathfindingProcessParameters(from, to);
            return CalculatePathFromTo();
        }

        private void InitializePathfindingProcessParameters(HexCell from, HexCell to) {
            _searchEnded = false;
            _destination = new Node(to, 0f, null);
            _source = new Node(from, MaximumMetric, null);
            _openList = new List<Node>{_source};
            _closedList = new List<Node>();
        }

        private HexCoordinates[] CalculatePathFromTo() {
            int limit = 0;
            while (_openList.Count > 0 && !_searchEnded && limit++ < 1000) {
                Node lowestMetricNode = _openList[0];
                ExpandNode(lowestMetricNode);
                SortOpenListByMetric();
            }
            return GeneratePathBasedOnLists();
        }

        private void ExpandNode(Node node) {
            _openList.Remove(node);
            HexCoordinates[] neighborsCoordinates = node.FindNeighbors();
            HandleNeighborCells(neighborsCoordinates, node);
            _closedList.Add(node);
        }

        private void HandleNeighborCells(HexCoordinates[] coordinates, Node parentNode) {
            List<HexCell> neighborCells = GenerateNeighbors(coordinates);

            foreach (HexCell neighbor in neighborCells) {
                if (IsDestination(neighbor)) {
                    _searchEnded = true;
                    _destination.SetParent(parentNode);
                    break;
                }
                Node node = CreateNode(parentNode, neighbor);
                if (OpenListContainsNodeWithLowerMetric(node)) continue;
                if (ClosedListContainsNode(node)) continue;
                _openList.Add(node);
            }
        }

        private List<HexCell> GenerateNeighbors(HexCoordinates[] coordinates) {
            List<HexCell> neighborCells = new List<HexCell>();
        
            foreach (HexCoordinates coordinate in coordinates) {
                HexCell hexCell = _grid.CellAtCoordinates(coordinate);
                if(CellNotFeasible(hexCell)) continue;
                neighborCells.Add(hexCell);
            }

            return neighborCells;
        }

        private bool CellNotFeasible(HexCell hexCell) {
            return CellDoesntExist(hexCell);
        }

        private bool CellDoesntExist(HexCell hexCell) {
            return hexCell == null;
        }
        private bool IsDestination(HexCell hexCell) {
            return hexCell.Equals(_destination.GetCell);
        }

        private Node CreateNode(Node parentNode, HexCell neighbor) {
            return new Node(neighbor, CalculateMetricOfCell(neighbor), parentNode);
        }

        private float CalculateMetricOfCell(HexCell cell) {
            return cell.GaussianDistanceTo(_destination.GetCell);
        }
        
        private bool OpenListContainsNodeWithLowerMetric(Node node) {
            Node nodeFromList = _openList.Find(nodeInList => nodeInList.Equals(node));
            return nodeFromList != null && nodeFromList.Metric < node.Metric;
        }
        
        private bool ClosedListContainsNode(Node node) {
            Node nodeFromList = _closedList.Find(nodeInList => nodeInList.Equals(node));
            return nodeFromList != null;
        }
        
        private void SortOpenListByMetric() {
            _openList.Sort((n1, n2) => Math.Sign(n1.Metric - n2.Metric));
        }

        private HexCoordinates[] GeneratePathBasedOnLists() {
            Node node = _destination;
            List<HexCoordinates> path = new List<HexCoordinates>();
        
            while (node != null) {
                path.Add(node.GetCell.coordinates);
                node = node.Parent;
            }

            return path.ToArray();
        }

        private bool PathConsistsOfNonFriendlyCells(HexCoordinates[] coordinatesArray) {
            int index = 0;
            foreach (HexCoordinates coordinates in coordinatesArray) {
                if (!_grid.CellAtCoordinates(coordinates).IsFriendlyCell() && index != 0) return false;
                index++;
            }
            return true;
        }
    }
}
