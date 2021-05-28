using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject groundPrefab;
    public GameObject wallPrefab;
    public float fragmentation;
    public int fixGround;
    public int fixWall;
    public int fixDirect = 6;

    static public int[,] MAP;


    void Awake() // добавить генерацию двух пустых площадок для баз
    {
        width = 21;// сделать настраиваемым в контроллере
        height = 21;

        GameObject Maze = new GameObject("Maze");
        Transform MAZE = Maze.transform;
        CreateMaze();
        FixedWall();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                if (MAP[i, j] == 1 || MAP[i, j] == 20)
                {
                    GameObject go = Instantiate<GameObject>(wallPrefab);
                    go.transform.SetParent(MAZE);
                    go.transform.position = new Vector3(i, j, 0);
                }
                else
                {
                    GameObject go = Instantiate<GameObject>(groundPrefab);
                    go.transform.SetParent(MAZE);
                    go.transform.position = new Vector3(i, j, 0);
                }
            }
        }
    }    

    private void CreateMaze()
    {
        MAP = new int[width, height];
        GameObject Maze = new GameObject("Maze");
        Transform MAZE = Maze.transform;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                {
                    MAP[i, j] = 10;
                }

                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > 0.1f)
                    {
                        MAP[i, j] = 1;

                        int a = Random.value < fragmentation ? 0 : (Random.value < fragmentation ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < fragmentation ? -1 : 1);
                        MAP[i + a, j + b] = 1;
                    }
                }

                if (MAP[i, j] != 1) MAP[i, j] = 0;
            }
        }
    }

    private void FixedGround()
    {
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < height - 1; j++)
            {
                if (MAP[i, j] == 0)
                {
                    int k = 0;
                    int a = MAP[i - 1, j];
                    int b = MAP[i + 1, j];
                    int c = MAP[i, j - 1];
                    int d = MAP[i, j + 1];
                    k = (a + b + c + d);

                    for (int l = k; l > fixGround; l--)
                    {
                        float rnd = Random.value;
                        if (MAP[i, j - 1] == 1 && rnd > 0.75)
                        {
                            int x = i;
                            int y = j - 1;
                            TileReplacement(x, y);
                        }
                        else if (MAP[i + 1, j] == 1 && rnd > 0.5 && rnd < 0.75)
                        {
                            int x = i + 1;
                            int y = j;
                            TileReplacement(x, y);
                        }
                        else if (MAP[i, j + 1] == 1 && rnd > 0.25 && rnd < 0.5)
                        {
                            int x = i;
                            int y = j + 1;
                            TileReplacement(x, y);
                        }
                        else if (MAP[i - 1, j] == 1)
                        {
                            int x = i - 1;
                            int y = j;
                            TileReplacement(x, y);
                        }
                    }
                }
            }
        }
    }

    private void FixedWall()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1 || i == width || j == height)
                {
                    MAP[i, j] = 1;
                }
            }
        }
    }

    private void TileReplacement(int x, int y)
    {
        if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
        {
            if (MAP[x, y] == 0)
            {
                MAP[x, y] = 1;
            }
            else if (MAP[x, y] == 1)
            {
                MAP[x, y] = 0;
            }
            else if (MAP[x, y] == 10) return;
        }
        else return;
    }

    public static int[,] GetMap ()
    {
        return MAP;
    }

    public static int width { get; set; }
    public static int height { get; set; }
}
