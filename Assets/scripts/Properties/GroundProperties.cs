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
        int numOfCells = 2 * (xNum * zNum);

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
        float sidewalkWidth = Tools.GetSize(this.transform.GetChild(1).gameObject, 'x');
        float sidewalkLength = Tools.GetSize(this.transform.GetChild(1).gameObject, 'z');
        float sidewalkHeight = Tools.GetSize(this.transform.GetChild(1).gameObject, 'y');
        float cellSize = sidewalkWidth / xCellNumber;

        // checking if we get the same size if computing along the z axis
        if (cellSize != sidewalkLength / zCellNumber)
        {
            Debug.LogWarning("WARNING: Ground size doesn't allow for square cells. cells MUST be square!" + $" width: {sidewalkWidth} x length: {sidewalkLength}, cellsize: {cellSize} x {sidewalkLength / zCellNumber}");
        }

        // compute cell coordinates
        int zIndex = 0;
        int xIndex = 0;
        Vector3 cellCoordinatesDelta = new Vector3(0f,0f,0f);

        for (int i = 0; i < cells.Count/2; i++)
        {
            zIndex = i / xCellNumber;
            xIndex = i - zIndex * xCellNumber;

            cellCoordinatesDelta.x = (2 * xIndex + 1) * cellSize / 2f - sidewalkWidth / 2f;
            cellCoordinatesDelta.y = sidewalkHeight;
            cellCoordinatesDelta.z = sidewalkLength / 2f - (2 * zIndex + 1) * cellSize / 2f;

            // the final cell coordinates
            cells[i].Coordinates = this.transform.GetChild(1).position + cellCoordinatesDelta;
        }
        for (int i = cells.Count / 2; i < cells.Count; i++)
        {
            int ii = i - cells.Count / 2;
            zIndex = ii / xCellNumber;
            xIndex = ii - zIndex * xCellNumber;

            cellCoordinatesDelta.x = (2 * xIndex + 1) * cellSize / 2f - sidewalkWidth / 2f;
            cellCoordinatesDelta.y = sidewalkHeight;
            cellCoordinatesDelta.z = sidewalkLength / 2f - (2 * zIndex + 1) * cellSize / 2f;

            // the final cell coordinates
            cells[i].Coordinates = this.transform.GetChild(2).position + cellCoordinatesDelta;
        }

        return cells;
    }
}