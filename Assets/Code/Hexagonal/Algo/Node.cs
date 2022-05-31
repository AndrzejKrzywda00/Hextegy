using System;

/*
 * Wrapper class for HexCell with added metric of pathfinding
 */
namespace Code.Hexagonal.Algo {
    public class Node {

        private readonly HexCell _hexCell;
        private float _metric;
        private Node _parent;
     
        public HexCell GetCell => _hexCell;

        public float Metric => _metric;

        public Node Parent => _parent;


        public override bool Equals(object o) {
            if (o == null) return false;
            try {
                Node otherNode = (Node) o;
                return otherNode._hexCell.Equals(_hexCell);
            }
            catch (InvalidCastException) {
                return false;
            }
        }

        public override int GetHashCode() {
            return _hexCell.GetHashCode();
        }
    
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

        public void SetParent(Node parent) {
            _parent = parent;
        }

        public HexCoordinates[] FindNeighbors() {
            return _hexCell.NeighborsCoordinates();
        }

    }
}
