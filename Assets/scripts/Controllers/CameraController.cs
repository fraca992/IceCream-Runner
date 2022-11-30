using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;

        newPosition.x = player.transform.position.x;
        transform.position = newPosition;
    }
}