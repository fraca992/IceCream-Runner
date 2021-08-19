using UnityEngine;
using Controller;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        #region Street Variables
        [SerializeField]
        private GameObject streetPrefab;
        [SerializeField]
        private float streetSpeed = 15f;
        [SerializeField]
        private int streetBudget = 10;
        [SerializeField]
        private int maxStreets = 5;
        
        private StreetController lvl1StreetController;

        private ItemController obstaclesController;
        #endregion

        #region Cell Variables
        [SerializeField]
        private int zCellNumber = 10;
        [SerializeField]
        private int xCellNumber = 2;
        [SerializeField]
        private int baseCellValue = 1;

        private int totCellNum;
        #endregion

        //DEBUG spawn
        public bool dbSpawn;
        public int num;

        private void Awake()
        {
            // Initializing variables
            lvl1StreetController = new StreetController(maxStreets, streetPrefab);
            totCellNum = 2 * zCellNumber * xCellNumber;
            bool hasSpawned;
            do
            {
                hasSpawned = lvl1StreetController.SpawnStreetIfNeeded(streetBudget, xCellNumber, totCellNum, baseCellValue);
            } while (hasSpawned);

            obstaclesController = new ItemController("Obstacles");
        }

        void FixedUpdate()
        {
            bool hasSpawned;

            // instantiate a new Street segment if needed, and initialises its cells
            hasSpawned = lvl1StreetController.SpawnStreetIfNeeded(streetBudget, xCellNumber, totCellNum, baseCellValue);

            // destroy old street segments
            lvl1StreetController.DestroyStreetIfOld();

            // move street segments
            float adjustedSpeed = streetSpeed * Time.deltaTime;
            lvl1StreetController.MoveStreets(adjustedSpeed);

            // spawn item
            //if (hasSpawned)
            //{
            //    obstaclesController.SpawnItems(streetBudget, lvl1StreetController.Streets[lvl1StreetController.Streets.Count - 1].Cells);
            //}

            //DEBUG spawn items w/ button
            if (dbSpawn)
            {
                obstaclesController.SpawnItems(streetBudget, lvl1StreetController.Streets[num].Cells);
                dbSpawn = false;
            }
        }

        private void OnDrawGizmos() //DEBUG gizmos
        {
            if (!Application.isPlaying) return;

            System.Collections.Generic.List<Common.Street> streets = lvl1StreetController.Streets;
            Gizmos.color = Color.red;
            for (int i = 0; i < streets.Count; i++)
            {
                for (int k = 0; k < streets[i].Cells.Count; k++)
                {
                    Gizmos.DrawCube(streets[i].Cells[k].Coordinates + 0.5f * Vector3.up, new Vector3(1, 1, 1));

                    UnityEditor.Handles.Label(streets[i].Cells[k].Coordinates + 1.5f * Vector3.up, streets[i].Cells[k].Value.ToString());
                }
            }
        }
    }
}