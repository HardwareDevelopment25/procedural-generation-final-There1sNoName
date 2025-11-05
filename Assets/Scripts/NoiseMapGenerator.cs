using JetBrains.Annotations;
using UnityEngine;

public static class NoiseMapGenerator
{
   
    // Generates a 2D noise map using Perlin noise with multiple octaves.
    public static float[,] GenerateNoise(float MapX, float MapY, float scale, int octaves, float persistence, float lacunarity, Vector2 offset, int seed)
    {
        float[,] noiseMap = new float[(int)MapX, (int)MapY];
        // Ensure scale is positive and non-zero
        if (scale <= 0)
        {
            scale = 0.0001f;

        }
        // Initialize min and max noise height for normalization
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        System.Random OctaveOffsets = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        // Generate random offsets for each octave
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = OctaveOffsets.Next(-100000, 100000) + offset.x;
            float offsetY = OctaveOffsets.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
        // Generate noise map
        for (int y = 0; y < MapY; y++)
        {
            for (int x = 0; x < MapX; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Combine multiple octaves of Perlin noise

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - (MapX / 2f)) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - (MapY / 2f)) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;

            



                }

                // Update min and max noise height
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }

                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseHeight);
            }
        }
        return noiseMap;
    }


    public static float[,] GenerateFallOffMap(int size, AnimationCurve Ac)
    {
        float[,] falloffMap = new float[size, size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xValue = x / (float)size * 2 - 1;
                float yValue = y / (float)size * 2 - 1;
                float value = Mathf.Max(Mathf.Abs(xValue), Mathf.Abs(yValue));
                falloffMap[x, y] = Ac.Evaluate(value);
            }
        }
        return falloffMap;

    }

}


