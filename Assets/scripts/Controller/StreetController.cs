using UnityEngine;
using Common;
using Properties;
using Manager;

namespace Controller
{
    public class StreetController : MonoBehaviour
    {
        #region Street Variables
        [SerializeField]
        private float streetSpeed = 15;
        [SerializeField]
        private int maxStreets = 5;
        [SerializeField]
        private GameObject streetPrefab;
        
        private StreetManager lvl1StreetManager;
        private GameObject newestStreetIndex;
        #endregion

        #region Cell Variables
        [SerializeField]
        private int zCellNumber = 10;
        [SerializeField]
        private int xCellNumber = 2;
        [SerializeField]
        private int startCellValue = 1;

        private int totCellNum;
        private Vector3[] cellCoords;
        private StreetProperties.CellProperties[] streetCells;
        private Vector3 oldPosition;
        private Vector3 newPosition;
        private Vector3 movement;
        #endregion

        private void Awake()
        {
            // Initializing variables
            lvl1StreetManager = new StreetManager(maxStreets, streetPrefab);
            totCellNum = 2 * zCellNumber * xCellNumber;
        }

        void FixedUpdate()
        {
            // instantiate new street segments and their cells
            newestStreetIndex = lvl1StreetManager.SpawnStreetIfNull(); //REVIEW: could call Initialize Cells inside SpawnStreetIfNull. Is there a reason for doing it explicitely?
            if (newestStreet != null) lvl1StreetManager.InitializeCells(newestStreetIndex, xCellNumber, totCellNum, startCellValue);

            // destroy old street segments
            lvl1StreetManager.DestroyStreetIfOld();

            // move street segments
            float adjustedSpeed = streetSpeed * Time.deltaTime;
            lvl1StreetManager.MoveStreets(adjustedSpeed);

            // moving cell coordinates with the street
            //TODO: put inside MoveStreets
            for (int i = 0; i < maxStreets; i++)
            {
                cellCoords = lvl1StreetManager.MoveCellCoordinates(movement, cellCoords);
                for (int j = 0; j < streetCells.Length; j++)
                {
                    streetCells[i].CellCoordinates = cellCoords[i];
                }
            }
        }
        //private void OnDrawGizmos() //DEBUG gizmos
        //{
        //    if (!Application.isPlaying) return;

        //    Gizmos.color = Color.red;
        //    for (int i = 0; i < streetCells.Length; i++)
        //    {
        //        Gizmos.DrawCube(streetCells[i].CellCoordinates + 0.5f * Vector3.up, new Vector3(1, 1, 1));
        //    }

        //    UnityEditor.Handles.color = Color.green;
        //    for (int i = 0; i < streetCells.Length; i++)
        //    {
        //        UnityEditor.Handles.Label(streetCells[i].CellCoordinates + 1.5f * Vector3.up, streetCells[i].CellValue.ToString());
        //    }
        //}
    }
}