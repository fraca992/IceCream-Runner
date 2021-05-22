using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRb;

    [SerializeField]
    private float moveForce = 20;

    void Awake()
    {
        player = gameObject;
        playerRb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Physic based movement, NOT DEFINITIVE
        if (IsMovingLeft) playerRb.AddForce(moveForce*Vector3.left); //TODO: could use Input.GetAxis to have a number between -1,+1 and multiply by a moveForce Vector3? This way the player would have a smooth start (also support moving slower on mobile?)?
        if (IsMovingRight) playerRb.AddForce(moveForce*Vector3.right);
    }

    private bool IsMovingLeft => Input.GetAxis("Horizontal") < 0;
    private bool IsMovingRight => Input.GetAxis("Horizontal") > 0;
}