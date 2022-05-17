using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;

    public int Y { get {return -x - z;} }
    
    public int X
    {
        get { return x; }
    }
    
    public int Z
    {
        get { return z; }
    }

    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z/2, z);
    }

    public override string ToString()
    {
        return "(" + x.ToString() + ", " + Y.ToString() + ", " + z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return x.ToString() + "\n" + Y.ToString() + "\n" + z.ToString();
    }

}
