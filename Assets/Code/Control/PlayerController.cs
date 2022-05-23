using UnityEngine;

public class PlayerController : MonoBehaviour {
    private HexCell _selectedCell;

    public void Handle(HexCell cell, HexMesh hexMesh) {
        if (!cell.IsEmpty()) {
            _selectedCell = cell;
            return;
        }
        
        if (_selectedCell == null) {
            cell.PutOnCell(Resources.Load<CommonKnight>("CommonKnight"));
            hexMesh.Triangulate(cell);
        } else {
            cell.prefab = _selectedCell.prefab;
            _selectedCell.PutOnCell(Resources.Load<NoElement>("NoElement"));

            hexMesh.Triangulate(cell);
            hexMesh.Triangulate(_selectedCell);

            _selectedCell = null;
        }
    }
}
