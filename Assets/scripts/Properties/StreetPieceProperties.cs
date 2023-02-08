using System.Collections.Generic;
using UnityEngine;
using Common;

public class StreetPieceProperties : MonoBehaviour
{
    #region Variables
    public int Id { get; private set; }
    public float Length { get; private set; }
    public float Width { get; private set; }
    public float SidewalkWidth { get; private set; }
    public float SidewalkHeight { get; private set; }
    public float RoadWidth { get; private set; }
    public float RoadHeight { get; private set; }
    #endregion

    // Constructor
    public void SPConstructor (int id)
    {
        Id = id;

        SidewalkWidth = Tools.GetSize(this.transform.GetChild(1).gameObject, 'x');
        SidewalkHeight = Tools.GetSize(this.transform.GetChild(1).gameObject, 'y');
        RoadWidth = Tools.GetSize(this.transform.GetChild(0).gameObject, 'x');
        RoadHeight = Tools.GetSize(this.transform.GetChild(0).gameObject, 'y');
        Length = Tools.GetSize(this.transform.gameObject, 'z');
        Width = 2*SidewalkWidth + RoadWidth;
    }
}