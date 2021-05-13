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
    private GameObject level;

    private GameObject[] streets;
    private Renderer streetRenderer;
    private float streetLength;

    private void Awake()
    {
        streets = new GameObject[maxStreets];
        streets[0] = GameObject.Find("Street");

        streetRenderer = streetPrefab.GetComponent<MeshRenderer>();
        streetLength = streetRenderer.bounds.size.z;
    }

    void Update()
    {
        float streetOverhead = streetLength/2f;
        // TODO: considerare nell'instantiate che le strade si muovono, e quindi il punto di spawn è variabile
        for (int i = 0; i < streets.Length; i++)
        {
            if (streets[i] == null)
            {
                streets[i] = Instantiate(streetPrefab, new Vector3(0, 0, streetOverhead + (i * streetLength)), Quaternion.identity, level.transform);
            }
        }


            level.transform.Translate(0, 0, - streetSpeed * Time.deltaTime);

    }
}