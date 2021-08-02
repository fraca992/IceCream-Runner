using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class Street
    {
        public GameObject StreetObject { get; set; }
        public int Budget { get; set; }
        public List<CellProperties> Cells { get; set; } = new List<CellProperties>();

        public Street(GameObject street, int budget, List<CellProperties> cells)
        {
            Budget = budget;
            StreetObject = street;
            Cells = cells;
        }



        public class CellProperties
        {
            public Vector3 Coordinates { get; set; }
            public int Value { get; set; }

            public CellProperties(Vector3 coords, int val)
            {
                Coordinates = coords;
                Value = val;
            }
        }
    }
}