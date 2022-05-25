using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Implementation of pathfinding algorithm adjusted for our kind of map
 * Generally operating on A* principles
 */
public class Pathfinder
{
    private HexCell destination;
    private HexCell source;

    public bool IsTherePathFromTo(HexCell from, HexCell to)
    {
        return CalculatePathFromTo(from, to).Length > 2;        // from and to always inside
    }

    private HexCoordinates[] CalculatePathFromTo(HexCell from, HexCell to)
    {
        return new HexCoordinates[2];
    }

}
