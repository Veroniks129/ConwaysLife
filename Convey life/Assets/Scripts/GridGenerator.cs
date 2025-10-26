using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellPrefab;

    public int width = 10;
    public int height = 10;

    private GameObject[,] grid;


    public void Initialize(GameObject cellPrefab, int width, int height) {
        this.cellPrefab = cellPrefab;
        this.width = width;
        this.height = height;
    }

    public void GenerateGrid()
    {
        grid = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                grid[x, y] = Instantiate(cellPrefab, position, Quaternion.identity);

                SpriteRenderer borderRenderer = grid[x, y].transform.Find("Border").GetComponent<SpriteRenderer>();
                SpriteRenderer interiorRenderer = grid[x, y].transform.Find("Interior").GetComponent<SpriteRenderer>();
                borderRenderer.sortingOrder = 0;
                interiorRenderer.sortingOrder = 1;
            }
        }
    }

    public GameObject[,] GetGrid()
    {
        return grid;
    }

    public void GenerateRandomStates(Cell[,] gridCell)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool isAlive = UnityEngine.Random.value > 0.5f;
                gridCell[x, y].state = isAlive ? "alive" : "dead";
            }
        }
    }
        
}

