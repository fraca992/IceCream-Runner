using UnityEngine;

namespace Common
{
    public class Cell
    {
        public int Points { get; set; }
        public Vector3 Coordinates { get; set; }

        public Cell(int p)
        {
            Points = p;
        }

        public Cell(int p, Vector3 c)
        {
            Points = p;
            Coordinates = c;
        }
    } 
}
