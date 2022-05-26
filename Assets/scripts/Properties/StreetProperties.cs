using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class StreetProperties : MonoBehaviour
{
    #region Variables
    // Public Properties
    public int Budget { get; set; }
    public int Id { get; set; }
    public int NumOfCellsX { get; set; }
    public int NumOfCellsZ { get; set; }
    public int CellStartingPoints { get; set; }

    private Cell[] StreetCells { get; set; }
    #endregion

    public void InitializeStreetProperties (int budget, int id, int xNum, int zNum, int cellPoints)
    {
        Budget = budget;
        Id = id;
        NumOfCellsX = xNum;
        NumOfCellsZ = zNum;
        CellStartingPoints = cellPoints;

        // instantiating cells in Cell Array with starting points
        int numOfCells = xNum * zNum;
        StreetCells = new Cell[numOfCells];

        for (int i = 0; i < numOfCells; ++i)
        {
            StreetCells[i] = new Cell(CellStartingPoints, this.transform.position);
        }
    }


    public Cell[] GetStreetCells()
    {
        return GetUpdatedCellCoordinates(StreetCells, NumOfCellsX, NumOfCellsZ);
    }

    private Cell[] GetUpdatedCellCoordinates(Cell[] cells, int xCellNumber, int zCellNumber)
    {
        float streetWidth = Tools.GetSize(this.gameObject, 'x');
        float streetLength = Tools.GetSize(this.gameObject, 'z');
        float sidewalkHeight = Tools.GetSize(this.transform.GetChild(1).gameObject, 'y'); //REVIEW: check if hierarchy is correct
        float cellSize = streetWidth / xCellNumber;

        // checking if we get the same size if computing along the z axis
        if (cellSize != streetLength / zCellNumber)
        {
            Debug.LogWarning("WARNING: Street size doesn't allow for square cells. cells MUST be square!");
        }

        // compute cell coordinates
        int zIndex = 0;
        int xIndex = 0;
        Vector3 cellCoordinatesDelta = new Vector3(0f,0f,0f);
        int plusMinus;

        for (int i = 0; i < cells.Length; i++)
        {
            zIndex = i / xCellNumber;
            xIndex = i - (i / xCellNumber) * xCellNumber;

            // plusMinus switches the sign after the left side half of the cell coordinates are computed, to start the right side ones. It's stupid.
            plusMinus = i < cells.Length / 2 ? -1 : +1;

            // compute the delta of the cells with resptect to the street coordinates
            cellCoordinatesDelta.x = plusMinus * (streetWidth / 2f + (xIndex + 0.5f) * cellSize);
            cellCoordinatesDelta.y = sidewalkHeight / 2f;
            cellCoordinatesDelta.z = (zIndex + 0.5f) * cellSize;

            // the final cell coordinates
            cells[i].Coordinates += cellCoordinatesDelta;
        }

        return cells;
    }
}