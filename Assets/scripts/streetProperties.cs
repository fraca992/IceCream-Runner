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
    [SerializeField]
    private int startCellValue = 1;

    private int totCellNum;
    private Vector3 oldPosition = new Vector3();

    CellsProperties Cells;
    #endregion

    public class CellsProperties // metti in tools?
    {
        public Vector3[] CellCoordinates { get; set; }
        public int[] CellValues { get; set; }
    }

    private void Awake()
    {
        Cells = new CellsProperties();
        totCellNum = 2 * zCellNumber * xCellNumber;
        Cells.CellCoordinates = GetCellCoordinates(xCellNumber, totCellNum);
        Cells.CellValues = SetCellValues(startCellValue, totCellNum);
    }

    void Update()
    {
        Vector3 newPosition = transform.position;
        Vector3 movement = newPosition - oldPosition;
        
        oldPosition = newPosition;
        Cells.CellCoordinates = MoveCellCoordinates(movement, Cells.CellCoordinates);
    }

    // Helper Methods
    private Vector3[] GetCellCoordinates(int xCellNum, int totCellNum)
    {
        // Returns an array with the coordinates of each cell in the street segment
        // IMPORTANT: a sidewalk MUST have leght as a multiple of width, or the cells won't be square
        Vector3[] cellCoords = new Vector3[totCellNum];
        Vector3[] leftSidewalkCoords = new Vector3[totCellNum/2];
        Vector3[] rightSidewalkCoords = new Vector3[totCellNum/2];

        float streetWidth = Tools.GetSize(transform.GetChild(0).gameObject, 'x');
        float SidewalkHeight = Tools.GetSize(transform.GetChild(0).GetChild(0).gameObject, 'y');
        float cellWidth = Tools.GetSize(transform.GetChild(0).GetChild(0).gameObject, 'x') / xCellNum;

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

    private int[] SetCellValues(int value, int totCellNum)
    {
        // returns starting values for cells
        int[] values = new int[totCellNum];

        for (int i = 0; i < totCellNum; i++)
        {
            values[i] = value;
        }

        return values;
    }

    private Vector3[] MoveCellCoordinates(Vector3 movement, Vector3[] cellCoords)
    {
        // returns new cell coordinates to move with the street
        for (int i = 0; i < cellCoords.Length; i++)
        {
            cellCoords[i] += movement;
        }

        return cellCoords;
    }

    //private void OnDrawGizmos() //DEBUG
    //{
    //    if (!Application.isPlaying) return;
    //    Gizmos.color = Color.red;
    //    for (int i = 0; i < Cells.CellCoordinates.Length; i++)
    //    {
    //        Gizmos.DrawCube(Cells.CellCoordinates[i] + 0.5f*Vector3.up, new Vector3 (1,1,1)); 
    //    }

    //    Gizmos.color = Color.green;
    //    for (int i = 0; i < Cells.CellCoordinates.Length; i++)
    //    {
    //        UnityEditor.Handles.Label(Cells.CellCoordinates[i] + 1.5f * Vector3.up, Cells.CellValues[i].ToString());
    //    }
    //}
}
