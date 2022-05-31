using UnityEngine.EventSystems;

namespace Code.CellObjects.Structures {
    public class SuperTower : ActiveObject {
        public override int Level() {
            return 2;
        }

        public override bool Protects() {
            return true;
        }

        public override int Price() {
            return 35;
        }

        public override int MaintenanceCost() {
            return 5;
        }
    }
}
