using UnityEngine;
using Controller;
using Properties;

namespace Manager
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField]
        private int streetBudget = 2;
        [SerializeField]
        private float budgetIncreaseRate = 0.1f;

        private ItemController obstaclesController;

        public bool Spawn; // DEBUG spawn
        public GameObject Street; // DEBUG spawn

        private void Awake()
        {
            obstaclesController = new ItemController("Obstacles");
        }


        void Update()
        {
            if (Spawn) // DEBUG spawn
            {
                obstaclesController.SpawnItems(10,Street.GetComponent<StreetProperties>().streetCells);
                Spawn = false;
            }
        }
    }
}