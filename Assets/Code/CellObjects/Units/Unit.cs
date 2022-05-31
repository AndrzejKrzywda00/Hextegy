using System;
using UnityEngine;

public abstract class Unit : CellObject {
    
    private double _upDownAnimationValue;
    
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
}