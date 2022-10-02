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
    private StreetStack lvl1Stack;
    private GameObject nextStreet;
    private string streetPrefabPath = "Streets/Street";
    private Vector3 streetSpawnCoordinates = Vector3.zero;
    [SerializeField]
    private int streetbudget = 100;
    [SerializeField]
    private int streetXCellNum = 2;
    [SerializeField]
    private int streetZCellNum = 10;
    [SerializeField]
    private int streetCellPoints = 10;
    #endregion

    void Awake()
    {
        player = GameObject.Find("Player");

        lvl1Stack = new StreetStack(stackSize, streetPrefabPath);
        for (int i = 0; i < stackSize; i++)
        {
            lvl1Stack.SpawnStreet(streetbudget, streetXCellNum, streetZCellNum, streetCellPoints);
        }
    }

    void Update()
    {
        // Spawn street continuously when 1 full street is behind the player
        float lastStreetPositionZ = lvl1Stack.streets[0].transform.position.z;
        float playerPosition = player.transform.position.z;
        float distanceBuffer = Tools.GetSize(lvl1Stack.streets[0], 'z')*2;

        if (lastStreetPositionZ + distanceBuffer < playerPosition)
        {
            lvl1Stack.SpawnStreet(streetbudget, streetXCellNum, streetZCellNum, streetCellPoints);
        }
    }

    private void FixedUpdate()
    {
        // Moving the streets in the stack
        streetMovement.z = -levelSpeed; //* Time.deltaTime;
        foreach (GameObject strt in lvl1Stack.streets)
        {
            strt.GetComponent<Rigidbody>().MovePosition(strt.transform.position + streetMovement);
        }
    }

    public class StreetStack
    {
        public int StackSize { get; private set; }
        public int StackCount { get { return streets.Count; } }
        public string Path { private get; set; }

        public List<GameObject> streets = new List<GameObject>();

        public StreetStack(int size = 5, string path = "")
        {
            StackSize = size;
            Path = path;
        }

        public GameObject SpawnStreet(int budget, int xCellNum, int zCellNum, int cellPoints)
        {
            Vector3 spawnCoordinates = GetNextStreetCoordinates();

            GameObject nextStreet = Instantiate(Resources.Load<GameObject>(Path), spawnCoordinates, Quaternion.identity);
            nextStreet.GetComponent<StreetProperties>().InitializeStreetProperties(budget, Tools.GetNextValue(), xCellNum, zCellNum, cellPoints);
            InsertStreetIntoStack(nextStreet);

            return nextStreet;
        }

        private Vector3 GetNextStreetCoordinates()
        {
            Vector3 nextCoords = Vector3.zero;
            float streetLength = StackCount == 0 ? 0 : Tools.GetSize(streets[StackCount - 1], 'z');

            nextCoords.z = StackCount == 0 ? 0 : streets[StackCount - 1].transform.position.z + streetLength;

            return nextCoords;
        }
        private void InsertStreetIntoStack(GameObject street)
        {
            // Before inserting a new street segment into the stack, we remove the oldest one (index = 0), if it would exceed the stack's max size
            if (streets.Count == StackSize)
            {
                GameObject.Destroy(streets[0]);
                streets.RemoveAt(0);
            }

            streets.Add(street);
        }
    }
}
