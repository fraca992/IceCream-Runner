using UnityEngine;
using Common;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private float streetSpeed = 15;
    [SerializeField]
    private int maxStreets = 5;
    [SerializeField]
    private GameObject streetPrefab;
    [SerializeField]
    private GameObject player;

    private GameObject[] streets;
    private Tools.StreetManager lvl1StreetManager;

    private void Awake()
    {
        // Initializing variables
        streets = new GameObject[maxStreets];
        streets[0] = GameObject.Find("Street"); //TODO: may remove the first street and instantiate this too at runtime

        lvl1StreetManager = new Tools.StreetManager(streets, streetPrefab);
    }

    void FixedUpdate()
    {
        // instantiate new street segments
        lvl1StreetManager.SpawnStreetIfNull();

        // destroy old street segments
        lvl1StreetManager.DestroyStreetIfOld();

        // move street segments
        float adjustedSpeed = streetSpeed / 100;
        lvl1StreetManager.MoveStreets(adjustedSpeed);
        
        // TODO: should control speed of streetcontroller
    }
}