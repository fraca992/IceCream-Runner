using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class LevelManager : MonoBehaviour
{
    #region General Variables
    [SerializeField]
    private float levelSpeed = 10f;
    private Vector3 segmentMovement = Vector3.zero;
    private GameObject player;
    #endregion

    #region Street Variables
    [SerializeField]
    private int stackSize = 10;
    public SegmentManager lvl1SegmentManager;
    private string streetPiecePrefabPath = "StreetPieces/CityStreet";
    private string obstacleFolderPath = "Obstacles";
    [SerializeField]
    private int segmentBudget = 100;
    [SerializeField]
    private int maxObstacles = 8;
    [SerializeField]
    private int XCellNum = 2;
    [SerializeField]
    private int ZCellNum = 10;
    #endregion


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // Spawn initial segments
        lvl1SegmentManager = new SegmentManager(streetPiecePrefabPath,XCellNum,ZCellNum,obstacleFolderPath, segmentBudget, maxObstacles,stackSize);
        for (int i = 0; i < stackSize; i++)
        {
            lvl1SegmentManager.SpawnSegment();
        }
    }

    private void FixedUpdate()
    {
        // Spawn segments continuously
        float lastStreetPositionZ = lvl1SegmentManager.segmentStack[0].StreetPiece.gameObject.transform.position.z;
        float playerPosition = player.transform.position.z;
        float distanceBuffer = lvl1SegmentManager.segmentStack[0].StreetPiece.Length * 2;

        if (lastStreetPositionZ + distanceBuffer < playerPosition)
        {
            lvl1SegmentManager.SpawnSegment();
        }

        //Moving the ground segments in the stack
        segmentMovement.z = -levelSpeed;
        foreach (SegmentProperties seg in lvl1SegmentManager.segmentStack) // TODO: could be a function
        {
            //moving ground
            seg.StreetPiece.gameObject.GetComponent<Rigidbody>().MovePosition(seg.StreetPiece.gameObject.transform.position + segmentMovement);

            //moving obstacles
            //foreach (GameObject obs in seg.obstacles)
            //{
            //    obs.GetComponent<Rigidbody>().velocity = seg.ground.GetComponent<Rigidbody>().velocity; //MovePosition(seg.ground.transform.position + segmentMovement);
            //}
        }
    }
}
