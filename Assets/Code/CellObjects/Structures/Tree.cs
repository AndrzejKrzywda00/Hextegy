using UnityEngine;

public class Tree : MonoBehaviour {
    public static void PutOnCell(HexCell hexCell) {
        Tree tree = Resources.Load<Tree>("Tree");
        hexCell.prefab = tree;
    }
}
