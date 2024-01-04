using UnityEngine;

public class Perlin {
    public static float PerlinNoise3D(Vector3 pos)
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

    private static float Perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    }
}
