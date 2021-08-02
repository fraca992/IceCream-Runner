using UnityEngine;
using Common;
using Manager;

namespace Controller
{
    public class StreetController : MonoBehaviour
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
        
        private StreetManager lvl1StreetManager;
        private int? newestStreetIndex;
        #endregion

        #region Cell Variables
        [SerializeField]
        private int zCellNumber = 10;
        [SerializeField]
        private int xCellNumber = 2;
        [SerializeField]
        private int baseCellValue = 1;

        private int totCellNum;
        private Vector3[] cellCoords;
        private Vector3 oldPosition;
        private Vector3 newPosition;
        #endregion

        private void Awake()
        {
            // Initializing variables
            lvl1StreetManager = new StreetManager(maxStreets, streetPrefab);
            totCellNum = 2 * zCellNumber * xCellNumber;

            do
            {
                newestStreetIndex = lvl1StreetManager.SpawnStreetIfNeeded(streetBudget, xCellNumber, totCellNum, baseCellValue);
            } while (newestStreetIndex != null);
        }

        void FixedUpdate()
        {
            // instantiate a new Street segment if needed, and initialises its cells
            newestStreetIndex = lvl1StreetManager.SpawnStreetIfNeeded(streetBudget, xCellNumber, totCellNum, baseCellValue);

            // destroy old street segments
            lvl1StreetManager.DestroyStreetIfOld();

            // move street segments
            float adjustedSpeed = streetSpeed * Time.deltaTime;
            lvl1StreetManager.MoveStreets(adjustedSpeed);
        }
    }
}