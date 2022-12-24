using UnityEngine;


public class CellProperties
{
    public bool isOccupied { get; set; }
    public Vector3 Coordinates { get; set; }
    public float Size { get; set; }

    // Constructor
    public CellProperties(Vector3 c, float s = 0f)
    {
        Coordinates = c;
        isOccupied = false;
        Size = s;
    }
} 

