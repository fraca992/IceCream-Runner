using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRb;
    private Vector3 moveForce = new Vector3();

    [SerializeField]
    private float lateralMoveForce = 20;

    void Awake()
    {
        player = gameObject;
        playerRb = player.GetComponent<Rigidbody>();

        moveForce.x = lateralMoveForce;
    }

    void FixedUpdate()
    {
        // Physic based movement, NOT DEFINITIVE
        if (Input.GetAxis("Horizontal") < 0) playerRb.AddForce(-moveForce);

        if (Input.GetAxis("Horizontal") > 0) playerRb.AddForce(moveForce);
    }
}