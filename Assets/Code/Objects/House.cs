using UnityEngine;

public class House : MonoBehaviour
{
    private int _generatedValue;
    int _price;
    void Start()
    {
        _generatedValue = 4;
        _price = 10;
    }

    void IncreasePrice()
    {
        if(_price < 1000 ) _price += 2;
    }
}
