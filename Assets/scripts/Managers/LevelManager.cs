using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class LevelManager : MonoBehaviour
{
    #region General Variables
    [SerializeField]
    private float levelSpeed = 10f;
    private Vector3 streetMovement = Vector3.zero;
    private GameObject player;
    #endregion

    #region Street Variables
    [SerializeField]
    private int stackSize = 10;
    public GroundStack groundStack;
    private string groundPrefabPath = "Grounds/Street";
    private string obstacleFolderPath = "Obstacles";
    [SerializeField]
    private int groundBudget = 100;
    [SerializeField]
    private int maxObstacles = 8;
    [SerializeField]
    private int streetXCellNum = 2;
    [SerializeField]
    private int streetZCellNum = 10;
    #endregion


    void Awake()
    {
        player = GameObject.Find("Player");

        // Spawn initial ground segments
        groundStack = new GroundStack(groundPrefabPath, obstacleFolderPath, stackSize, maxObstacles);
        for (int i = 0; i < stackSize; i++)
        {
            groundStack.SpawnSegment(groundBudget, streetXCellNum, streetZCellNum);
        }
    }

    private void FixedUpdate()
    {
        // Spawn ground segments continuously
        float lastStreetPositionZ = groundStack.groundList[0].ground.transform.position.z;
        float playerPosition = player.transform.position.z;
        float distanceBuffer = Tools.GetSize(groundStack.groundList[0].ground, 'z') * 2;

        if (lastStreetPositionZ + distanceBuffer < playerPosition)
        {
            groundStack.SpawnSegment(groundBudget, streetXCellNum, streetZCellNum);
        }

        //Moving the ground segments in the stack
        streetMovement.z = -levelSpeed;
        foreach (Segment seg in groundStack.groundList)
        {
            seg.ground.GetComponent<Rigidbody>().MovePosition(seg.ground.transform.position + streetMovement); 
        }
    }

    // Segment struct, each one holds the ground Object as well as Obstacles and Items etc.
    public struct Segment
    {
        public GameObject ground;
        public List<GameObject> obstacles;
    }

    // Class describing the ground segments Stack. Structured as a FIFO.
    // Also implements all the Stack functionalities like spawning new ground, trigger Item/Obstacle placement etc.
    public class GroundStack
    {
        public int StackSize { get; private set; }
        public int maxObstacles { get; private set; }
        public int StackCount { get { return groundList.Count; } }
        public string GroundPath { private get; set; }

        private ItemSpawner obstacleSpawner;

        public List<Segment> groundList = new List<Segment>();

        // Constructor
        public GroundStack(string itmPath, string obsPath, int size, int maxObs)
        {
            StackSize = size;
            maxObstacles = maxObs;
            GroundPath = itmPath;

            obstacleSpawner = new ItemSpawner(obsPath);
        }

        // Spawns a new ground segment ahead of the player
        public void SpawnSegment(int totalBudget, int xCellNum, int zCellNum)
        {
            Vector3 spawnCoordinates = GetNextGroundCoordinates();

            //spawning ground
            GameObject newGround = Instantiate(Resources.Load<GameObject>(GroundPath), spawnCoordinates, Quaternion.identity);
            newGround.GetComponent<GroundProperties>().InitializeGroundProperties(totalBudget, Tools.GetNextValue(), xCellNum, zCellNum);
            

            // Spawning obstacles
            List<GameObject> newObstacles = obstacleSpawner.FillItemList(maxObstacles, totalBudget);
            obstacleSpawner.PlaceItems(newGround);

            // creating the new Segment
            Segment newSegment = new Segment();
            newSegment.ground = newGround;
            newSegment.obstacles = newObstacles;

            InsertSegmentIntoStack(newSegment);
            return;
        }

        // Computes coordinates for spawning a new ground segment
        private Vector3 GetNextGroundCoordinates()
        {
            Vector3 nextCoords = Vector3.zero;
            float streetLength = StackCount == 0 ? 0 : Tools.GetSize(groundList[StackCount - 1].ground, 'z', 'r');

            nextCoords.z = StackCount == 0 ? 0 : groundList[StackCount - 1].ground.transform.position.z + streetLength;

            return nextCoords;
        }

        // Inserts new segment into the Stack
        private void InsertSegmentIntoStack(Segment seg)
        {
            // Before inserting a new street segment into the stack, we remove the oldest one (index = 0), if it would exceed the stack's max size
            if (groundList.Count == StackSize)
            {
                foreach (var obs in groundList[0].obstacles)
                {
                    GameObject.Destroy(obs);
                }

                GameObject.Destroy(groundList[0].ground);

                groundList.RemoveAt(0);
            }

            groundList.Add(seg);
        }
    }


    // Class describing an Item spawner. This object manages the lists of Items (obstacles, power-ups, etc.), selects new items once a new ground segment spawns
    // and places them on the segment.
    // TODO: for now the item placement is completely random. implement an algorithm to avoid path blocking and unbalanced positioning!
    private class ItemSpawner
    {
        public string ItemPath { private get; set; }
        public List<GameObject> selectedItems = new List<GameObject>();
        private List<GameObject> allItems = new List<GameObject>();
        private int minCost;

        // Constructor
        public ItemSpawner(string path)
        {
            ItemPath = path;

            GameObject[] testtest2 = Resources.LoadAll<GameObject>(ItemPath);

            allItems.AddRange(testtest2);

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
        public List<GameObject> FillItemList(int maxItems, int totBudget)
        {
            int budget = totBudget;

            selectedItems.Clear();

            // we add items to the list as long as we don't reach maxItems or the budget is too low to afford even the cheaper item
            while (selectedItems.Count <= maxItems && budget >= minCost)
            {
                int rndIndex = Random.Range(0, allItems.Count);
                bool canAfford = budget > allItems[rndIndex].GetComponent<ObstacleProperties>().Cost;

                if (canAfford)
                {
                    selectedItems.Add(allItems[rndIndex]);
                }
            }
            return selectedItems;
        }

        // this function places all the selected Items on the Ground segment
        public void PlaceItems(GameObject ground, List<GameObject> items = null)
        {
            if (items == null) items = selectedItems;

            List<CellProperties> cells = ground.GetComponent<GroundProperties>().GetGroundCells();

            int rndCellIndex;
            int rndItemIndex;

            // we place items until we have 0 items and/or no space
            while (items.Count > 0 && cells.Count > 0)
            {
                rndCellIndex = Random.Range(0, cells.Count);
                rndItemIndex = Random.Range(0, items.Count);

                GameObject itm = selectedItems[rndItemIndex];
                CellProperties cl = cells[rndCellIndex];

                // TODO: Must eventually account for different size/shape of items
                if (cl.isOccupied == false)
                {
                    GameObject newItem = Instantiate(itm, cl.Coordinates, Quaternion.identity);
                    cells.RemoveAt(rndCellIndex);
                    items.RemoveAt(rndItemIndex);

                }
            }
            return;
        }
    }
}
