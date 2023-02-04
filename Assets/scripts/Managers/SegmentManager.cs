using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Common;

// Class describing the ground segments Stack. Structured as a FIFO.
// Also implements all the Stack functionalities like spawning new ground, trigger Item/Obstacle placement etc.
public class SegmentManager : MonoBehaviour
{
    public int StackSize { get; private set; }
    public int maxObstacles { get; private set; }
    public string StreetPiecePath { private get; set; }
    public int StackCount { get { return segmentStack.Count; } }

    private ItemSpawner obstacleSpawner;
    public List<Segment> segmentStack = new List<Segment>();

    //// Constructor
    //public SegmentManager(string itmPath, string obsPath, int size, int maxObs)
    //{
    //    StackSize = size;
    //    maxObstacles = maxObs;
    //    StreetPiecePath = itmPath;
    //    obstacleSpawner = new ItemSpawner(obsPath);
    //}

    // Class describing a single segment
    public class Segment //TODO: START HERE, refactoring. forza e coraggio :)
    {
        public GameObject StreetPiece { get; private set; }
        public List<GameObject> Obstacles { get; private set; }

        public int xCellNumber { get; private set; }
        public int zCellNumber { get; private set; }
        private float cellSize;
        public List<CellProperties> Cells { get { return GetUpdatedCellCoordinates(Cells, 1, 3f); } private set { Cells = value; } }
        

        // Constructor
        public Segment(GameObject strP, int xCellNum, int zCellNum, float clSize, List<GameObject> obs)
        {
            streetPiece = strP;
            xCellNumber = xCellNum;
            zCellNumber = zCellNum;
            cellSize = clSize;
            Cells = new List<CellProperties> test();

            Cells = GetUpdatedCellCoordinates(clls, xCellNum, cellSize);
            obstacles = obs;
        }

        // Computes an updated position for the cells
        private List<CellProperties> GetUpdatedCellCoordinates(List<CellProperties> cells, int xCellNumber, float cellSize)
        {
            // compute cell coordinates
            int zIndex = 0;
            int xIndex = 0;
            Vector3 cellCoordinatesDelta = new Vector3(0f, 0f, 0f);

            for (int i = 0; i < cells.Count / 2; i++)
            {
                zIndex = i / xCellNumber;
                xIndex = i - zIndex * xCellNumber;

                cellCoordinatesDelta.x = (2 * xIndex + 1) * cellSize / 2f - this.streetPiece.GetComponent<GroundProperties>().SidewalkWidth / 2f;
                cellCoordinatesDelta.y = this.streetPiece.GetComponent<GroundProperties>().SidewalkHeight;
                cellCoordinatesDelta.z = this.streetPiece.GetComponent<GroundProperties>().SidewalkLength / 2f - (2 * zIndex + 1) * cellSize / 2f;

                // the final cell coordinates
                cells[i].Coordinates = this.streetPiece.transform.GetChild(1).position + cellCoordinatesDelta;
            }
            for (int i = cells.Count / 2; i < cells.Count; i++)
            {
                int ii = i - cells.Count / 2;
                zIndex = ii / xCellNumber;
                xIndex = ii - zIndex * xCellNumber;

                cellCoordinatesDelta.x = (2 * xIndex + 1) * cellSize / 2f - this.streetPiece.GetComponent<GroundProperties>().SidewalkWidth / 2f;
                cellCoordinatesDelta.y = this.streetPiece.GetComponent<GroundProperties>().SidewalkHeight;
                cellCoordinatesDelta.z = this.streetPiece.GetComponent<GroundProperties>().SidewalkLength / 2f - (2 * zIndex + 1) * cellSize / 2f;

                // the final cell coordinates
                cells[i].Coordinates = this.streetPiece.transform.GetChild(2).position + cellCoordinatesDelta;
            }

            return cells;
        }
    }


    //    // Spawns a new ground segment ahead of the player
    //    public void SpawnSegment(int totalBudget)
    //    {
    //        Vector3 spawnCoordinates = GetNextGroundCoordinates();

    //        // Spawning ground
    //        GameObject newGround = Instantiate(Resources.Load<GameObject>(GroundPath), spawnCoordinates, Quaternion.identity);
    //        newGround.GetComponent<GroundProperties>().InitializeGroundProperties(totalBudget, Tools.GetNextValue());

    //        // TODO: Create cells
    //        float cellSize = SidewalkWidth / NumOfCellsX;
    //        int numOfCells = 2 * (xNum * zNum);

    //        // checking if we get the same size if computing along the z axis //TODO: MOVE TO SEGMENT
    //        if (cellSize != SidewalkLength / NumOfCellsZ)
    //        {
    //            Debug.LogWarning("WARNING: Ground size doesn't allow for square cells. cells MUST be square!" + $" width: {sidewalkWidth} x length: {sidewalkLength}, cellsize: {cellSize} x {sidewalkLength / NumOfCellsZ}");
    //        }
    //        // instantiating cells in Cell Array with starting points
    //        for (int i = 0; i < numOfCells; ++i)
    //        {
    //            groundCells.Add(new CellProperties(this.transform.position, cellSize));
    //        }


    //        // Spawning obstacles
    //        obstacleSpawner.FillItemList(maxObstacles, totalBudget);
    //        List<GameObject> newObstacles = obstacleSpawner.PlaceItems(newGround);

    //        // creating the new Segment
    //        Segment newSegment = new Segment(newGround, , newObstacles); // TODO> Add cells here

    //        InsertSegmentIntoStack(newSegment);
    //        return;
    //    }

    //    // Computes coordinates for spawning a new ground segment
    //    private Vector3 GetNextGroundCoordinates()
    //    {
    //        Vector3 nextCoords = Vector3.zero;
    //        float streetLength = StackCount == 0 ? 0 : Tools.GetSize(segmentStack[StackCount - 1].ground, 'z', 'r');

    //        nextCoords.z = StackCount == 0 ? 0 : segmentStack[StackCount - 1].ground.transform.position.z + streetLength;

    //        return nextCoords;
    //    }

    //    // Inserts new segment into the Stack
    //    private void InsertSegmentIntoStack(Segment seg)
    //    {
    //        // Before inserting a new street segment into the stack, we remove the oldest one (index = 0), if it would exceed the stack's max size
    //        if (segmentStack.Count == StackSize)
    //        {
    //            foreach (var obs in segmentStack[0].obstacles)
    //            {
    //                GameObject.Destroy(obs);
    //            }

    //            GameObject.Destroy(segmentStack[0].ground);

    //            segmentStack.RemoveAt(0);
    //        }

    //        segmentStack.Add(seg);
    //    }
}



    // Class describing an Item spawner. This object manages the lists of Items (obstacles, power-ups, etc.), selects new items once a new ground segment spawns
    // and places them on the segment.
    // TODO: for now the item placement is completely random. implement an algorithm to avoid path blocking and unbalanced positioning!
    public class ItemSpawner
    {
        private string ItemPath;
        private List<GameObject> allItems = new List<GameObject>();
        private List<GameObject> selectedItems = new List<GameObject>();
        public List<GameObject> spawnedItems = new List<GameObject>();
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
        public List<GameObject> PlaceItems(List<CellProperties> cells, List<GameObject> items = null) //TODO: maybe pass Segment instead of cells?
        {
            spawnedItems.Clear();

            if (items == null) items = selectedItems;

            int rndCellIndex;
            int rndItemIndex;

            // we place items until we have 0 items and/or no space
            while (items.Count > 0 && cells.Count > 0)
            {
                rndCellIndex = UnityEngine.Random.Range(0, cells.Count);
                rndItemIndex = UnityEngine.Random.Range(0, items.Count);

                GameObject itm = selectedItems[rndItemIndex];
                CellProperties cl = cells[rndCellIndex];

                // TODO: Must eventually account for different size/shape of items
                if (cl.isOccupied == false)
                {
                    spawnedItems.Add(Instantiate(itm, cl.Coordinates, Quaternion.identity));
                    cells.RemoveAt(rndCellIndex);
                    items.RemoveAt(rndItemIndex);
                }
            }
            return spawnedItems;
        }
    }