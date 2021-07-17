using System;
using UnityEngine;
using Common;

public class StreetProperties : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int zCellNumber = 10;
    [SerializeField]
    private int xCellNumber = 2;

    public Vector3[] CellCoordinates { get; set; }

    private Vector3 oldPosition = new Vector3();
    int cellNum;
    #endregion

    private void Awake()
    {
        cellNum = zCellNumber * xCellNumber;
        CellCoordinates = new Vector3[2 * cellNum];
        CellCoordinates = GetCellCoordinates(xCellNumber, cellNum);
    }

    void Update()
    {
        Vector3 newPosition = transform.position;
        Vector3 movement = newPosition - oldPosition;
        
        oldPosition = newPosition;
        CellCoordinates = MoveCellCoordinates(movement, CellCoordinates);
    }

    // Helper Methods
    private Vector3[] GetCellCoordinates(int xCellNum, int cellNum)
    {
        // Returns an array with the coordinates of each cell in the street segment
        // IMPORTANT: a sidewalk MUST have leght as a multiple of width, or the cells won't be square
        Vector3[] cellCoords = new Vector3[CellCoordinates.Length];
        Vector3[] leftSidewalkCoords = new Vector3[cellNum];
        Vector3[] rightSidewalkCoords = new Vector3[cellNum];

        float streetWidth = Tools.GetSize(transform.GetChild(0).gameObject, 'x');
        float SidewalkHeight = Tools.GetSize(transform.GetChild(0).GetChild(0).gameObject, 'y');
        float cellWidth = Tools.GetSize(transform.GetChild(0).GetChild(0).gameObject, 'x') / xCellNum;

        int zIndex = 0;
        int xIndex = 0;

        for (int i = 0; i < cellNum; i++)
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

    private Vector3[] MoveCellCoordinates(Vector3 movement, Vector3[] cellCoords)
    {
        // Moves cell coordinates with the street
        for (int i = 0; i < cellCoords.Length; i++)
        {
            cellCoords[i] += movement;
        }

        return cellCoords;
    }
}
