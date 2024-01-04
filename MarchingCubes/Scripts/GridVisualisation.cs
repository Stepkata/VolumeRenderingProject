using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualisation : MonoBehaviour
{
    public MarchingCubeSettings settings;
    [SerializeField] private GameObject myPrefab;

    void Start()
    {
        var isoLevel = 0f;

        GridGenerator gen = new();

        if (settings.usePerlin){
                isoLevel = settings.threshold;
                gen = new GridGenerator(settings.scale, isoLevel);
                gen.GeneratePerlin(settings.gridSize);
            } else if (settings.useSphere) {
                isoLevel = settings.radius;
                gen = new GridGenerator(settings.scale, isoLevel);
                gen.GenerateSphere(settings.gridSize);
            } else if (settings.useCheese) {
                isoLevel = settings.radius;
                gen = new GridGenerator(settings.scale, isoLevel);
                gen.GenerateCheese(settings.gridSize);
            }
            
        if (settings.showGrid){
            var points = gen.points.GetData();
            var values = gen.values.GetData();
            for (int i=0; i<points.Count; i++){
                ShowGrid(points[i], values[i]);
            }
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
