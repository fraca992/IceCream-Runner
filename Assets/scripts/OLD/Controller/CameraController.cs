using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
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

}