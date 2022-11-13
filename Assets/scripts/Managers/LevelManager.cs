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
    private GroundStack groundStack;
    private GameObject nextGround;
    private string groundPrefabPath = "Grounds/Street";
    private string obstacleFolderPath = "Obstacles";
    private Vector3 streetSpawnCoordinates = Vector3.zero;
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
            groundStack.SpawnGround(groundBudget, streetXCellNum, streetZCellNum);
        }
    }

    private void FixedUpdate()
    {
        // Spawn street continuously when 2 full street is behind the player
        float lastStreetPositionZ = groundStack.groundList[0].transform.position.z;
        float playerPosition = player.transform.position.z;
        float distanceBuffer = Tools.GetSize(groundStack.groundList[0], 'z') * 2;

        if (lastStreetPositionZ + distanceBuffer < playerPosition)
        {
            groundStack.SpawnGround(groundBudget, streetXCellNum, streetZCellNum);
        }

        //Moving the streets in the stack
        streetMovement.z = -levelSpeed;
        foreach (GameObject strt in groundStack.groundList)
        {
            strt.GetComponent<Rigidbody>().MovePosition(strt.transform.position + streetMovement); 
        }
    }

    public class GroundStack
    {
        public int StackSize { get; private set; }
        public int maxObstacles { get; private set; }
        public int StackCount { get { return groundList.Count; } }
        public string GroundPath { private get; set; }

        public List<GameObject> groundList = new List<GameObject>();

        private ItemSpawner obstacleSpawner;

        public GroundStack(string itmPath, string obsPath, int size, int maxObs)
        {
            StackSize = size;
            maxObstacles = maxObs;
            GroundPath = itmPath;

            obstacleSpawner = new ItemSpawner(obsPath);
        }

        public GameObject SpawnGround(int totalBudget, int xCellNum, int zCellNum)
        {
            Vector3 spawnCoordinates = GetNextGroundCoordinates();

            GameObject newGround = Instantiate(Resources.Load<GameObject>(GroundPath), spawnCoordinates, Quaternion.identity);
            newGround.GetComponent<GroundProperties>().InitializeGroundProperties(totalBudget, Tools.GetNextValue(), xCellNum, zCellNum);
            InsertGroundIntoStack(newGround);

            // Spawning obstacles
            obstacleSpawner.FillItemList(maxObstacles, totalBudget);
            obstacleSpawner.PlaceItems(newGround);

            return newGround;
        }

        private Vector3 GetNextGroundCoordinates()
        {
            Vector3 nextCoords = Vector3.zero;
            float streetLength = StackCount == 0 ? 0 : Tools.GetSize(groundList[StackCount - 1], 'z', 'r');

            nextCoords.z = StackCount == 0 ? 0 : groundList[StackCount - 1].transform.position.z + streetLength;

            return nextCoords;
        }
        private void InsertGroundIntoStack(GameObject ground)
        {
            // Before inserting a new street segment into the stack, we remove the oldest one (index = 0), if it would exceed the stack's max size
            if (groundList.Count == StackSize)
            {
                GameObject.Destroy(groundList[0]);
                groundList.RemoveAt(0);
            }

            groundList.Add(ground);
        }
    }

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

        // this function fills selectedItems
        public List<GameObject> FillItemList(int maxItems, int totBudget)
        {
            int budget = totBudget;

            selectedItems.Clear();

            // we add items to the list as long as we don't reach maxItems or the busget is too low to afford even the cheaper item
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

        // this function places all the selected Items on the Ground piece
        public void PlaceItems(GameObject ground, List<GameObject> items = null)
        {
            if (items == null) items = selectedItems;

            List<Cell> cells = ground.GetComponent<GroundProperties>().GetGroundCells();

            int rndCellIndex;
            int rndItemIndex;

            // we place items until we have 0 items and/or no space
            while (items.Count > 0 && cells.Count > 0)
            {
                rndCellIndex = Random.Range(0, cells.Count);
                rndItemIndex = Random.Range(0, items.Count);

                GameObject itm = selectedItems[rndItemIndex];
                Cell cl = cells[rndCellIndex];

                // TODO: Must account for different size/shape of items
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
