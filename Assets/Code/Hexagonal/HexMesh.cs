using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    private Mesh _hexMesh;
    private List<Vector3> _vertices;
    private List<int> _triangles;
    private MeshCollider _collider;

    public HexFeatureManager features;

    private void Awake()
    {
        CreateMeshAndCollider();
        InstantiateLists();
    }

    private void InstantiateLists()
    {
        InstantiateVertices();
        InstantiateTriangles();
    }

    private void CreateMeshAndCollider()
    {
        GetComponent<MeshFilter>().mesh = _hexMesh = new Mesh();
        _hexMesh.name = "Hex Mesh";
        _collider = gameObject.AddComponent<MeshCollider>();
    }

    private void InstantiateVertices()
    {
        _vertices = new List<Vector3>();
    }

    private void InstantiateTriangles()
    {
        _triangles = new List<int>();
    }

    public void Triangulate(HexCell[] cells)
    {
        ClearData();
        foreach(HexCell cell in cells) Triangulate(cell);
        GenerateTrianglesFromData();
        features.Apply();
        AddColliderToMesh();
    }

    private void GenerateTrianglesFromData()
    {
        _hexMesh.vertices = _vertices.ToArray();
        _hexMesh.triangles = _triangles.ToArray();
        _hexMesh.RecalculateNormals();
    }

    private void ClearData()
    {
        _hexMesh.Clear();
        _vertices.Clear();
        _triangles.Clear();
        features.Clear();
    }

    void Triangulate(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        CreateCellContent(cell);
        for (int i=0; i<6; i++)
        {
            AddTriangle
            (
                  center,
                  center + HexMetrics.Corners[i], 
                  center + HexMetrics.Corners[(i+1)%6]
            );
        }
    }

    private void CreateCellContent(HexCell cell) {
        Vector3 position = cell.Position;
        position.y += 2;
        features.AddFeature(position, cell.prefab);
    }

    private void AddColliderToMesh()
    {
        _collider.sharedMesh = _hexMesh;
    }
    
    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = _vertices.Count;
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);
    }
}
