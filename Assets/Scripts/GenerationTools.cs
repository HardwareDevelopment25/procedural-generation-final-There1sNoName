using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class GenerationTools
{

    public static Mesh CreateTriangle(float TriangleSize)
    {
        Mesh triangle = new Mesh();
        Vector3[] vertices = new Vector3[3];
        {
            vertices[0] = new Vector3(0, 0, 0);
            vertices[1] = new Vector3(TriangleSize, 0, 0);
            vertices[2] = new Vector3(TriangleSize / 2, TriangleSize, 0);
        }



        Vector2[] uv = new Vector2[3];
        {
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(0.5f, 1);
        }

        int[] triangles = new int[3]
        {
            0, 1, 2
        };

        triangle.vertices = vertices;
        triangle.uv = uv;
        triangle.triangles = triangles;

        return triangle;
    }

    
    public static Mesh CreateSquare(float SquareSize)
    {
        Mesh Square = new Mesh();
        Vector3[] vertices = new Vector3[4];
        {
            vertices[0] = new Vector3(0, 0, 0);
            vertices[1] = new Vector3(SquareSize, 0, 0);
            vertices[2] = new Vector3(0, 0, SquareSize);
            vertices[3] = new Vector3(SquareSize, 0 , SquareSize);
        }
        Vector2[] uv = new Vector2[4];
        {
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(0, 1);
            uv[3] = new Vector2(1, 1);
        }
        int[] triangles = new int[6]
        {
            0, 2, 1,
            2, 3, 1
        };
        Square.vertices = vertices;
        Square.uv = uv;
        Square.triangles = triangles;
        return Square;

    }

    public static Mesh CreateCube(float CubeSize)
    {
        Mesh cube = new Mesh();
        Vector3[] vertices = new Vector3[8];
        {
            vertices[0] = new Vector3(0, 0, 0);
            vertices[1] = new Vector3(CubeSize, 0, 0);
            vertices[2] = new Vector3(0, CubeSize, 0);
            vertices[3] = new Vector3(CubeSize, CubeSize, 0);
            vertices[4] = new Vector3(0, 0, CubeSize);
            vertices[5] = new Vector3(CubeSize, 0, CubeSize);
            vertices[6] = new Vector3(0, CubeSize, CubeSize);
            vertices[7] = new Vector3(CubeSize, CubeSize, CubeSize);
        }
        int[] Faces = new int[36]
        {
            // Front
            0, 2, 1,
            2, 3, 1,
            // Back
            5, 7, 4,
            7, 6, 4,
            // Left
            4, 6, 0,
            6, 2, 0,
            // Right
            1, 3, 5,
            3, 7, 5,
            // Top
            2, 6, 3,
            6, 7, 3,
            // Bottom
            4, 0, 5,
            0, 1, 5
        };
        cube.vertices = vertices;
        cube.triangles = Faces;
        return cube;
    }



    public static Texture2D RenderBoolArrayAsTexture(bool[,] maze)
    {
        Texture2D texture = new Texture2D(maze.GetLength(0), maze.GetLength(1));

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {

                if (maze[x, y])
                {
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    texture.SetPixel(x, y, Color.black);
                }
            }
        }
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        return texture;
        
    }

    public static Texture2D RenderNoiseAsTexture(float[,] maze)
    {
        Texture2D texture = new Texture2D(maze.GetLength(0), maze.GetLength(1));

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                Color gradiant = new Color(maze[x, y], maze[x, y], maze[x, y]);
                texture.SetPixel(x, y, gradiant);

            }
        }
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        return texture;

    }
}
