using System.Collections.Generic;
using UnityEngine;

namespace Code.Hexagonal {
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class HexMesh : MonoBehaviour {
    
        private Mesh _mesh;
        private List<Vector3> _vertices;
        private List<int> _triangles;
        private List<Color> _colors;
        private MeshCollider _collider;

        private void Awake() {
            CreateMeshAndCollider();
            InstantiateLists();
        }

        private void CreateMeshAndCollider() {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
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
            AddColliderToMesh();
        }

        private void ClearData() {
            _mesh.Clear();
            _vertices.Clear();
            _triangles.Clear();
            _colors.Clear();
        }

        private void Triangulate(HexCell hexCell) {
            if (hexCell == null) return;
        
            Vector3 center = hexCell.transform.localPosition;
            for (int i = 0; i < HexMetrics.NumOfTrianglesInHexagon; i++) {
                AddTriangleAndColorIt(hexCell, center, i);
            }
        }

        private void AddTriangleAndColorIt(HexCell hexCell, Vector3 center, int i) {
            AddTriangle(
                center,
                center + HexMetrics.Corners[i],
                center + HexMetrics.Corners[(i + 1) % HexMetrics.NumOfTrianglesInHexagon]
            );
            AddTriangleColor(hexCell.GetCellColor());
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
            SetVerticesToMesh();
            SetTrianglesToMesh();
            SetColorsToMesh();
            _mesh.RecalculateNormals();
        }

        private void SetColorsToMesh() {
            _mesh.colors = _colors.ToArray();
        }

        private void SetTrianglesToMesh() {
            _mesh.triangles = _triangles.ToArray();
        }

        private void SetVerticesToMesh() {
            _mesh.vertices = _vertices.ToArray();
        }

        private void AddColliderToMesh() {
            _collider.sharedMesh = _mesh;
        }
    }
}
