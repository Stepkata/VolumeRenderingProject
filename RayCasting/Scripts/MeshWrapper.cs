using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshWrapper {

    private float _scale;
    private float _isoLevel;
    private Vector3 _gridSize;
    private List<int> _triangles;
    private List<Vector3> _vertices;
    private List<GridCell> _gridCells;
    private MeshFilter _filter;

    public MeshWrapper(MeshFilter filter, Vector3 gridSize, float scale, float isoLevel) {
        Debug.Log("MeshWrapper constructor");
        _triangles = new List<int>();
        _vertices = new List<Vector3>();
        _gridCells = new List<GridCell>();
        _gridSize = gridSize;
        _isoLevel = isoLevel;
        _scale = scale;
        _filter = filter;
    }

    public void GenerateGrid() {
        for (var x = 0; x < _gridSize.x; x++) {
            for (var y = 0; y < _gridSize.y; y++) {
                for (var z = 0; z < _gridSize.z; z++) {
                    var cell = new GridCell {
                        Points = new Vector3[8],
                        Values = new float[8]
                    };
                    for (var i = 0; i < 8; i++) {
                        var vertex = new Vector3(x + (i & 1), y + ((i & 2) >> 1), z + ((i & 4) >> 2));
                        cell.Points[i] = vertex * _scale;
                        cell.Values[i] = Perlin.PerlinNoise3D(vertex / _gridSize.x);
                    }

                    _gridCells.Add(cell);
                }
            }
        }
    }

    public void GenerateMesh() {
        foreach (var cell in _gridCells)
            _processGridCell(cell);
        
        var mesh = new Mesh();

        // Print first 10 vertices joined with comma
        Debug.Log("Vertices: " + string.Join(", ", _vertices.GetRange(0, 30)));
        // Print first 10 triangles joined with comma
        Debug.Log("Triangles: " + string.Join(", ", _triangles.GetRange(0, 30)));

        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        mesh.name = "Marching Cubes Mesh";
        
        _filter.mesh = mesh;
        Debug.Log("Mesh generated");
    }

    private void _processGridCell(GridCell cell) {
        var vertexList = new Vector3[12];
        var configurationID = 0;
        for (var i = 0; i < 8; i++)
            if (cell.Values[i] < _isoLevel)
                configurationID |= 1 << i;
        
        // If there are no edges, then we can skip this cell, since there is no mesh
        if (CubeConversionTables.EdgesForConfiguration[configurationID] == 0)
            return;

        var edges = new Vector2Int[] {
            new (0, 1), new (1, 2), new (2, 3),
            new (3, 0), new (4, 5), new (5, 6), 
            new (6, 7), new (7, 4), new (0, 4), 
            new (1, 5), new (2, 6), new (3, 7)
        };

        for (var i = 0; i < 12; i++) {
            var edge = edges[i];
            if ((CubeConversionTables.EdgesForConfiguration[configurationID] & (1 << i)) != 0) {
                vertexList[i] = InterpolateVector3(
                    cell.Points[edge.x], 
                    cell.Points[edge.y], 
                    cell.Values[edge.x], 
                    cell.Values[edge.y]
                );
            }
        }

        var vertexCount = 0;
        var tightVertexList = new Vector3[12];
        var localRemap = new int[12];
        Array.Fill(localRemap, -1);

        var triangleTable = CubeConversionTables.TrianglesForConfiguration[configurationID];

        for (var i = 0; triangleTable[i] != -1; i++) {
            if (localRemap[triangleTable[i]] != -1)
                continue;
            
            localRemap[triangleTable[i]] = vertexCount;
            tightVertexList[vertexCount] = vertexList[triangleTable[i]];
            vertexCount++;
        }
        
        Debug.Log("Configuration ID: " + configurationID + "\n" + 
                  "Vertex list" + string.Join(", ", vertexList) + "\n" +    
                  "Tight vertex list: " + string.Join(", ", tightVertexList) + "\n" +
                "Local remap: " + string.Join(", ", localRemap) + "\n" +
                "Triangle table: " + string.Join(", ", triangleTable));

        var localTriangles = new List<int>();
        
        for (var i = 0; triangleTable[i] != -1; i += 3) {
            localTriangles.Add(localRemap[triangleTable[i]] + _vertices.Count);
            localTriangles.Add(localRemap[triangleTable[i + 1]] + _vertices.Count);
            localTriangles.Add(localRemap[triangleTable[i + 2]] + _vertices.Count);
        }
        
        // Add triangles and vertices to global list
        _triangles.AddRange(localTriangles);
        for (var i = 0; i < vertexCount; i++)
            _vertices.Add(tightVertexList[i]);
    }

    private Vector3 InterpolateVector3(Vector3 point1, Vector3 point2, float val1, float val2) {
        return Vector3.Lerp(point1, point2, 0.5f);
    }

    private struct GridCell {
        public Vector3[] Points { get; set; }
        public float[] Values { get; set; }
    }
}
