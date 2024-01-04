using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GridGenerator
{
    private float _scale;
    private float _isoLevel;

    public LongList<Vector3> points;
    public LongList<float> values;

    public GridGenerator(){
        _scale = 1;
        _isoLevel = 1;
        points = new LongList<Vector3>();
        values = new LongList<float>();
    }
    public GridGenerator(float s, float il){
        _scale = s;
        _isoLevel = il;
        points = new LongList<Vector3>();
        values = new LongList<float>();
    }
    public float PerlinNoise3D(Vector3 pos, Vector3 pos0)
    {
        pos.y += 1;
        pos.z += 2;
        var xy = Perlin3DFixed(pos.x, pos.y);
        var xz = Perlin3DFixed(pos.x, pos.z);
        var yz = Perlin3DFixed(pos.y, pos.z);
        var yx = Perlin3DFixed(pos.y, pos.x);
        var zx = Perlin3DFixed(pos.z, pos.x);
        var zy = Perlin3DFixed(pos.z, pos.y);

        return xy * xz * yz * yx * zx * zy;
    }

    public float Shpere3D(Vector3 position, Vector3 pos0){
        return (float)Math.Sqrt(Math.Pow(position.x-pos0.x, 2) + Math.Pow(position.y-pos0.y, 2) +  Math.Pow(position.z-pos0.z, 2));
    }

    private float Perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    }

    public float SwissCheese(Vector3 position, Vector3 pos0){
        return (float)Math.Cos(Math.Sqrt(Math.Pow(position.x, 2) + Math.Pow(position.y, 2) +  Math.Pow(position.z, 2)));
    }

    private LongList<GridCell> GenerateMathematicalGrid(Vector3 gridSize, Func<Vector3, Vector3, float> f, float modify){
        var gridCells = new LongList<GridCell>();
        for (var x = 0; x < gridSize.x; x++) {
            for (var y = 0; y < gridSize.y; y++) {
                for (var z = 0; z < gridSize.z; z++) {
                    var cell = new GridCell {
                        Points = new Vector3[8],
                        Values = new float[8]
                    };

                    var vertices = new Vector3[8] {
                        new (x, y, z),
                        new (x + 1, y, z),
                        new (x + 1, y + 1, z),
                        new (x, y + 1, z),
                        new (x, y, z + 1),
                        new (x + 1, y, z + 1),
                        new (x + 1, y + 1, z + 1),
                        new (x, y + 1, z + 1)
                    };
                    
                    for (var i = 0; i < 8; i++) {
                        cell.Points[i] = vertices[i] * _scale;
                        float value = f(vertices[i]*modify, new Vector3(gridSize.x/2, gridSize.y/2, gridSize.z/2));
                        cell.Values[i] = value;
                        values.Add( value);
                        points.Add(vertices[i] * _scale);
                    }
                    gridCells.Add(cell);
                }
            }
        }
        return gridCells;
    }

    public LongList<GridCell> GeneratePerlin(Vector3 gridSize){
        return GenerateMathematicalGrid(gridSize, PerlinNoise3D, 1/gridSize.x * 0.5f);
    }

    public LongList<GridCell> GenerateCheese(Vector3 gridSize){
        return GenerateMathematicalGrid(gridSize, SwissCheese, 2f);
    }

    public LongList<GridCell> GenerateSphere(Vector3 gridSize){
        return GenerateMathematicalGrid(gridSize, Shpere3D, 1f);
    }

}
