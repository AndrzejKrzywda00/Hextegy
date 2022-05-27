using UnityEngine;
/*
 * Wrapper class for HexCell with added metric of pathfinding
 */
public class Node {

    private HexCell _hexCell;
    private float _metric;
    private HexCell _parent;
    
    public HexCell GetCell => _hexCell;

    public float Metric => _metric;

    public HexCell Parent => _parent;
    
    public Node(HexCell hexCell, float metric) {
        _hexCell = hexCell;
        _metric = metric;
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
