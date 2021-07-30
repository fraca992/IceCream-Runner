using UnityEngine;

namespace Properties
{
    public class StreetProperties
    {
        public int StreetBudget { get; set; }
        public CellProperties[] StreetCells { get; set; }


        public class CellProperties
        {
            public Vector3 CellCoordinates { get; set; }
            public int CellValue { get; set; }
        }
    }
}