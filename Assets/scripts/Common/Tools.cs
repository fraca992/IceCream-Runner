using UnityEngine;

namespace Common
{
    public static class Tools
    {
        // Gets size of a GameObject along the chosen direction of a GameObject using its MeshRenderer or Box collider
        public static float GetSize(GameObject obj, char direction, char useColliderorRenderer = 'r')
        {
            BoxCollider objCollider;
            MeshRenderer objRenderer;

            float lengthC, lengthR;
            useColliderorRenderer = char.ToLower(useColliderorRenderer);

            objCollider = obj.GetComponentInChildren<BoxCollider>();
            objRenderer = obj.GetComponentInChildren<MeshRenderer>();


            switch (direction)
            {
                case 'x':
                    lengthC = objCollider.bounds.size.x;
                    lengthR = objRenderer.bounds.size.x;
                    break;
                case 'y':
                    lengthC = objCollider.bounds.size.y;
                    lengthR = objRenderer.bounds.size.y;
                    break;
                case 'z':
                    lengthC = objCollider.bounds.size.z;
                    lengthR = objRenderer.bounds.size.z;
                    break;
                default:
                    lengthC = 0;
                    lengthR = 0;
                    break;
            }

            switch (useColliderorRenderer)
            {
                case 'c':
                    return lengthC;
                case 'r':
                    return lengthR;
                default:
                    return 0f;
            }
        }

        // Static counter for Street ID
        static int counterValue = 0;
        public static int GetNextValue()
        {
            return counterValue++;
        }

        // Methods for drawing text gizoms
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
}
