using System;
using UnityEngine;

namespace Code.CellObjects.Units {
    public abstract class Unit : ActiveObject {
    
        private double _upDownAnimationValue;
        private bool _hasMoveLeftInThisTurn = true;
        
        public bool SetHasMoveLeftInThisTurn {
            set => _hasMoveLeftInThisTurn = value;
        }
    
        private void FixedUpdate() {
            if (!CanMoveInThisTurn()) {
                _upDownAnimationValue = 0;
                return;
            }
            Vector3 transformPosition = transform.position;
            transformPosition.z += (float) Math.Sin(_upDownAnimationValue) / 15;
            _upDownAnimationValue += Math.PI / 30;
            if (_upDownAnimationValue >= 2*Math.PI) _upDownAnimationValue = 0;
            transform.position = transformPosition;
        }

        public abstract int MovementRange();

        public bool CanMoveInThisTurn() {
            return MovementRange() > 0 && _hasMoveLeftInThisTurn;
        }
    }
}