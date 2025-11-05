using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MikeMiner : MonoBehaviour
{
    private int CaveWidth;
    private int CaveHeight;
    private bool[,] CaveGrid;
    private Vector2Int minerPos;
    public int MinerMoves;
    public GameObject Wall;
    public GameObject Floor;
    System.Random rand;
    public int seed;
    private List<int> directions = new List<int>(0);
    Stack<Vector2Int> Path = new Stack<Vector2Int>();

    private enum minerDirection
    {
        up,
        down,
        left,
        right
    }

    private void Awake()
    {
        rand = new System.Random(seed);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        CaveWidth = 50;
        CaveHeight = 50;

       
        CaveGrid = new bool[CaveWidth, CaveHeight];
        minerPos = new Vector2Int(CaveWidth / 2, CaveHeight / 2);

        CaveGrid[minerPos.x, minerPos.y] = false;

        GenerateCave();
        MoveMiner();
        DrawCubes();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateCave()
    {
        for (int x = 0; x < CaveWidth; x++)
        {
            for (int y = 0; y < CaveHeight; y++)
            {
                CaveGrid[x, y] = true;
            }
        }


    }

    private void MoveMiner()
    {
        while (Path.Count > 0)
        {
            GetNeighbour();

            if (directions.Count > 0)
            {
                int chosenDir = directions[rand.Next(0, directions.Count - 1)];

                minerDirection dir = (minerDirection)rand.Next(0, 4);

                switch (dir)
                {
                    case minerDirection.up:
                        minerPos.y += 2;
                        CaveGrid[minerPos.x, minerPos.y - 1] = true;
                        CaveGrid[minerPos.x, minerPos.y] = true;
                        break;

                    case minerDirection.down:
                        minerPos.y -= 2;
                        CaveGrid[minerPos.x, minerPos.y + 1] = true;
                        CaveGrid[minerPos.x, minerPos.y] = true;
                        break;

                    case minerDirection.left:
                        minerPos.x -= 2;
                        CaveGrid[minerPos.x + 1, minerPos.y] = true;
                        CaveGrid[minerPos.x, minerPos.y] = true;
                        break;

                    case minerDirection.right:
                        minerPos.x += 2;
                        CaveGrid[minerPos.x - 1, minerPos.y] = true;
                        CaveGrid[minerPos.x, minerPos.y] = true;
                        break;



                }




            }
            else
            {
              minerPos = Path.Pop();
            }
                CaveGrid[(int)minerPos.x, (int)minerPos.y] = false;
        }
        
    }

    private void DrawCubes()
    {
        for (int x = 0; x < CaveWidth; x++)
        {
            for (int y = 0; y < CaveHeight; y++)
            {
                if (CaveGrid[x, y] == true)
                {
                 GameObject.Instantiate(Wall,new Vector3 (x,0,y), Quaternion.identity);
                }
                else
                {
                 GameObject.Instantiate(Floor,new Vector3 (x,0,y), Quaternion.identity);
                }
            }
        }
    }

    private void GetNeighbour()
    {
        if (minerPos.y + 2 < CaveHeight - 1 && CaveGrid[minerPos.x, minerPos.y + 2] == false)
        {
            directions.Add(0);
        }
        if (minerPos.y - 2 > 1 && CaveGrid[minerPos.x, minerPos.y - 2] == false)
        {
            directions.Add(1);
        }
        if (minerPos.x - 2 > 1 && CaveGrid[minerPos.x - 2, minerPos.y] == false)
        {
            directions.Add(2);
        }
        if (minerPos.x + 2 < CaveWidth - 1 && CaveGrid[minerPos.x + 2, minerPos.y] == false)
        {
            directions.Add(3);
        }





    }
}


