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


// iMPORTANTE! questo deve avere dentro il GameObject Street anche
// quindi il Manager deve instanziare un array non di street, ma una Lista di tipo StreetProperties (rinomina a Street?) e quando distrugge, rimuove elemento da Lista.