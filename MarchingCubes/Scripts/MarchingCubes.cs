using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MarchingCubes : MonoBehaviour
{
    public MarchingCubeSettings settings;
    [SerializeField] private readonly GameObject myPrefab;
    public float rotationSpeed = 45f;

    private GameObject parentObject;

    public void Start()
    {
        List<List<GridCell>> gridCells = new();
        var isoLevel = 0f;

        GridGenerator gen = new();
        parentObject = new("ParentObject");

        if (settings.usePerlin){
                isoLevel = settings.threshold;
                gen = new GridGenerator(settings.scale, isoLevel);
                gridCells = gen.GeneratePerlin(settings.gridSize).GetData();
            } else if (settings.useSphere) {
                isoLevel = settings.radius;
                gen = new GridGenerator(settings.scale, isoLevel);
                gridCells = gen.GenerateSphere(settings.gridSize).GetData();
            } else if (settings.useCheese) {
                isoLevel = settings.radius;
                gen = new GridGenerator(settings.scale, isoLevel);
                gridCells = gen.GenerateCheese(settings.gridSize).GetData();
            }
        
        List<GameObject> generatedObjects = new();

        // Set the number of objects you want to generate
        int numberOfObjects = gridCells.Count;

        // Instantiate objects and assign the generated mesh
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Create a new GameObject
            GameObject newObj = new("GeneratedObject" + i);
            newObj.transform.parent = parentObject.transform;

            // Attach a MeshFilter component to the GameObject
            MeshFilter meshFilter = newObj.AddComponent<MeshFilter>();

            // Assign the generated mesh to the MeshFilter
            var meshWrapper = new MeshWrapper();
            meshWrapper.SetGrid(isoLevel, gridCells[i]);
            
            meshFilter.mesh = meshWrapper.GenerateMesh();

            MeshRenderer meshRenderer = newObj.AddComponent<MeshRenderer>();

            Material newMaterial = new(Shader.Find("Sprites/Diffuse"))
            {
                color = Color.red
            };

            meshRenderer.material = newMaterial;

            // Add the object to the list
            generatedObjects.Add(newObj);
        }

        if (settings.showGrid){
            var points = gen.points.GetData();
            var values = gen.values.GetData();
            for (int i=0; i<points.Count; i++){
                ShowGrid(points[i], values[i]);
            }
        }

    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.A)){
            parentObject.transform.Rotate( Vector3.up, rotationSpeed * Time.deltaTime);
       } else if (Input.GetKey(KeyCode.D)){
            parentObject.transform.Rotate( Vector3.down, rotationSpeed * Time.deltaTime);
       }  else if (Input.GetKey(KeyCode.W)){
            parentObject.transform.Rotate( Vector3.left, rotationSpeed * Time.deltaTime);
       }  else if (Input.GetKey(KeyCode.S)){
            parentObject.transform.Rotate( Vector3.right, rotationSpeed * Time.deltaTime);
       } 
    }

    private void ShowGrid( List<Vector3> points, List<float> values ){
        for (int i = 0; i < points.Count; i++)
        {
            var point = points[i];
            var value = values[i];
        
            // Instantiate sphere prefab
            GameObject sphere = Instantiate(myPrefab, point, Quaternion.identity);

            // Set color based on the threshold
            Renderer renderer = sphere.GetComponent<Renderer>();
            var th = settings.usePerlin ? settings.threshold : settings.radius;
            if (value > th)
            {
                renderer.material.color = Color.black;
            }
            else
            {
                renderer.material.color = Color.white;
            }
        }


    }
}
