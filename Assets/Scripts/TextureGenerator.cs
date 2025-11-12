using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class TextureGenerator : MonoBehaviour
{
    public int imageSize = 64;
    private Texture2D texture;
    public int Scaler = 75;
    public AnimationCurve falloffCurve;
    public float[,] FinalMap;
    [Range(1, 6)]
    public int LOD = 1;
    [System.Serializable]
    

    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }

    public TerrainType[] terrainType;

    void Start()
    {
        texture = new Texture2D(imageSize, imageSize);
        //CreatePattern();



        GenerateColourTerrain();
        MeshFilter MF = gameObject.AddComponent<MeshFilter>();
        MF.mesh = MeshGenerator.GenerateTerrain(FinalMap, 20, falloffCurve,LOD).CreateMesh();

        MeshRenderer MR = gameObject.AddComponent<MeshRenderer>();
        MR.material.mainTexture = texture;




    }

    
    public void GenerateFractalTexture()
    {
        for (int y = 0; y < texture.width; y++)
        {
            for (int x = 0; x < texture.height; x++)
            {
                Color color = ((x & y) != 0 ? Color.white : Color.black);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
    }

    public void GenerateRandomTexture()
    {
        for (int y = 0; y < texture.width; y++)
        {
            for (int x = 0; x < texture.height; x++)
            {
                Color color = new Color(Random.value, Random.value, 1);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
    }

    public void GeneratePerlinTexture()
    {
        for (int y = 0; y < texture.width; y++)
        {
            for (int x = 0; x < texture.height; x++)
            {
                float xCoord = (float)x / texture.width * Scaler;
                float yCoord = (float)y / texture.height * Scaler;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Color color = new Color(sample, sample, sample);
                texture.SetPixel(x, y, color);
            }
        }
        

    }    

    public void CreatePattern()
    {
        float[,] nm = NoiseMapGenerator.GenerateNoise(imageSize, imageSize, Scaler, 4, 0.5f, 2, Vector2.zero, 42);

        for(int y = 0; y < texture.width; y++)
        {
            for(int x = 0; x < texture.height; x++)
            {
                float sample = nm[x, y];
                Color color = new Color(sample, sample, sample);
                texture.SetPixel(x, y, color);

            }
        }

     //   GenerateTerrain();
    }

    public void GenerateColourTerrain()
    {

        float[,] nm = NoiseMapGenerator.GenerateNoise(imageSize, imageSize, Scaler, 4, 0.5f, 2, Vector2.zero, 42);
        float[,] fallOffMap = NoiseMapGenerator.GenerateFallOffMap(imageSize, falloffCurve);

        FinalMap = new float[imageSize, imageSize];

        //merge noise map and falloff map
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                FinalMap[x, y] = Mathf.Clamp01(nm[x, y] - fallOffMap[x, y]);
            }
        }


        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                float currentHeight = FinalMap[x, y];
                Color color = Color.black;

                for (int i = 0; i < terrainType.Length; i++)
                {
                    if (currentHeight <= terrainType[i].height)
                    {
                        color = (terrainType[i].color);
                        break;
                    }
                }
                texture.SetPixel(x, y, color);
            }
        }

        


        texture.Apply();    
      
    }


}
