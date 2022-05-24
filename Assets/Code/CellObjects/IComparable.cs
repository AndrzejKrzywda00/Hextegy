using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComparable
{
    bool IsWeakerThan(IComparable unit);
    int Level();
}
