using System;
using UnityEngine;

public class ExperiencedKnight : MonoBehaviour, IComparable
{

    private int _price = 20;
    private int _maintainCost = 5;

    public int MaintainCost => _maintainCost;
    public int Price => _price;
    
    public bool IsWeakerThan(IComparable unit)
    {
        return Level() - unit.Level() < 0;
    }

    public int Level()
    {
        return 2;
    }
}
