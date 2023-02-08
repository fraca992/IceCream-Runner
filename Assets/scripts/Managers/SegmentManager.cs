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

    // Spawns a new ground segment ahead of the player //TODO: START REFACTORING HERE
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
            int newCellID = Int32.Parse($"{newStreetPiece.Id}{i}"); //REVIEW: check this actually works!
            
            newCells.Add(new CellProperties());
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
        float streetLength = StackCount == 0 ? 0 : Tools.GetSize(segmentStack[StackCount - 1].StreetPiece.gameObject, 'z', 'r');

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
                GameObject.Destroy(obs);
            }

            GameObject.Destroy(segmentStack[0].StreetPiece);

            segmentStack.RemoveAt(0);
        }

        segmentStack.Add(seg);
    }
}



    // Class describing an Item spawner. This object manages the lists of Items (obstacles, power-ups, etc.), selects new items once a new ground segment spawns
    // and places them on the segment.
    // TODO: for now the item placement is completely random. implement an algorithm to avoid path blocking and unbalanced positioning!
    // also move to its own script
    public class ItemSpawner : MonoBehaviour
    {
        private string ItemPath;
        private List<GameObject> allItems = new List<GameObject>();
        private List<GameObject> selectedItems = new List<GameObject>();
        public List<ObstacleProperties> spawnedObstacles = new List<ObstacleProperties>();
        private int minCost;

        // Constructor
        public ItemSpawner(string path)
        {
            ItemPath = path;
            allItems.AddRange(Resources.LoadAll<GameObject>(ItemPath));

            //minCost of allItems is found to ensure we don't get stuck while selecting items if remaining budget is too low
            minCost = allItems[0].GetComponent<ObstacleProperties>().Cost;
            for (int i = 1; i < allItems.Count; i++)
            {
                int tempCost = allItems[i].GetComponent<ObstacleProperties>().Cost;
                if (tempCost < minCost)
                {
                    minCost = tempCost;
                }
            }
        }

        // this function fills selectedItems with the Items that will be placed on the segment.
        public void FillItemList(int maxItems, int totBudget)
        {
            int budget = totBudget;

            selectedItems.Clear();

            // we add items to the list as long as we don't reach maxItems or the budget is too low to afford even the cheaper item
            while (selectedItems.Count < maxItems && budget >= minCost)
            {
                int rndIndex = UnityEngine.Random.Range(0, allItems.Count);
                bool canAfford = budget > allItems[rndIndex].GetComponent<ObstacleProperties>().Cost;

                if (canAfford)
                {
                    selectedItems.Add(allItems[rndIndex]);
                    budget -= allItems[rndIndex].GetComponent<ObstacleProperties>().Cost;
                }
            }
            return;
        }

        // this function places all the selected Items on the Ground segment
        public List<ObstacleProperties> PlaceObstacles(List<CellProperties> cells)
        {
            spawnedObstacles.Clear();


            int rndCellIndex;
            int rndItemIndex;

            // we place items until we have 0 items and/or no space
            while (selectedItems.Count > 0 && cells.Count > 0)
            {
                rndCellIndex = UnityEngine.Random.Range(0, cells.Count);
                rndItemIndex = UnityEngine.Random.Range(0, selectedItems.Count);

                GameObject itm = selectedItems[rndItemIndex];
                CellProperties cl = cells[rndCellIndex];

                // TODO: Must eventually account for different size/shape of items
                if (cl.isOccupied == false)
                {
                    spawnedObstacles.Add(Instantiate(itm, cl.Coordinates, Quaternion.identity).GetComponent<ObstacleProperties>());
                    cells.RemoveAt(rndCellIndex);
                    selectedItems.RemoveAt(rndItemIndex);
                }
            }
            return spawnedObstacles;
        }
    }