using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour
{
    public int gridSize = 64;
    public float fillProbability = 0.4f;
    private int[,] grid;
    public int seed;
    System.Random rand;
    public float iterationDelay;
    public static Texture2D IntToBoolTexture(int[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        CreateBorder(grid,1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = (grid[x, y] == 1) ? Color.black : Color.white;
                texture.SetPixel(x, y, color);

                
            }
        }
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        return texture;
    }

    public static void CreateBorder(int[,] grid, int borderWidth)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < borderWidth || x >= width - borderWidth || y < borderWidth || y >= height - borderWidth)
                {
                    grid[x, y] = 1;
                }
            }
        }
    }

    public static int CheckNeighbors(int x , int y, int[,] grid)
    {
        int activeNeighbors = 0;

      if (x > 2  && x < grid.GetLength(0) - 1 && y > 2 && y < grid.GetLength(1) - 1)
        {
            if (grid[x - 1, y] == 1) // Left
                activeNeighbors++;

            if (grid[x + 1, y] == 1) // Right
                activeNeighbors++;

            if (grid[x, y - 1] == 1) // Down
                activeNeighbors++;

            if (grid[x, y + 1] == 1) // Up
                activeNeighbors++;

            if (grid[x - 1, y - 1] == 1) // Down-Left
                activeNeighbors++;

            if (grid[x - 1, y + 1] == 1) // Up-Left
                activeNeighbors++;

            if (grid[x + 1, y - 1] == 1) // Down-Right
                activeNeighbors++;

            if (grid[x + 1, y + 1] == 1) // Up-Right
                activeNeighbors++;

            else if (y == 2)
                activeNeighbors -= 3;

            else if (y == grid.GetLength(1) - 2)
                activeNeighbors -= 3;
            
            else if (x == 2)
                activeNeighbors -= 3;
            
            else if (x == grid.GetLength(0) - 2)
                activeNeighbors -= 3;

        }

        return activeNeighbors;

    }

    public IEnumerator<WaitForSeconds> SecondsDelay()
    {

        while (true)
        {
            GenerateCave(grid);
            GetComponent<Renderer>().material.mainTexture = IntToBoolTexture(grid);
            yield return new WaitForSeconds(iterationDelay);

        }
    }

    public static void GenerateCave(int[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int activeNeighbors = CheckNeighbors(x, y, grid);

                if ( activeNeighbors > 4)
                {
                    grid[x, y] = 1;
                }

                else if (activeNeighbors < 4)
                {
                    grid[x, y] = 0;
                }
             

            }
        }
    }

    public void RunGameOfLifeIteration(int[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        int[,] newGrid = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int activeNeighbors = CheckNeighbors(x, y, grid);
                if (grid[x, y] == 1)
                {
                    // Cell is alive
                    if (activeNeighbors < 2 || activeNeighbors > 3)
                    {
                        newGrid[x, y] = 0; // Cell dies
                    }
                    else
                    {
                        newGrid[x, y] = 1; // Cell lives
                    }
                }
                else
                {
                    // Cell is dead
                    if (activeNeighbors == 3)
                    {
                        newGrid[x, y] = 1; // Cell becomes alive
                    }
                    else
                    {
                        newGrid[x, y] = 0; // Cell remains dead
                    }
                }
            }
        }
        // Copy newGrid back to grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = newGrid[x, y];
            }
        }




    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rand = new System.Random(seed);
        grid = new int[gridSize, gridSize];
        
    }

    // Update is called once per frame
    void Start()
    {
        // Initialize the grid with random values based on fillProbability with seed   

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                grid[x, y] = (rand.NextDouble() < fillProbability) ? 1 : 0;
            }

            
        }

        StartCoroutine(SecondsDelay());

        



        GetComponent<Renderer>().material.mainTexture = IntToBoolTexture(grid);

    }
}
