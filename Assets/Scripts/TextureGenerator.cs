using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class TextureGenerator : MonoBehaviour
{
    public int imageSize = 64;
    private Texture2D texture;
    public int Scaler = 75;
    public AnimationCurve falloffCurve;
    public Texture2D results;
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
        float[,] nm = NoiseMapGenerator.GenerateNoise(imageSize, imageSize, Scaler, 4, 0.5f, 2, Vector2.zero, 42);

        results = GenerateColourTerrain(nm);
        MeshFilter MF = gameObject.AddComponent<MeshFilter>();
        MF.mesh = MeshGenerator.GenerateTerrain(nm, 20).CreateMesh();

        MeshRenderer MR = gameObject.AddComponent<MeshRenderer>();
        MR.material.mainTexture = results;




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

    public Texture2D GenerateColourTerrain(float[,] nm)
    {
        
        float[,] fallOffMap = NoiseMapGenerator.GenerateFallOffMap(imageSize, falloffCurve);
        Texture2D texture = new Texture2D(nm.GetLength(0), nm.GetLength(1));
        //merge noise map and falloff map
        for (int y = 0; y < texture.width; y++)
        {
            for (int x = 0; x < texture.height; x++)
            {
                nm[x, y] = Mathf.Clamp01(nm[x, y] - fallOffMap[x, y]);
            }
        }


        for (int y = 0; y < texture.width; y++)
        {
            for (int x = 0; x < texture.height; x++)
            {
                float currentHeight = nm[x, y];
                for (int i = 0; i < terrainType.Length; i++)
                {
                    if (currentHeight <= terrainType[i].height)
                    {
                        texture.SetPixel(x, y, terrainType[i].color);
                        break;
                    }
                }
            }
        }

        


        texture.Apply();    
        return texture;
    }

}
