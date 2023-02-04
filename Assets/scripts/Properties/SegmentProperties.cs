using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SegmentProperties //TODO: START HERE, refactoring. forza e coraggio :)
{
    public GameObject StreetPiece { get; private set; }
    public List<GameObject> Obstacles { get; private set; }

    private int xCellNumber;
    private float cellSize;
    public List<CellProperties> Cells { get { return GetUpdatedCellCoordinates(StreetPiece, Cells, xCellNumber, cellSize); } private set { Cells = value; } }
    

    // Constructor
    public SegmentProperties(GameObject strP, List<GameObject> obs, List<CellProperties> cls, int xCellNum, float cellSz)
    {
        StreetPiece = strP;
        Obstacles = obs;
        Cells = cls;
        xCellNumber = xCellNum;
        cellSize = cellSz;
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

