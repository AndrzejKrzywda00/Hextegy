using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexFeatureManager : MonoBehaviour
{

    public Transform featurePrefab;

    public void Clear() {}

    public void Apply() {}

    private void Awake()
    {
        featurePrefab = Resources.Load<Tree>("Tree").transform;
    }

    public void AddFeature(Vector3 position, MonoBehaviour prefab)
    {
        Transform instance = Instantiate(prefab.transform);
        instance.localPosition = position;
        instance.Rotate(new Vector3(0, 180 ,0));
    }

    private Vector3 RandomizePosition(Vector3 position)
    {
        position.x += Random.Range(0, 4);
        position.z += Random.Range(0, 4);
        return position;
    }
}
