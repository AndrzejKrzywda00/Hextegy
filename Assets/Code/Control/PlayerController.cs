using UnityEngine;

public class PlayerController : MonoBehaviour {

    private int _coins;
    private int _balance;
    private HexCell _cellWithSelectedUnit;

    public int Balance => _balance;
    public int Coins => _coins;

    public void Handle(HexCell cell, HexMesh hexMesh) {
        
        if (_cellWithSelectedUnit != null) {
            cell.prefab = _cellWithSelectedUnit.prefab; 
            _cellWithSelectedUnit.PutOnCell(Resources.Load<NoElement>("NoElement"));
            
            hexMesh.Triangulate(cell);
            hexMesh.Triangulate(_cellWithSelectedUnit); 
            _cellWithSelectedUnit = null;
        }
        
        if (cell.HasUnit()) {
            _cellWithSelectedUnit = cell;
            return;
        }

    }

    public void EndTurn()
    {
        _coins -= _balance;
    }
    
}
