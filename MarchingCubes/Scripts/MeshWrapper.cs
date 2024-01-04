using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshWrapper {

    private float _isoLevel;
    private List<int> _triangles;
    private List<Vector3> _vertices;
    private List<GridCell> _gridCells;

    public List<Vector3> points;
    public List<float> values;


    public MeshWrapper() {
        Debug.Log("MeshWrapper constructor");
        _triangles = new List<int>();
        _vertices = new List<Vector3>();
        _gridCells = new List<GridCell>();

        points = new List<Vector3>();
        values = new List<float>();

    }

    public void SetGrid(float isoLevel, List<GridCell> gridc){
        _isoLevel = isoLevel;
        _gridCells = gridc;
    }

    public Mesh GenerateMesh() {
        Debug.Log(_gridCells.Count);
        foreach (var cell in _gridCells){
            ProcessGridCell(cell);
        }

        var mesh = new Mesh
        {
            vertices = _vertices.ToArray(),
            triangles = _triangles.ToArray()
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        mesh.name = "Marching Cubes Mesh";
        
        Debug.Log("Mesh generated");
        return mesh;
    }

    private void ProcessGridCell(GridCell cell) {
        var vertexList = new Vector3[12];
        var configurationID = 0;
        for (var i = 0; i < 8; i++)
            if (cell.Values[i] < _isoLevel)
                configurationID |= (int)Math.Pow(2,i);
        
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
        float t = (_isoLevel - val1) / (val2 - val1);
        return point1 + t * (point2-point1);
    }

    private Vector3 InterpolateVectorWeak(Vector3 point1, Vector3 point2, float val1, float val2) {
        return (point2+point1)/2;
    }
}
