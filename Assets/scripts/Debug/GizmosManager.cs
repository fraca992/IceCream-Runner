using System.Collections.Generic;
using UnityEngine;
using Common;

public class GizmosManager : MonoBehaviour
{
    //    public bool isDebugging = false;

    //    private void OnDrawGizmos()
    //    {
    //        if (!isDebugging) return;

    //        List<Segment.Segment> segments = this.GetComponent<LevelManager>().lvl1Spawner.segmentStack;
    //        if (segments == null) return; 

    //        HighlightCells(segments);
    //        HighlightGroundandObstacles();
    //    }

    //    // drawing the boundaries and number of cells on all ground segments
    //    private void HighlightCells(List<Segment.Segment> segments)
    //    {
    //        int j = 0;
    //        foreach (var seg in segments)
    //        {
    //            // highlighting segment cells and number of segment (j) and cell (i)
    //            int i = 0;
    //            foreach (var cell in seg.ground.GetComponent<GroundProperties>().GetGroundCells())
    //            {
    //                float cellSize = seg.ground.GetComponent<GroundProperties>().GetGroundCells()[0].Size;

    //                Gizmos.color = Color.red;
    //                Gizmos.DrawWireCube(cell.Coordinates, new Vector3(cellSize, cellSize, cellSize));
    //                Tools.drawString($"{j},{i}", cell.Coordinates + 1.5f * Vector3.up, 0, 0, Color.red);
    //                i++;
    //            }
    //            j++;
    //        }
    //    }

    //    private void HighlightGroundandObstacles()
    //    {
    //        List<Segment.Segment> segments = this.GetComponent<LevelManager>().lvl1Spawner.segmentStack;
    //        float cellSize = segments[0].ground.GetComponent<GroundProperties>().GetGroundCells()[0].Size;

    //        if (segments != null)
    //        {
    //            // highlight thisStack[0] ground and items
    //            Vector3 groundPos = segments.segmentStack[0].ground.transform.position;

    //            Gizmos.color = Color.cyan;
    //            Gizmos.DrawLine(groundPos + Vector3.right, groundPos - Vector3.right);
    //            Gizmos.DrawLine(groundPos + Vector3.up, groundPos - Vector3.up);





    //            foreach (var seg in segments)
    //            {
    //                foreach (var obs in seg.obstacles)
    //                {
    //                    Vector3 itemPos = obs.transform.position;

    //                    Gizmos.color = Color.cyan;
    //                    Gizmos.DrawLine(itemPos + Vector3.right, itemPos - Vector3.right);
    //                    Gizmos.DrawLine(itemPos + Vector3.up, itemPos - Vector3.up);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            segments = this.GetComponent<LevelManager>().lvl1Spawner.segmentStack;
    //        }
    //    }
}
