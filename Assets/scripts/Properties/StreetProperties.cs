using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class StreetProperties : MonoBehaviour
{
    #region Variables
    // Public Properties
    public int Budget { get; set; }
    public int Index { get; set; }
    public int Id { get; set; }
    public int NumOfCellsX { get; set; }
    public int NumOfCellsZ { get; set; }
    public int CellStartingPoints { get; set; }

    public Cell[] StreetCells { get { return GetUpdatedCellCoordinates(StreetCells); } private set { StreetCells = value; } }

    // Other variables
    // int numOfCells;
    #endregion

    public StreetProperties(int budget, int index, int id, int xNum = 2, int zNum = 10, int cellPoints = 10)
    {
        Budget = budget;
        Index = index;
        Id = id;
        NumOfCellsX = xNum;
        NumOfCellsZ = zNum;
        CellStartingPoints = cellPoints;
    }

    // Awake is called when the object is created
    void Awake()
    {
        int numOfCells = NumOfCellsX * NumOfCellsZ;

        StreetCells = new Cell[numOfCells];
        for (int i = 0; i < numOfCells; ++i)
        {
            StreetCells[i] = new Cell(CellStartingPoints);
        }
    }

    private Cell[] GetUpdatedCellCoordinates(Cell[] cells, int xCellNumber, int zCellNumber) //TODO: finisci qua, bestia
    {
        float streetWidth = Tools.GetSize(this.gameObject, 'x');
        float streetLength = Tools.GetSize(this.gameObject, 'z');

        float cellSize = streetWidth / xCellNumber;
        if (cellSize != streetLength / zCellNumber) // checking if we get the same size if computing along the z axis
        {
            Debug.LogWarning("WARNING: cells are not square");
        }

        // calcolo coordinate

        return cells;
    }
}