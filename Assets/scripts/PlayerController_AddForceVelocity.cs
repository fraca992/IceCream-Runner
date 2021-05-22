using UnityEngine;

public class PlayerController_AddForceVelocity : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRb;

    [SerializeField]
    private float moveVelocityChange = 20;

    void Awake()
    {
        player = gameObject;
        playerRb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // non mi piace manco per un caxxo
        if (IsMovingLeft) playerRb.AddForce(moveVelocityChange * Vector3.left, ForceMode.VelocityChange);
        if (IsMovingRight) playerRb.AddForce(moveVelocityChange * Vector3.right, ForceMode.VelocityChange);
    }

    private bool IsMovingLeft => Input.GetAxis("Horizontal") < 0;
    private bool IsMovingRight => Input.GetAxis("Horizontal") > 0;
}