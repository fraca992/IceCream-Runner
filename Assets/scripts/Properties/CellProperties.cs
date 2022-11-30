using UnityEngine;


public class CellProperties
{
    public bool isOccupied { get; set; }
    public Vector3 Coordinates { get; set; }

    // Constructor
    public CellProperties(Vector3 c)
    {
        Coordinates = c;
        isOccupied = false;
    }
} 

