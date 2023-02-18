using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Common;

// Class describing the ground segments Stack. Structured as a FIFO.
// Also implements all the Stack functionalities like spawning new ground, trigger Item/Obstacle placement etc.
public class SegmentManager
{
    private string streetPiecePath;
    private int xNumofCells;
    private int zNumofCells;

    private ItemSpawner obstacleSpawner;
    public int Budget { get; set; }
    private int maxObstacles;

    public List<SegmentProperties> segmentStack;
    public int stackSize { get; private set; }
    private int StackCount { get { return segmentStack.Count; } }

    // Constructor
    public SegmentManager(string strtPcPath, int xCells, int zCells, string obsPath, int budget, int maxObs, int stkSize)
    {
        streetPiecePath = strtPcPath;
        xNumofCells= xCells;
        zNumofCells= zCells;

        obstacleSpawner = new ItemSpawner(obsPath);
        Budget = budget;
        maxObstacles = maxObs;

        segmentStack = new List<SegmentProperties>();
        stackSize = stkSize;
    }

    // Spawns a new ground segment ahead of the player
    public void SpawnSegment(int totalBudget = 0)
    {
        if (totalBudget == 0) totalBudget = Budget;


        // Spawning ground
        Vector3 spawnCoordinates = GetNextStreetPieceCoordinates();
        StreetPieceProperties newStreetPiece = GameObject.Instantiate(Resources.Load<GameObject>(streetPiecePath), spawnCoordinates, Quaternion.identity).GetComponent<StreetPieceProperties>();
        newStreetPiece.GetComponent<StreetPieceProperties>().SPConstructor(Tools.GetNextValue());


        // instantiating cells in Cell Array with starting points
        List<CellProperties> newCells = new List<CellProperties>();
        int numOfCells = 2 * (xNumofCells * zNumofCells);
        for (int i = 0; i < numOfCells; ++i)
        {
            // to easily recognize the cells as belonging to a segment, the ID will be built using the ID of the Street Piece + Cell number
            int newCellID = Int32.Parse($"{newStreetPiece.Id}{i}"); //HACK: check this actually works! otherwise use string ids
            
            newCells.Add(new CellProperties(newCellID));
        }


        // Spawning obstacles
        obstacleSpawner.FillItemList(maxObstacles, totalBudget);
        List<ObstacleProperties> newObstacles = obstacleSpawner.PlaceObstacles(newCells);

        // creating the new Segment
        SegmentProperties newSegment = new SegmentProperties(Budget, newStreetPiece, newObstacles, newCells, xNumofCells, zNumofCells);
        InsertSegmentIntoStack(newSegment);

        return;
    }

    // Computes coordinates for spawning a new ground segment
    private Vector3 GetNextStreetPieceCoordinates()
    {
        Vector3 nextCoords = Vector3.zero;
        float streetLength = StackCount == 0 ? 0 : segmentStack[StackCount - 1].StreetPiece.Length;

        nextCoords.z = StackCount == 0 ? 0 : segmentStack[StackCount - 1].StreetPiece.gameObject.transform.position.z + streetLength;

        return nextCoords;
    }

    // Inserts new segment into the Stack
    private void InsertSegmentIntoStack(SegmentProperties seg)
    {
        // Before inserting a new street segment into the stack, we remove the oldest one (index = 0), if it would exceed the stack's max size
        if (segmentStack.Count == stackSize)
        {
            foreach (var obs in segmentStack[0].Obstacles)
            {
                GameObject.Destroy(obs.gameObject);
            }

            GameObject.Destroy(segmentStack[0].StreetPiece.gameObject);

            segmentStack.RemoveAt(0);
        }

        segmentStack.Add(seg);
    }

    // Computes an updated position for the cells // TODO: adatta ad essere usat qua
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