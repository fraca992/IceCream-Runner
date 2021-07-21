using UnityEngine;
using Controller;

namespace Manager
{
    public class StreetManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject player;
        [SerializeField]
        private float streetSpeed = 15;
        [SerializeField]
        private int maxStreets = 5;
        [SerializeField]
        private GameObject streetPrefab;

        private GameObject[] streets;
        private StreetController lvl1StreetManager;

        private void Awake()
        {
            // Initializing variables
            streets = new GameObject[maxStreets];
            streets[0] = GameObject.Find("Street"); //REVIEW: may remove the first street and instantiate this too at runtime
            lvl1StreetManager = new StreetController(streets, streetPrefab);
        }

        void FixedUpdate()
        {
            // instantiate new street segments
            lvl1StreetManager.SpawnStreetIfNull();

            // destroy old street segments
            lvl1StreetManager.DestroyStreetIfOld();

            // move street segments
            float adjustedSpeed = streetSpeed * Time.deltaTime;
            lvl1StreetManager.MoveStreets(adjustedSpeed);
        }
    }
}