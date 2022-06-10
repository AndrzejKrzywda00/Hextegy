using Code.CellObjects.Units.Implementations;
using UnityEngine;

namespace Code.CellObjects {
    public abstract class CellObject : MonoBehaviour {
        protected abstract int Level();

        public bool IsWeakerThan(CellObject unit) {
            if (unit == null) return true;
            if (this is UnitTier4 && unit is UnitTier4) return true;
            return Level() < unit.Level();
        }
    }
}
