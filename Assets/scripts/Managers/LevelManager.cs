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
    private int stackSize = 7;
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

        lvl1Stack = new StreetStack(stackSize);
        for (int i = 0; i < stackSize; i++)
        {
            streetSpawnCoordinates = GetNextStreetCoordinates(lvl1Stack);
            nextStreet = SpawnStreet(streetPrefabPath, streetSpawnCoordinates, streetbudget, Tools.GetNextValue(), streetXCellNum, streetZCellNum, streetCellPoints);
            lvl1Stack.InsertStreetIntoStack(nextStreet);
        }
    }

    void Update()
    {
        // Moving the streets in the stack
        streetMovement.z -= levelSpeed * Time.deltaTime;
        foreach (GameObject strt in lvl1Stack.streets)
        {
            strt.GetComponent<Rigidbody>().MovePosition(streetMovement);
        }

        //TODO: spawn street continuously when 1 full street is behind the player
        float lastStreetPositionZ = lvl1Stack.streets[0].transform.position.z;
        float playerPosition = player.transform.position.z;
        float distanceBuffer = Tools.GetSize(lvl1Stack.streets[0], 'z');

        if (lastStreetPositionZ + distanceBuffer < playerPosition)
        {
            streetSpawnCoordinates = GetNextStreetCoordinates(lvl1Stack); // è copiato dallo spawn iniziale in Awakw. non mi piace, idealmente dovrebbe essere una funzione (o integra le 3 linee di codice in SpawnStreet direttamente)
            nextStreet = SpawnStreet(streetPrefabPath, streetSpawnCoordinates, streetbudget, Tools.GetNextValue(), streetXCellNum, streetZCellNum, streetCellPoints);
            lvl1Stack.InsertStreetIntoStack(nextStreet);
        }
    }

    public class StreetStack
    {
        public int StackSize { get; private set; }
        public int StackCount { get { return streets.Count; } }

        public List<GameObject> streets = new List<GameObject>();

        public StreetStack(int size = 5)
        {
            StackSize = size;
        }

        internal void InsertStreetIntoStack (GameObject street)
        {
            // Before inserting a new street segment into the stack, we remove the oldest one (index = 0), if it would exceed the stack's max size
            if(streets.Count == StackSize) streets.RemoveAt(0); // REVIEW may need a destroy game object first?
            streets.Add(street);
        }
    }

    private GameObject SpawnStreet(string path, Vector3 spawnCoordinates, int budget, int id, int xCellNum, int zCellNum, int cellPoints)
    {
        GameObject nextStreet = Instantiate(Resources.Load<GameObject>(path), spawnCoordinates, Quaternion.identity);
        nextStreet.GetComponent<StreetProperties>().InitializeStreetProperties(budget, id, xCellNum, zCellNum, cellPoints);

        return nextStreet;
    }

    private Vector3 GetNextStreetCoordinates(StreetStack stack)
    {
        Vector3 nextCoords = Vector3.zero;
        float streetLength = stack.StackCount == 0 ? 0 : Tools.GetSize(stack.streets[lvl1Stack.StackCount - 1], 'z');

        nextCoords.z = stack.StackCount == 0 ? 0 : stack.streets[stack.StackCount - 1].transform.position.z + streetLength;

        return nextCoords;
    }
}
