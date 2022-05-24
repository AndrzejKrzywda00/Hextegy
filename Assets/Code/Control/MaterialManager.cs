using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour {

    private Dictionary<int, Material> _playersToColors = new Dictionary<int, Material>();
    
    void Awake()
    {
        _playersToColors.Add(0, Resources.Load<Material>("Materials/GreyBackground"));
        _playersToColors.Add(1, Resources.Load<Material>("Materials/GreenBackground"));
        _playersToColors.Add(2, Resources.Load<Material>("Materials/PurpleBackground"));
        _playersToColors.Add(3, Resources.Load<Material>("Materials/SkyBackground"));
        _playersToColors.Add(4, Resources.Load<Material>("Materials/TomatoBackground"));
        _playersToColors.Add(5, Resources.Load<Material>("Materials/YellowBackground"));
    }

    public Material BackgroundOfPlayer(int playerId)
    {
        if(_playersToColors.ContainsKey(playerId)) return _playersToColors[playerId];
        throw new ArgumentException();
    }
}
