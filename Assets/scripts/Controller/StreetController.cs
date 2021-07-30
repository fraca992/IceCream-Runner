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
        private GameObject player;
        [SerializeField]
        private float streetSpeed = 15;
        [SerializeField]
        private int maxStreets = 5;
        [SerializeField]
        private GameObject streetPrefab;

        private GameObject[] streets;
        private StreetManager lvl1StreetManager;
        private GameObject newestStreet;
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
            streets = new GameObject[maxStreets];
            lvl1StreetManager = new StreetManager(streets, streetPrefab);
            totCellNum = 2 * zCellNumber * xCellNumber;

            streets[0] = GameObject.Find("Street"); //REVIEW: may remove the first street and instantiate this too at runtime
        }

        void FixedUpdate()
        {
            // instantiate new street segments and their cells
            newestStreet = lvl1StreetManager.SpawnStreetIfNull();
            if (newestStreet != null) lvl1StreetManager.InitializeCells(newestStreet, totCellNum, totCellNum, startCellValue);

            // destroy old street segments
            lvl1StreetManager.DestroyStreetIfOld();

            // move street segments
            float adjustedSpeed = streetSpeed * Time.deltaTime;
            lvl1StreetManager.MoveStreets(adjustedSpeed);


            // moving cell coordinates with the street
            for (int i = 0; i < streets.Length; i++)
            {
                newPosition = streets[i].transform.position;
                movement = newPosition - oldPosition;
                oldPosition = newPosition;
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