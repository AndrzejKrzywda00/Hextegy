using UnityEngine;

public class CommonKnight : MonoBehaviour, IComparable {

    public bool IsWeakerThan(IComparable unit)
    {
        return Level() - unit.Level() < 0;
    }

    public int Level()
    {
        return 1;
    }
    
}
