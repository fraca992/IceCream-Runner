using System.Collections.Generic;
using UnityEngine;
using Common;

public class GroundProperties : MonoBehaviour
{
    #region Variables
    public int Budget { get; set; }
    public int Id { get; set; }
    public int NumOfCellsX { get; set; } // Number of cells along the X axis of the Sidewalk
    public int NumOfCellsZ { get; set; } // Number of cells along the Z axis of the Sidewalk
    private float sidewalkWidth;
    private float sidewalkLength;
    private float sidewalkHeight;

    private List<CellProperties> groundCells = new List<CellProperties>();
    #endregion

    // Constructor
    public void InitializeGroundProperties (int budget, int id, int xNum, int zNum)
    {
        Budget = budget;
        Id = id;
        NumOfCellsX = xNum;
        NumOfCellsZ = zNum;
        sidewalkWidth = Tools.GetSize(this.transform.GetChild(1).gameObject, 'x');
        sidewalkLength = Tools.GetSize(this.transform.GetChild(1).gameObject, 'z');
        sidewalkHeight = Tools.GetSize(this.transform.GetChild(1).gameObject, 'y');
        float cellSize = sidewalkWidth / NumOfCellsX;
        int numOfCells = 2 * (xNum * zNum);

        // checking if we get the same size if computing along the z axis
        if (cellSize != sidewalkLength / NumOfCellsZ)
        {
            Debug.LogWarning("WARNING: Ground size doesn't allow for square cells. cells MUST be square!" + $" width: {sidewalkWidth} x length: {sidewalkLength}, cellsize: {cellSize} x {sidewalkLength / NumOfCellsZ}");
        }

        // instantiating cells in Cell Array with starting points
        for (int i = 0; i < numOfCells; ++i)
        {
            groundCells.Add(new CellProperties(this.transform.position, cellSize));
        }
    }

    // used to access the position of the cells
    public List<CellProperties> GetGroundCells()
    {
        return GetUpdatedCellCoordinates(groundCells, NumOfCellsX, NumOfCellsZ, groundCells[0].Size);
    }

    // Computes an updated position for the cells
    private List<CellProperties> GetUpdatedCellCoordinates(List<CellProperties> cells, int xCellNumber, int zCellNumber, float cellSize)
    {
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