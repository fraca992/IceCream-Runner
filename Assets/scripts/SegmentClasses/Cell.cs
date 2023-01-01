using UnityEngine;


public class Cell
{
    public bool isOccupied { get; set; }
    public Vector3 Coordinates { get; set; }
    public int Id { get; private set; }

    // Constructor
    public Cell(int id = 0, Vector3? c = null)
    {
        isOccupied = false;
        Id = id;
        Coordinates = c == null? Vector3.zero : c.Value;
    }
} 

