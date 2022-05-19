﻿using UnityEngine;

public class HexFeatureManager : MonoBehaviour
{

    public Transform featurePrefab;
    
    public void Clear() {}

    public void Apply() {}

    public void AddFeature(Vector3 position)
    {
        Transform instance = Instantiate(featurePrefab);
        instance.localPosition = RandomizePosition(position);
        instance.Rotate(new Vector3(0, 180 ,0));
    }

    private Vector3 RandomizePosition(Vector3 position)
    {
        position.x += Random.Range(0, 4);
        position.z += Random.Range(0, 4);
        return position;
    }
}