using UnityEngine;

[CreateAssetMenu(fileName = "Simulation settings", menuName = "Marching Cubes/Simulation settings")]
public class MarchingCubeSettings: ScriptableObject
{
    [Header("Simulation settings")]
    public Vector3Int gridSize = new (32, 32, 32);
}
