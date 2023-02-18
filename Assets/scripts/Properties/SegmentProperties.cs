using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


public class SegmentProperties
{
    public int Budget { get; private set; }
    public StreetPieceProperties StreetPiece { get; private set; }
    public List<ObstacleProperties> Obstacles { get; private set; }

    private int xCellNumber;
    private int zCellNumber;
    private float cellSize;
    private List<CellProperties> _Cells;
    public ReadOnlyCollection<CellProperties> Cells { get { return GetUpdatedCellCoordinates(StreetPiece.gameObject, _Cells, xCellNumber, cellSize).AsReadOnly(); } }

    // Constructor
    public SegmentProperties(int budget, StreetPieceProperties strP, List<ObstacleProperties> obs, List<CellProperties> cls, int xCellNum, int zCellNum)
    {
        Budget = budget;
        StreetPiece = strP;
        Obstacles = obs;
        _Cells = cls;
        xCellNumber = xCellNum;
        zCellNumber= zCellNum;

        // checking if we get the same size if computing along the z axis
        if ((StreetPiece.Length / zCellNumber) == (StreetPiece.SidewalkWidth / xCellNumber))
        {
            cellSize = StreetPiece.Length / zCellNumber;
        } else
        {
            Debug.LogWarning("WARNING: Ground size doesn't allow for square cells. cells MUST be square!" + 
                $" width: {StreetPiece.SidewalkWidth} x length: {StreetPiece.Length}, cellsize: {(StreetPiece.Length / zCellNumber)} x {(StreetPiece.Width / xCellNumber)}");
        }
    }

    // Computes an updated position for the cells
    private List<CellProperties> GetUpdatedCellCoordinates(GameObject streetPiece, List<CellProperties> cells, int xCellNumber, float cellSize)
    {
        // compute cell coordinates
        int zIndex = 0;
        int xIndex = 0;
        Vector3 cellCoordinatesDelta = new Vector3(0f, 0f, 0f);
        StreetPieceProperties strtPcProperties = streetPiece.GetComponent<StreetPieceProperties>();

        for (int i = 0; i < cells.Count / 2; i++)
        {
            zIndex = i / xCellNumber;
            xIndex = i - zIndex * xCellNumber;

            cellCoordinatesDelta.x = (2 * xIndex + 1) * cellSize / 2f - strtPcProperties.SidewalkWidth / 2f;
            cellCoordinatesDelta.y = strtPcProperties.SidewalkHeight;
            cellCoordinatesDelta.z = strtPcProperties.Length / 2f - (2 * zIndex + 1) * cellSize / 2f;

            // the final cell coordinates
            cells[i].Coordinates = streetPiece.transform.GetChild(1).position + cellCoordinatesDelta;
        }
        for (int i = cells.Count / 2; i < cells.Count; i++)
        {
            int ii = i - cells.Count / 2;
            zIndex = ii / xCellNumber;
            xIndex = ii - zIndex * xCellNumber;

            cellCoordinatesDelta.x = (2 * xIndex + 1) * cellSize / 2f - strtPcProperties.SidewalkWidth / 2f;
            cellCoordinatesDelta.y = strtPcProperties.SidewalkHeight;
            cellCoordinatesDelta.z = strtPcProperties.Length / 2f - (2 * zIndex + 1) * cellSize / 2f;

            // the final cell coordinates
            cells[i].Coordinates = streetPiece.transform.GetChild(2).position + cellCoordinatesDelta;
        }

        return cells;
    }
}
