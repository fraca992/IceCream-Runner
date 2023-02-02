using System.Collections.Generic;
using UnityEngine;
using Common;

public class StreetPieceProperties : MonoBehaviour
{
    #region Variables
    public int Budget { get; private set; }
    public int Id { get; private set; }
    public float Length { get; private set; }
    public float SidewalkWidth { get; private set; }
    public float SidewalkHeight { get; private set; }
    public float RoadWidth { get; private set; }
    public float RoadHeight { get; private set; }
    #endregion

    // Constructor
    public void InitializeGroundProperties (int id)
    {
        Id = id;

        Length = Tools.GetSize(this.transform.gameObject, 'z');
        SidewalkWidth = Tools.GetSize(this.transform.GetChild(1).gameObject, 'x');
        SidewalkHeight = Tools.GetSize(this.transform.GetChild(1).gameObject, 'y');
        RoadWidth = Tools.GetSize(this.transform.GetChild(0).gameObject, 'x');
        RoadHeight = Tools.GetSize(this.transform.GetChild(0).gameObject, 'y');
    }
}