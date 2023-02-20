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
    private float cellSize;

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
        int newId = Tools.GetNextValue();

        // Fetch the Segment GameObject to hold the StreetPiece and Items, if not Instantates it
        GameObject newSegmentGO = GameObject.Find($"Segment_{newId}");
        if (newSegmentGO == null)
        {
            newSegmentGO = new GameObject($"Segment_{newId}");
            newSegmentGO.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            newSegmentGO.transform.SetParent(GameObject.Find("Street").transform);
        }

        // Instantiating ground
        Vector3 spawnCoordinates = GetNextStreetPieceCoordinates();
        StreetPieceProperties newStreetPiece = GameObject.Instantiate(Resources.Load<GameObject>(streetPiecePath), spawnCoordinates, Quaternion.identity, newSegmentGO.transform).GetComponent<StreetPieceProperties>();
        newStreetPiece.GetComponent<StreetPieceProperties>().SPConstructor(newId);


        // creating cells in Cell Array with starting points
        List<CellProperties> newCells = new List<CellProperties>();
        int numOfCells = 2 * (xNumofCells * zNumofCells);
        for (int i = 0; i < numOfCells; ++i)
        {
            // to easily recognize the cells as belonging to a segment, the ID will be built using the ID of the Street Piece + Cell number
            int newCellID = Int32.Parse($"{newId}{i}"); //HACK: check this actually works! otherwise use string ids
            
            newCells.Add(new CellProperties(newCellID));
        }
        cellSize = GetCellSize(newStreetPiece,xNumofCells,zNumofCells);
        newCells = Tools.GetUpdatedCellCoordinates(newStreetPiece.gameObject, newCells, xNumofCells, cellSize);

        // Instantiating obstacles
        obstacleSpawner.FillItemList(maxObstacles, totalBudget);
        List<ObstacleProperties> newObstacles = obstacleSpawner.PlaceObstacles(newCells, newSegmentGO.transform);

        // Adding a new SegmentProperties script to the Segment_x GameObject, initializing it then adding it to the SegmentStack
        SegmentProperties newSegment;
        newSegmentGO.AddComponent<SegmentProperties>();
        newSegment = newSegmentGO.GetComponent<SegmentProperties>().InitializeStreetPiece(Budget, newStreetPiece, newObstacles, newCells, xNumofCells, zNumofCells);
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

    // Computes cell Size and checks whether they are square
    private float GetCellSize(StreetPieceProperties sp, int xCellNumber, int zCellNumber)
    {
        float cellSize = 0;

        // checking if we get the same size if computing along the z axis
        if ((sp.Length / zCellNumber) == (sp.SidewalkWidth / xCellNumber))
        {
            cellSize = sp.Length / zCellNumber;
            return cellSize;
        }
        else
        {
            Debug.LogWarning("WARNING: Ground size doesn't allow for square cells. cells MUST be square!" +
                $" width: {sp.SidewalkWidth} x length: {sp.Length}, cellsize: {(sp.Length / zCellNumber)} x {(sp.Width / xCellNumber)}");
            return 0;
        }
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
            GameObject.Destroy(segmentStack[0].gameObject);

            segmentStack.RemoveAt(0);
        }

        segmentStack.Add(seg);
    }
}