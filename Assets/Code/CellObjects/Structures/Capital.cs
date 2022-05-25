using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capital : MonoBehaviour {
    private int _playerId;

    public int PlayerId => _playerId;

    public void SetPlayerId(int id) {
        _playerId = id;
    }
    
}
