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

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.InnerRadius * 2f);
        float y = -x;
        float offset = position.z / (HexMetrics.OuterRadius * 3f);
        x -= offset;
        y -= offset;
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x -y);

        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }
        
        return new HexCoordinates(iX, iZ);
    }

    public override string ToString()
    {
        return "(" + x.ToString() + ", " + Y.ToString() + ", " + z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return x.ToString() + "\n" + Y.ToString() + "\n" + z.ToString();
    }

    public HexCoordinates[] NeighborsOf(HexCoordinates coordinates)
    {
        const int neighborsInHex = 6;
        HexCoordinates[] neighbors = new HexCoordinates[neighborsInHex];
        neighbors[0] = FromOffsetCoordinates(coordinates.x - 1, coordinates.z + 1);
        neighbors[1] = FromOffsetCoordinates(coordinates.x, coordinates.z + 1);
        neighbors[2] = FromOffsetCoordinates(coordinates.x + 1, coordinates.z);
        neighbors[3] = FromOffsetCoordinates(coordinates.x + 1, coordinates.z - 1);
        neighbors[4] = FromOffsetCoordinates(coordinates.x, coordinates.z - 1);
        neighbors[5] = FromOffsetCoordinates(coordinates.x - 1, coordinates.z);
        return neighbors;
    }

}
