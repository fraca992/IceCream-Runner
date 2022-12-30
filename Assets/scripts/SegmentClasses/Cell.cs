using UnityEngine;


public class Cell
{
    public bool isOccupied { get; set; }
    public Vector3 Coordinates { get; set; }
    public float Size { get; private set; }
    public int Id { get; private set; }

    // Constructor
    public Cell(Vector3 c, float s = 0f, int id = 0)
    {
        Coordinates = c;
        isOccupied = false;
        Size = s;
        Id = id;
    }
} 

