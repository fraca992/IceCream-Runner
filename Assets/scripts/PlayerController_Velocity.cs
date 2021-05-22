using UnityEngine;

public class PlayerController_Velocity : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRb;

    [SerializeField]
    private float moveVelocity = 20;

    void Awake()
    {
        player = gameObject;
        playerRb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // non mi piace manco per un caxxo
        if (IsMovingLeft) playerRb.velocity = moveVelocity * Vector3.left;
        if (IsMovingRight) playerRb.velocity = moveVelocity * Vector3.right;
    }

    private bool IsMovingLeft => Input.GetAxis("Horizontal") < 0;
    private bool IsMovingRight => Input.GetAxis("Horizontal") > 0;
}