using UnityEngine;
/*
 * Wrapper class for HexCell with added metric of pathfinding
 */
public class Node {

    private HexCell _hexCell;
    private float _metric;
    
    public Node CreateNodeWithMetric(HexCell hexCell, float metric) {
        _hexCell = hexCell;
        _metric = metric;
        return this;
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
