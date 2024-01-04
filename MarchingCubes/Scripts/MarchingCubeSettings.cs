using UnityEngine;

[CreateAssetMenu(fileName = "Simulation settings", menuName = "marching_cubes/Simulation settings")]
public class MarchingCubeSettings: ScriptableObject
{
    [Header("Simulation settings")]
    public Vector3Int gridSize = new Vector3Int(32, 32, 32);
    public float scale = 1.0f;

    public bool showGrid = true;

    [Header("Features")]
    public bool usePerlin;
    public bool useSphere;
    public bool useCheese;

    [Header("Perlin Settings")]
    [Range(0, 1)] public float threshold = 0.4f;

    [Header("Sphere Settings")]
    [Min(0)] public float radius = 1;

}
