using Unity.VisualScripting;
using UnityEngine;

public class ShapeCreator : MonoBehaviour
{
    public TextureGenerator TerrainTextureGenerator;

    public int SizeOfGrid = 128;
    public float HeightMultiplier = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshFilter mf = this.AddComponent<MeshFilter>();
        MeshRenderer mr = this.AddComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.mainTexture = TerrainTextureGenerator.results;
        mr.material = mat;
        
        float[,] NoiseMap = NoiseMapGenerator.GenerateNoise(SizeOfGrid, SizeOfGrid, 10, 1, 5, 1, Vector2.zero, 42);
        mf.mesh = MeshGenerator.GenerateTerrain(NoiseMap, HeightMultiplier).CreateMesh();
    }


}
