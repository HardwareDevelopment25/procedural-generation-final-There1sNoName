using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    System.Random rand;
    public int size = 10;
    public int seed = 0;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    private bool[,] maze;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rand = new System.Random(seed);
        maze = new bool[size, size];
        GenerateMaze();
        DrawMaze();
    }

    void DrawMaze()
    {
        for(int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                
                if (maze[x, y])
                {
                    GameObject.Instantiate(floorPrefab, new Vector3(x,0,y), Quaternion.identity);
                }
                else
                {
                    GameObject.Instantiate(wallPrefab, new Vector3(x,0,y), Quaternion.identity);
                }
            }
        }
    }
    private void GenerateMaze()
    {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        Vector2Int current = new Vector2Int(0, 0);

        maze[current.x, current.y] = true;

        stack.Push(current);

        while (stack.Count > 0)
        {
            current = stack.Pop();

            List<Vector2Int> neighbors = new List<Vector2Int>();

            if (current.x > 1 && !maze[current.x - 2, current.y])
            {
                neighbors.Add(new Vector2Int(current.x - 2, current.y));
            }

            if (current.x < size - 2 && !maze[current.x + 2, current.y])
            {
                neighbors.Add(new Vector2Int(current.x + 2, current.y));
            }

            if (current.y > 1 && !maze[current.x, current.y - 2])
            {
                neighbors.Add(new Vector2Int(current.x, current.y - 2));
            }

            if (current.y < size - 2 && !maze[current.x, current.y + 2])
            {
                neighbors.Add(new Vector2Int(current.x, current.y + 2));

            }

            if (neighbors.Count > 0)
            {
                stack.Push(current);
                Vector2Int chosen = neighbors[rand.Next(0, neighbors.Count)];

                if (chosen.x == current.x)
                {
                    maze[chosen.x, current.y + 1] = true;
                }
                else
                {
                    maze[chosen.x + 1, chosen.y] = true;


                }

                maze[chosen.x, chosen.y] = true;

                stack.Push(chosen);

            }
        }

    }
}
