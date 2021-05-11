using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject street;
    [SerializeField]
    private float streetSpeed = 9;
    [SerializeField]
    private short maxStreets = 3;

    public GameObject[] streets { private get; set; }

    private void Awaken()
    {
        streets = new GameObject[maxStreets];
        streetSpeed *= -Time.deltaTime/10;
    }

    // Update is called once per frame
    void Update()
    {
        street.transform.Translate(0, 0, streetSpeed);
    }
}