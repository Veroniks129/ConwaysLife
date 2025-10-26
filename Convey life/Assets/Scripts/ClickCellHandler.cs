using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCellHandler
{
    private Cell[,] gridCell;
    
    public ClickCellHandler(Cell[,] gridCell)
    {
        this.gridCell = gridCell;
    }

    public void Process()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int gridX = Mathf.FloorToInt(mousePos.x + 0.5f);
        int gridY = Mathf.FloorToInt(mousePos.y + 0.5f);

        if (gridX >= gridCell.GetLength(0) || gridX < 0 ||
            gridY >= gridCell.GetLength(1) || gridY < 0)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            gridCell[gridX, gridY].state = "alive";
        }
        else if (Input.GetMouseButtonDown(1))
        {
            gridCell[gridX, gridY].state = "dead";
        }

    }
}
