using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxVelocity = 500;

    private GameObject player;
    private Rigidbody playerRb;
    private float lateralMovement;

    void Awake()
    {
        player = gameObject;
        playerRb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float actualVelocity = maxVelocity * Time.deltaTime; // Ensures that even if we change physics update time, it stays constant
        lateralMovement = Input.GetAxis("Horizontal");


        // Using a temp playerVelocity ensures we can only modify the x component without messing with vertical or forward velocity
        Vector3 playerVelocity = playerRb.velocity;
        playerVelocity.x = lateralMovement * actualVelocity;
        playerRb.velocity = playerVelocity;


    }
}
