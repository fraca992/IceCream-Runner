using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject street;
    [SerializeField]
    private float streetSpeed = 10;

    private void Start()
    {
        streetSpeed *= -Time.deltaTime/10;
    }

    // Update is called once per frame
    void Update()
    {
        street.transform.Translate(0, 0, streetSpeed);
    }
}
