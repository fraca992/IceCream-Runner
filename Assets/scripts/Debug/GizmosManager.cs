using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosManager : MonoBehaviour
{
    public bool isDebugging = false;

    private void OnDrawGizmos()
    {
        if (!isDebugging) return;


        LevelManager.GroundStack thisStack = this.GetComponent<LevelManager>().groundStack;
        
        if (thisStack != null)
        {
            foreach (var strt in thisStack.groundList)
            {
                int i = 0;
                foreach (var cell in strt.GetComponent<GroundProperties>().GetGroundCells())
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(cell.Coordinates, new Vector3(5,5,5));
                    drawString(i.ToString(), cell.Coordinates + 1.5f * Vector3.up,0,0,Color.red);
                    i++;
                }
            }
        }
        else
        {
            thisStack = this.GetComponent<LevelManager>().groundStack;
        }
    }

    static public void drawString(string text, Vector3 worldPos, float oX = 0, float oY = 0, Color? colour = null)
    {

#if UNITY_EDITOR
        UnityEditor.Handles.BeginGUI();

        var restoreColor = GUI.color;

        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

        if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
        {
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
            return;
        }

        UnityEditor.Handles.Label(TransformByPixel(worldPos, oX, oY), text);

        GUI.color = restoreColor;
        UnityEditor.Handles.EndGUI();
#endif
    }

    static Vector3 TransformByPixel(Vector3 position, float x, float y)
    {
        return TransformByPixel(position, new Vector3(x, y));
    }

    static Vector3 TransformByPixel(Vector3 position, Vector3 translateBy)
    {
        Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
        if (cam)
            return cam.ScreenToWorldPoint(cam.WorldToScreenPoint(position) + translateBy);
        else
            return position;
    }

}
