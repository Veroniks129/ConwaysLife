using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameProcess
{
    private Cell[,] gridCell;

    public int width;
    public int height;

    public GameProcess(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.gridCell = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridCell[x, y] = new Cell("dead");
            }
        }
    }

    public void Process()
    {
        Cell[,] current_states = new Cell[gridCell.GetLength(0), gridCell.GetLength(1)];

        try
        {
            System.Array.Copy(gridCell, current_states, gridCell.Length);
        }
        catch (Exception e)
        {
            Debug.LogError($"GameProcess.Process() array copy: {e.Message}");
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int alive_cnt = 0;
                try
                {
                    alive_cnt = countAlive(current_states, x, y);
                }
                catch (Exception e)
                {
                    Debug.LogError($"GameProcess.Process() countAlive(): {e.Message} ");
                }
                
                if (alive_cnt < 2 || alive_cnt > 3)
                {
                    gridCell[x, y].state = "dead";
                }
                else if (alive_cnt == 3)
                {
                    gridCell[x, y].state = "alive";
                }
            }
        }
    }

    int countAlive(Cell[,] current, int x, int y)
    {
        int live_cnt = 0;
        
        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int x_ = x + dx;
                int y_ = y + dy;
                if (0 <= x_ && x_ < width && 0 <= y_ && y_ < height)
                {
                    live_cnt += Convert.ToInt32(current[x_, y_].state == "alive");
                }
            }
        }

        return live_cnt;
    }

    public Cell[,] GetGridCell()
    {
        return gridCell;
    }
}
