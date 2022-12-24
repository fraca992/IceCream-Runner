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
    public SegmentClasses.SegmentSpawner lvl1Spawner;
    private string groundPrefabPath = "Grounds/Street";
    private string obstacleFolderPath = "Obstacles";
    [SerializeField]
    private int groundBudget = 100;
    [SerializeField]
    private int maxObstacles = 8;
    [SerializeField]
    private int streetXCellNum = 2;
    [SerializeField]
    private int streetZCellNum = 10;
    #endregion


    void Awake()
    {
        player = GameObject.Find("Player");

        // Spawn initial segments
        lvl1Spawner = new SegmentClasses.SegmentSpawner(groundPrefabPath, obstacleFolderPath, stackSize, maxObstacles);
        for (int i = 0; i < stackSize; i++)
        {
            lvl1Spawner.SpawnSegment(groundBudget, streetXCellNum, streetZCellNum);
        }
    }

    private void FixedUpdate()
    {
        // Spawn segments continuously
        float lastStreetPositionZ = lvl1Spawner.segmentStack[0].ground.transform.position.z;
        float playerPosition = player.transform.position.z;
        float distanceBuffer = Tools.GetSize(lvl1Spawner.segmentStack[0].ground, 'z') * 2;

        if (lastStreetPositionZ + distanceBuffer < playerPosition)
        {
            lvl1Spawner.SpawnSegment(groundBudget, streetXCellNum, streetZCellNum);
        }

        //Moving the ground segments in the stack
        segmentMovement.z = -levelSpeed;
        foreach (SegmentClasses.Segment seg in lvl1Spawner.segmentStack) // TODO: could be a function
        {
            //moving ground
            seg.ground.GetComponent<Rigidbody>().MovePosition(seg.ground.transform.position + segmentMovement);

            //moving obstacles
            //foreach (GameObject obs in seg.obstacles)
            //{
            //    obs.GetComponent<Rigidbody>().velocity = seg.ground.GetComponent<Rigidbody>().velocity; //MovePosition(seg.ground.transform.position + segmentMovement);
            //}
        }
    }
}
