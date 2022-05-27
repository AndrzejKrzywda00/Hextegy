using UnityEngine;
/*
 * Wrapper class for HexCell with added metric of pathfinding
 */
public class Node {

    private HexCell _hexCell;
    private float _metric;
    private Node _parent;
    
    public HexCell GetCell => _hexCell;

    public float Metric => _metric;

    public Node Parent => _parent;
    
    public Node(HexCell hexCell, float metric, Node parent) {
        _hexCell = hexCell;
        _metric = metric;
        _parent = parent;
    }

    public Node(HexCell hexCell) {
        _hexCell = hexCell;
    }

    public void SetMetric(float metric) {
        _metric = metric;
    }

    public HexCoordinates[] FindNeighbors() {
        return _hexCell.NeighborsCoordinates();
    }

}
