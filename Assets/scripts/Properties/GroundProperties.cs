using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class GroundProperties : MonoBehaviour
{
    #region Variables
    // Public Properties
    public int Budget { get; set; }
    public int Id { get; set; }
    public int NumOfCellsX { get; set; }
    public int NumOfCellsZ { get; set; }

    private List<Cell> groundCells = new List<Cell>();
    #endregion

    public void InitializeGroundProperties (int budget, int id, int xNum, int zNum)
    {
        Budget = budget;
        Id = id;
        NumOfCellsX = xNum;
        NumOfCellsZ = zNum;

        // instantiating cells in Cell Array with starting points
        int numOfCells = xNum * zNum;

        for (int i = 0; i < numOfCells; ++i)
        {
            groundCells.Add(new Cell(this.transform.position));
        }
    }


    public List<Cell> GetGroundCells()
    {
        return GetUpdatedCellCoordinates(groundCells, NumOfCellsX, NumOfCellsZ);
    }

    private List<Cell> GetUpdatedCellCoordinates(List<Cell> cells, int xCellNumber, int zCellNumber)
    {
        float groundWidth = Tools.GetSize(this.gameObject, 'x');
        float groundLength = Tools.GetSize(this.gameObject, 'z');
        float sidewalkHeight = Tools.GetSize(this.transform.GetChild(1).gameObject, 'y'); //REVIEW: check if hierarchy is correct
        float cellSize = groundWidth / xCellNumber;

        // checking if we get the same size if computing along the z axis
        if (cellSize != groundLength / zCellNumber)
        {
            Debug.LogWarning("WARNING: Ground size doesn't allow for square cells. cells MUST be square!");
        }

        // compute cell coordinates
        int zIndex = 0;
        int xIndex = 0;
        Vector3 cellCoordinatesDelta = new Vector3(0f,0f,0f);
        int plusMinus;

        for (int i = 0; i < cells.Count; i++)
        {
            zIndex = i / xCellNumber;
            xIndex = i - (i / xCellNumber) * xCellNumber;

            // plusMinus switches the sign after the left side half of the cell coordinates are computed, to start the right side ones. It's stupid.
            plusMinus = i < cells.Count / 2 ? -1 : +1;

            // compute the delta of the cells with resptect to the street coordinates
            cellCoordinatesDelta.x = plusMinus * (groundWidth / 2f + (xIndex + 0.5f) * cellSize);
            cellCoordinatesDelta.y = sidewalkHeight / 2f;
            cellCoordinatesDelta.z = (zIndex + 0.5f) * cellSize;

            // the final cell coordinates
            cells[i].Coordinates += cellCoordinatesDelta;
        }

        return cells;
    }
}