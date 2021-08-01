using UnityEngine;
using Common;
using Properties;

namespace Manager
{
    public class StreetManager : Object
    {
        // Manager for steet Spawning, Destroying and Moving
        private GameObject[] streets;
        private GameObject streetPrefab;
        private float streetLength;
        private int newestStreetIndex = 0;

        // Constructor
        public StreetManager(int maxStreets, GameObject streetPrefab)
        {
            streets = new GameObject[maxStreets];
            streets[0] = GameObject.Find("Street"); //REVIEW: may remove the first street and instantiate this too at runtime
            this.streetPrefab = streetPrefab;
            streetLength = Tools.GetSize(streetPrefab, 'z');
        }

        // Methods
        #region Street Methods
        public int SpawnStreetIfNull()
        {
            // Instantiate new street segments
            Vector3 nextStreetPosition = new Vector3();

            for (int i = 0; i < streets.Length; i++)
            {
                if (streets[i] == null)
                {
                    nextStreetPosition.z = streets[newestStreetIndex].transform.position.z + streetLength;
                    streets[i] = Instantiate(streetPrefab, nextStreetPosition, Quaternion.identity);
                    newestStreetIndex = i;
                    
                    return newestStreetIndex;
                }
            }
            return null;
        }

        public void DestroyStreetIfOld(int bufferStreetNumber = 2)
        {
            // Destroy old street segments behind the player
            float bufferLength = -bufferStreetNumber * streetLength;
            for (int i = 0; i < streets.Length; i++)
            {
                if (streets[i].transform.position.z < bufferLength)
                {
                    Destroy(streets[i]);
                }
            }
        }

        public void MoveStreets(float streetSpeed)
        {
            // Move street segments
            foreach (GameObject street in streets)
            {
                street?.transform.Translate(0, 0, -streetSpeed);
            }
        }
        #endregion

        #region Cell Methods
        public void InitializeCells(int streetIndex, int xCellNum, int cellNumber, int startValue)
        {
            // creating the cell coordinates and initializing cell value with the starting value
            StreetProperties.CellProperties[] cells;
            Vector3[] cellCoords;
            int[] cellValues;

            streets[streetIndex].StreetCells = Tools.InitializeArray<StreetProperties.CellProperties>(cellNumber);
            cellCoords = GetCellCoordinates(street, xCellNum, cellNumber);
            cellValues = GetCellValues(startValue, cellNumber);

            for (int i = 0; i < cells.Length; i++)
            {
                streets[streetIndex].StreetCells.CellCoordinates = cellCoords[i];
                streets[streetIndex].StreetCells.CellValue = cellValues[i];
            }
        }

        public Vector3[] GetCellCoordinates(int streetIndex, int xCellNum, int cellNumber)
        {
            // Returns an array with the coordinates of each cell in the street segment
            // IMPORTANT: a sidewalk MUST have leght as a multiple of width, or the cells won't be square
            Vector3[] cellCoords = new Vector3[cellNumber];
            Vector3[] leftSidewalkCoords = new Vector3[cellNumber / 2];
            Vector3[] rightSidewalkCoords = new Vector3[cellNumber / 2];
            GameObject street = streets[streetIndex];

            float streetWidth = Tools.GetSize(street.transform.GetChild(0).gameObject, 'x');
            float SidewalkHeight = Tools.GetSize(street.transform.GetChild(0).GetChild(0).gameObject, 'y');
            float cellWidth = Tools.GetSize(street.transform.GetChild(0).GetChild(0).gameObject, 'x') / xCellNum;

            int zIndex = 0;
            int xIndex = 0;

            for (int i = 0; i < leftSidewalkCoords.Length; i++)
            {
                zIndex = i / xCellNum;
                xIndex = i - (zIndex * xCellNum);

                leftSidewalkCoords[i].x += -(streetWidth / 2f + (xIndex + 0.5f) * cellWidth);
                leftSidewalkCoords[i].y += SidewalkHeight / 2f;
                leftSidewalkCoords[i].z += (zIndex + 0.5f) * cellWidth;

                rightSidewalkCoords[i].x += streetWidth / 2f + (xIndex + 0.5f) * cellWidth;
                rightSidewalkCoords[i].y += SidewalkHeight / 2f;
                rightSidewalkCoords[i].z += (zIndex + 0.5f) * cellWidth;
            }

            leftSidewalkCoords.CopyTo(cellCoords, 0);
            rightSidewalkCoords.CopyTo(cellCoords, leftSidewalkCoords.Length);

            return cellCoords;
        }

        public int[] GetCellValues(int value, int cellNumber)
        {
            // returns starting values for cells
            int[] values = new int[cellNumber];

            for (int i = 0; i < cellNumber; i++)
            {
                values[i] = value;
            }

            return values;
        }

        public Vector3[] MoveCellCoordinates(Vector3 movement, Vector3[] cellCoords)
        {
            // returns new cell coordinates to move with the street
            for (int i = 0; i < cellCoords.Length; i++)
            {
                cellCoords[i] += movement;
            }

            return cellCoords;
        }
        #endregion

    }
}