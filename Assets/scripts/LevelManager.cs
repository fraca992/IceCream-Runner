using UnityEngine;
using Common;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    #region StreetMovingVariables
    [SerializeField]
    private float streetSpeed = 15;
    [SerializeField]
    private int maxStreets = 5;
    [SerializeField]
    private GameObject streetPrefab;
    
    private GameObject[] streets;
    private Tools.StreetManager lvl1StreetManager;
    #endregion

    #region StreetBudgetVariables
    [SerializeField]
    private int streetBudget = 2;
    [SerializeField]
    private float budgetIncreaseRate = 0.1f;

    public int StreetBudget { get; private set; }
    public float BudgetIncreaseRate { get; private set; }
    #endregion


    private void Awake()
    {
        // Initializing variables
        streets = new GameObject[maxStreets];
        streets[0] = GameObject.Find("Street"); //TODO: may remove the first street and instantiate this too at runtime
        lvl1StreetManager = new Tools.StreetManager(streets, streetPrefab);

        StreetBudget = streetBudget;
        BudgetIncreaseRate = budgetIncreaseRate;
    }

    void FixedUpdate()
    {
        MoveStreets();

        //TODO: street budget tracker/increase
    }

    // Helper Methods
    private void MoveStreets()
    {
        // instantiate new street segments
        lvl1StreetManager.SpawnStreetIfNull();

        // destroy old street segments
        lvl1StreetManager.DestroyStreetIfOld();

        // move street segments
        float adjustedSpeed = streetSpeed / 100;
        lvl1StreetManager.MoveStreets(adjustedSpeed);
    }
}