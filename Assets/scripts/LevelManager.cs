using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private float streetSpeed = 9;
    [SerializeField]
    private int maxStreets = 3;
    [SerializeField]
    private GameObject streetPrefab;
    [SerializeField]
    private GameObject player;

    private GameObject[] streets;
    private Renderer streetRenderer;
    private float streetLength;
    private int streetIndex = 0;
    Vector3 nextStreetPosition = new Vector3(0, 0, 0);

    private void Awake()
    {
        streets = new GameObject[maxStreets];
        streets[0] = GameObject.Find("Street");

        streetRenderer = streetPrefab.GetComponentInChildren<MeshRenderer>();
        streetLength = streetRenderer.bounds.size.z;
    }

    void Update()
    {
        // instantiate new street segments
        for (int i = 0; i < streets.Length; i++)
        {
            if (streets[i] == null)
            {
                nextStreetPosition.z = streets[streetIndex].transform.position.z + streetLength;
                streets[i] = Instantiate(streetPrefab, nextStreetPosition, Quaternion.identity);
                streetIndex = i;
            }
        }

        // destroy old street segments
        for (int i = 0; i < streets.Length; i++)
        {
            if (player.transform.position.z > streets[i].transform.position.z + 2*streetLength)
            {
                Destroy(streets[i]);
            }
        }

        // move street segments
        foreach (GameObject street in streets)
        {
            if (street != null)
            {
                street.transform.Translate(0, 0, -streetSpeed * Time.deltaTime);
            }
        }
    }
}