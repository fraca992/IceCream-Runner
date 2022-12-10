using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class LevelManager : MonoBehaviour
{
    #region General Variables
    [SerializeField]
    private float levelSpeed = 10f;
    private Vector3 streetMovement = Vector3.zero;
    private GameObject player;
    #endregion

    #region Street Variables
    [SerializeField]
    private int stackSize = 10;
    public SegmentClasses.SegmentSpawner groundStack;
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

        // Spawn initial ground segments
        groundStack = new SegmentClasses.SegmentSpawner(groundPrefabPath, obstacleFolderPath, stackSize, maxObstacles);
        for (int i = 0; i < stackSize; i++)
        {
            groundStack.SpawnSegment(groundBudget, streetXCellNum, streetZCellNum);
        }
    }

    private void FixedUpdate()
    {
        // Spawn ground segments continuously
        float lastStreetPositionZ = groundStack.groundStack[0].ground.transform.position.z;
        float playerPosition = player.transform.position.z;
        float distanceBuffer = Tools.GetSize(groundStack.groundStack[0].ground, 'z') * 2;

        if (lastStreetPositionZ + distanceBuffer < playerPosition)
        {
            groundStack.SpawnSegment(groundBudget, streetXCellNum, streetZCellNum);
        }

        //Moving the ground segments in the stack
        streetMovement.z = -levelSpeed;
        foreach (SegmentClasses.Segment seg in groundStack.groundStack)
        {
            seg.ground.GetComponent<Rigidbody>().MovePosition(seg.ground.transform.position + streetMovement); 
        }
    }
}
