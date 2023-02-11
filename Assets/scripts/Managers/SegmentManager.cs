using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Common;

// Class describing the ground segments Stack. Structured as a FIFO.
// Also implements all the Stack functionalities like spawning new ground, trigger Item/Obstacle placement etc.
public class SegmentManager : MonoBehaviour
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
    public void SpawnSegment(int totalBudget)
    {
        // Spawning ground
        Vector3 spawnCoordinates = GetNextStreetPieceCoordinates();
        StreetPieceProperties newStreetPiece = Instantiate(Resources.Load<GameObject>(streetPiecePath), spawnCoordinates, Quaternion.identity).GetComponent<StreetPieceProperties>();
        newStreetPiece.GetComponent<StreetPieceProperties>().SPConstructor(Tools.GetNextValue());


        // instantiating cells in Cell Array with starting points
        List<CellProperties> newCells = new List<CellProperties>();
        int numOfCells = 2 * (xNumofCells * zNumofCells);
        for (int i = 0; i < numOfCells; ++i)
        {
            // to easily recognize the cells as belonging to a segment, the ID will be built using the ID of the Street Piece + Cell number
            int newCellID = Int32.Parse($"{newStreetPiece.Id}{i}"); //HACK: check this actually works!
            
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
}