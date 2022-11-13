using UnityEngine;

namespace Common
{
    public class Cell
    {
        public bool isOccupied { get; set; }
        public Vector3 Coordinates { get; set; }

        public Cell(Vector3 c)
        {
            Coordinates = c;
            isOccupied = false;
        }
    } 
}
