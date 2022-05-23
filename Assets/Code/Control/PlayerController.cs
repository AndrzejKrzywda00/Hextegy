using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private MonoBehaviour selectedElement;
    
    void Start() {
        
    }
    
    void Update() {
        
    }

    public void Handle(HexCell cell) {
        if (cell.IsEmpty()) CommonKnight.PutOnCell(cell);
    }
}
