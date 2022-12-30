using System.Collections.Generic;
using UnityEngine;
using Common;

public class GroundProperties : MonoBehaviour
{
    #region Variables
    public int Budget { get; private set; }
    public int Id { get; private set; }
    public float SidewalkWidth { get; private set; }
    public float SidewalkLength { get; private set; }
    public float SidewalkHeight { get; private set; }
    #endregion

    // Constructor
    public void InitializeGroundProperties (int budget, int id)
    {
        Budget = budget;
        Id = id;
        SidewalkWidth = Tools.GetSize(this.transform.GetChild(1).gameObject, 'x');
        SidewalkLength = Tools.GetSize(this.transform.GetChild(1).gameObject, 'z');
        SidewalkHeight = Tools.GetSize(this.transform.GetChild(1).gameObject, 'y');
    }
}