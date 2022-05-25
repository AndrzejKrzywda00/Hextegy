using UnityEngine;
using Random = UnityEngine.Random;

public class HexFeatureManager : MonoBehaviour {
    public void Clear() {}
    public void Apply() {}

    private Vector3 RandomizePosition(Vector3 position) {
        position.x += Random.Range(0, 4);
        position.z += Random.Range(0, 4);
        return position;
    }
}
