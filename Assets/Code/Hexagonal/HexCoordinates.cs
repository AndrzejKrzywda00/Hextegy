using UnityEngine;

[System.Serializable]
public struct HexCoordinates {
    
    [SerializeField]
    private int x, z;

    public int Y => -x - z;
    public int X => x;
    public int Z => z;

    public HexCoordinates(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z) {
        return new HexCoordinates(x - z/2, z);
    }

    public static HexCoordinates FromPosition(Vector3 position) {
        
        float offset = position.z / (HexMetrics.OuterRadius * 3f);
        float x = position.x / (HexMetrics.InnerRadius * 2f);
        float y = -x;

        y -= offset;
        x -= offset;
        
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x -y);

        if (iX + iY + iZ == 0) {
            return new HexCoordinates(iX, iZ);
        }
        
        float dX = Mathf.Abs(x - iX);
        float dY = Mathf.Abs(y - iY);
        float dZ = Mathf.Abs(-x - y - iZ);

        if (dX > dY && dX > dZ) {
            iX = -iY - iZ;
        } else if (dZ > dY) {
            iZ = -iX - iY;
        }

        return new HexCoordinates(iX, iZ);
    }

    public override string ToString() {
        return "(" + x + ", " + Y + ", " + z + ")";
    }

    public string ToStringOnSeparateLines() {
        return x + "\n" + Y + "\n" + z;
    }

    public HexCoordinates[] NeighborsOf(HexCoordinates hexCoordinates) {
        const int neighborsInHex = 6;
        HexCoordinates[] neighbors = new HexCoordinates[neighborsInHex];
        
        neighbors[0] = FromOffsetCoordinates(hexCoordinates.x - 1, hexCoordinates.z + 1);
        neighbors[1] = FromOffsetCoordinates(hexCoordinates.x, hexCoordinates.z + 1);
        neighbors[2] = FromOffsetCoordinates(hexCoordinates.x + 1, hexCoordinates.z);
        neighbors[3] = FromOffsetCoordinates(hexCoordinates.x + 1, hexCoordinates.z - 1);
        neighbors[4] = FromOffsetCoordinates(hexCoordinates.x, hexCoordinates.z - 1);
        neighbors[5] = FromOffsetCoordinates(hexCoordinates.x - 1, hexCoordinates.z);
        
        return neighbors;
    }
}
