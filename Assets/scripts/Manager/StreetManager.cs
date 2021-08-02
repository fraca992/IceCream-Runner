using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Manager
{
    public class StreetManager : Object
    {
        // Manager for steet Spawning, Destroying and Moving
        private List<Street> streets = new List<Street>();
        private GameObject streetPrefab;
        private int maxStreets;
        private float streetLength;
        private int newestStreetIndex = 0;
        
        // Constructor
        public StreetManager(int maxStreets, GameObject streetPrefab)
        {
            this.maxStreets = maxStreets;
            this.streetPrefab = streetPrefab;
            streetLength = Tools.GetSize(streetPrefab, 'z');
        }

        // Methods
        #region Street Methods
        public int? SpawnStreetIfNeeded(int streetBudget, int xCellNum, int cellNumber, int startCellValue)
        {
            // Instantiate a new street segment
            Vector3 nextStreetPosition = new Vector3();

            if (streets.Count < maxStreets)
            {
                nextStreetPosition.z = streets[newestStreetIndex].StreetObject.transform.position.z + streetLength;
                List<Street.CellProperties> newCells = InitializeCells(newestStreetIndex, xCellNum, cellNumber, startCellValue);
                Street newStreet = new Street(Instantiate(streetPrefab, nextStreetPosition, Quaternion.identity), streetBudget, newCells);

                streets.Add(newStreet);
                newestStreetIndex = streets.Count - 1;

                return newestStreetIndex;
            }

            return null;
        }

        public void DestroyStreetIfOld(int bufferStreetNumber = 2)
        {
            // Destroy old street segments behind the player
            float bufferLength = -bufferStreetNumber * streetLength;
            for (int i = 0; i < streets.Count; i++)
            {
                if (streets[i].StreetObject.transform.position.z < bufferLength)
                {
                    Destroy(streets[i].StreetObject);
                    streets.RemoveAt(i);
                }
            }
        }

        public void MoveStreets(float streetMovement)
        {
            // Move street segments
            foreach (Street street in streets)
            {
                street?.StreetObject.transform.Translate(0, 0, -streetMovement);
                MoveCellCoordinates(-streetMovement, street.Cells);
            }
        }
        #endregion

        #region Cell Methods
        public List<Street.CellProperties> InitializeCells(int streetIndex, int xCellNum, int cellNumber, int startValue)
        {
            // creates a new cell list
            List<Street.CellProperties> newCells = new List<Street.CellProperties>();
            Vector3[] cellCoords;
            int[] cellValues;

            cellCoords = GetCellCoordinates(streetIndex, xCellNum, cellNumber);
            cellValues = GetCellValues(startValue, cellNumber);

            for (int i = 0; i < cellNumber; i++)
            {
                newCells.Add(new Street.CellProperties(cellCoords[i], cellValues[i]));
            }

            return newCells;
        }

        public Vector3[] GetCellCoordinates(int streetIndex, int xCellNum, int cellNumber)
        {
            // Returns an array with the coordinates of each cell in the street segment
            // IMPORTANT: a sidewalk MUST have leght as a multiple of width, or the cells won't be square
            Vector3[] cellCoords = new Vector3[cellNumber];
            Vector3[] leftSidewalkCoords = new Vector3[cellNumber / 2];
            Vector3[] rightSidewalkCoords = new Vector3[cellNumber / 2];
            GameObject street = streets[streetIndex].StreetObject;

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

        public void MoveCellCoordinates(float movement, List<Street.CellProperties> cells)
        {
            // returns new cell coordinates to move with the street
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].Coordinates += new Vector3(0, 0, movement);
            }
        }
        #endregion

        private void OnDrawGizmos() //DEBUG gizmos
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;
            for (int i = 0; i < streets.Count; i++)
            {
                for (int k = 0; k < streets[i].Cells.Count; k++)
                {
                    Gizmos.DrawCube(streets[i].Cells[k].Coordinates + 0.5f * Vector3.up, new Vector3(1, 1, 1));

                    UnityEditor.Handles.Label(streets[i].Cells[k].Coordinates + 1.5f * Vector3.up, streets[i].Cells[k].Value.ToString());
                }
            }
        }
    }
}