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
        float actualVelocity = maxVelocity * Time.deltaTime; // Ensures that even if we change physics update time, it stays constant

        lateralMovement = Input.GetAxis("Horizontal");

        playerRb.velocity = lateralMovement * actualVelocity * Vector3.right;
    }
}