using UnityEngine;

public class MarchingCubes : MonoBehaviour
{
    public MarchingCubeSettings settings;
    
    void Start()
    {
        Debug.Log(settings.gridSize);
        var meshWrapper = new MeshWrapper(GetComponent<MeshFilter>(), settings.gridSize, 1, 0.7f);
        meshWrapper.GenerateGrid();
        meshWrapper.GenerateMesh();
    }
}
