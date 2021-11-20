using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetProperties : MonoBehaviour
{
    /*
* A SP must contain its `cells`' points ---> `Street Properties`
- A SP must compute its `cells`' coordinates ---> `Street Properties`*/

    public int StreetBudget { get; set; }
    public int StreeetIndex { get; set; }
    public int StreetId { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Cell[] GetStreetCells(); // Methods that returns an array of the N cells of the street, with updated coordinates.
    }
}
