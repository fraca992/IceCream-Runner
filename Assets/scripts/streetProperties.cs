using System;
using UnityEngine;
using Common;

public class StreetProperties : MonoBehaviour
{
    [SerializeField]
    private int zCellNumber = 10;
    [SerializeField]
    private int xCellNumber = 2;

    public Vector3[] leftSidewalkCellCoords { get; set; }
    public Vector3[] rightSidewalkCellCoords { get; set; }

    private Vector3 oldPosition = new Vector3();

    private void Awake()
    {
        // Creates cell coordinates grid, separated by left and right sidewalk
        int cellNum = zCellNumber * xCellNumber;
        Vector3[] cellCoordinates = new Vector3[2 * cellNum];
        leftSidewalkCellCoords = new Vector3[cellNum];
        rightSidewalkCellCoords = new Vector3[cellNum];
        
        cellCoordinates = GetCellCoordinates(zCellNumber, xCellNumber);
        Array.Copy(cellCoordinates, 0, leftSidewalkCellCoords, 0, leftSidewalkCellCoords.Length);
        Array.Copy(cellCoordinates, leftSidewalkCellCoords.Length, rightSidewalkCellCoords, 0, rightSidewalkCellCoords.Length);
    }

    void Update()
    {
        // Moves cell grid with the street
        Vector3 newPosition = transform.position;
        Vector3 movement = newPosition - oldPosition;

        oldPosition = newPosition;
        MoveCellCoordinates(leftSidewalkCellCoords, movement);
        MoveCellCoordinates(rightSidewalkCellCoords, movement);
    }

    // Helper Methods
    Vector3[] GetCellCoordinates(int zCellNum, int xCellNum)
    {
        // Returns an array with the coordinates of each cell in the street segment
        // IMPORTANT: a sidewalk MUST have leght as a multiple of width, or the cells won't be square
        int cellNum = zCellNum * xCellNum;
        Vector3[] cellCoords = new Vector3[2 * cellNum];
        Vector3[] leftSidewalkCoords = new Vector3[cellNum];
        Vector3[] rightSidewalkCoords = new Vector3[cellNum];

        float streetWidth = Tools.GetSize(transform.GetChild(0).gameObject, 'x');
        float SidewalkHeight = Tools.GetSize(transform.GetChild(0).GetChild(0).gameObject, 'y');
        float cellWidth = Tools.GetSize(transform.GetChild(0).GetChild(0).gameObject, 'x')/xCellNum;

        int zIndex = 0;
        int xIndex = 0;

        for (int i = 0; i < cellNum; i++)
        {
            zIndex = i / xCellNum;
            xIndex = i - (zIndex * xCellNum);

            //leftSidewalkCoords[i] = transform.position;
            leftSidewalkCoords[i].x += -(streetWidth / 2f + (xIndex + 0.5f) * cellWidth);
            leftSidewalkCoords[i].y += SidewalkHeight / 2f;
            leftSidewalkCoords[i].z += (zIndex + 0.5f) * cellWidth;

            //rightSidewalkCoords[i] = transform.position;
            rightSidewalkCoords[i].x += streetWidth / 2f + (xIndex + 0.5f) * cellWidth;
            rightSidewalkCoords[i].y += SidewalkHeight / 2f;
            rightSidewalkCoords[i].z += (zIndex + 0.5f) * cellWidth;
        }

        leftSidewalkCoords.CopyTo(cellCoords, 0);
        rightSidewalkCoords.CopyTo(cellCoords, leftSidewalkCoords.Length);

        return cellCoords;
    }      

    Vector3[] MoveCellCoordinates(Vector3[] cellCoords, Vector3 movement)
    {
        // Moves cell grid to the new position
        for (int i = 0; i < cellCoords.Length; i++)
        {
            cellCoords[i] += movement;
        }

        return cellCoords;
    }
}
