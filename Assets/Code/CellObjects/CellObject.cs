using UnityEngine;

namespace Code.CellObjects {
    public abstract class CellObject : MonoBehaviour {
        protected abstract int Level();

        public bool IsWeakerThan(CellObject unit) {
            if (unit == null) return true;
            return Level() < unit.Level();
        }
    }
}
