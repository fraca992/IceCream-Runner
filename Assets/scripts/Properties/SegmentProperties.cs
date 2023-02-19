using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Common;


public class SegmentProperties
{
    public int Budget { get; private set; }
    public StreetPieceProperties StreetPiece { get; private set; }
    public List<ObstacleProperties> Obstacles { get; private set; }

    private int xCellNumber;
    private int zCellNumber;
    private float cellSize;
    private List<CellProperties> _Cells;
    public ReadOnlyCollection<CellProperties> Cells { get { return Tools.GetUpdatedCellCoordinates(StreetPiece.gameObject, _Cells, xCellNumber, cellSize).AsReadOnly(); } }

    // Constructor
    public SegmentProperties(int budget, StreetPieceProperties strP, List<ObstacleProperties> obs, List<CellProperties> cls, int xCellNum, int zCellNum)
    {
        Budget = budget;
        StreetPiece = strP;
        Obstacles = obs;
        _Cells = cls;
        xCellNumber = xCellNum;
        zCellNumber= zCellNum;
    }
}
