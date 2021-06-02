using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRb;
    private float lateralMovement;

    [SerializeField]
    private float maxVelocity = 500;

    void Awake()
    {
        player = gameObject;
        playerRb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        lateralMovement = Input.GetAxis("Horizontal");

        playerRb.velocity = lateralMovement * maxVelocity * Time.deltaTime * Vector3.right;
    }
}