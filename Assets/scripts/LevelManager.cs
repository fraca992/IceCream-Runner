using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private float streetSpeed = 9;
    [SerializeField]
    private short maxStreets = 3;
    [SerializeField]
    private GameObject streetPrefab;
    [SerializeField]
    private GameObject player;

    private GameObject[] streets;
    private Renderer streetRenderer;
    private float streetLength;

    private void Awake()
    {
        streets = new GameObject[maxStreets];
        streets[0] = GameObject.Find("Street");

        streetRenderer = streetPrefab.GetComponentInChildren<MeshRenderer>();
        streetLength = streetRenderer.bounds.size.z;
    }

    void Update()
    {
        Vector3 nextStreetPosition = new Vector3();
        if (streets[streets.Length-1] != null)
        {
            nextStreetPosition.z = streets[streets.Length-1].transform.position.z + streetLength;
        }

        for (int i = 0; i < streets.Length; i++)
        {
            if (streets[i] == null) // TODO: controlla se c'è un modo più compatto di scrivere questo con il punto interrogativo
            {
                streets[i] = Instantiate(streetPrefab, nextStreetPosition + new Vector3(0, 0, i*streetLength), Quaternion.identity);
            }
        }

        // destroy old street segments //TODO: lascia una strada come "buffer" prima di cancellarla
        for (int i = 0; i < streets.Length; i++)
        {
            if (player.transform.position.z > streets[i].transform.position.z + streetLength)
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