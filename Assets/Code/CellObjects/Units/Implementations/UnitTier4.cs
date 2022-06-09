using UnityEngine;

namespace Code.CellObjects.Units.Implementations {
    public class UnitTier4 : Unit {
        public override int Price() {
            return 60;
        }

        public override bool IsProtectingNearbyFriendlyCells() {
            return false;
        }

        public override int MaintenanceCost() {
            return 26;
        }

        protected override int Level() {
            return 4;
        }

        public override int MovementRange() {
            return 2;
        }
    }
}
