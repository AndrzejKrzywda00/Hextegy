using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {
    
    private Mesh _hexMesh;
    private List<Vector3> _vertices;
    private List<int> _triangles;
    private List<Color> _colors;
    private MeshCollider _collider;

    public HexFeatureManager features;

    private void Awake() {
        CreateMeshAndCollider();
        InstantiateLists();
    }

    private void CreateMeshAndCollider() {
        _hexMesh = new Mesh {
            name = "Hex Mesh"
        };
        GetComponent<MeshFilter>().mesh = _hexMesh;
        _collider = gameObject.AddComponent<MeshCollider>();
    }

    private void InstantiateLists() {
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _colors = new List<Color>();
    }

    public void Triangulate(HexCell[] cells) {
        ClearData();
        foreach (HexCell hexCell in cells) Triangulate(hexCell);
        GenerateTrianglesFromData();
        features.Apply();
        AddColliderToMesh();
    }

    private void ClearData() {
        _hexMesh.Clear();
        _vertices.Clear();
        _triangles.Clear();
        _colors.Clear();
        features.Clear();
    }

    private void Triangulate(HexCell hexCell) {
        if (hexCell == null) return;
        Vector3 center = hexCell.transform.localPosition;
        for (int i = 0; i < 6; i++) {
            AddTriangle (
                center,
                center + HexMetrics.Corners[i], 
                center + HexMetrics.Corners[(i + 1) % 6]
                );
            AddTriangleColor(hexCell.CellColor(hexCell));
        }
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3) {
        int vertexIndex = _vertices.Count;
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);
    }

    private void AddTriangleColor(Color color) {
        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);
    }

    private void GenerateTrianglesFromData() {
        _hexMesh.vertices = _vertices.ToArray();
        _hexMesh.triangles = _triangles.ToArray();
        _hexMesh.colors = _colors.ToArray();
        _hexMesh.RecalculateNormals();
    }
    
    private void AddColliderToMesh() {
        _collider.sharedMesh = _hexMesh;
    }
}
