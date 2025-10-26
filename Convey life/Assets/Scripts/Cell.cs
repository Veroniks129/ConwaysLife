using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public string state = "dead"; // alive, dead

    public Cell(string state) { this.state = state; }

    public Cell Clone()
    {
        return new Cell(this.state);
    }
}
