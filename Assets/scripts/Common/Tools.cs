using UnityEngine;

namespace Common
{
    public static class Tools
    {
        public static float GetSize(GameObject obj, char direction)
        {
            //returns size along the chosen direction of a GameObject using its MeshRenderer
            Renderer objRenderer;
            float length;

            objRenderer = obj.GetComponentInChildren<MeshRenderer>();

            switch (direction)
            {
                case 'x':
                    length = objRenderer.bounds.size.x;
                    break;
                case 'y':
                    length = objRenderer.bounds.size.y;
                    break;
                case 'z':
                    length = objRenderer.bounds.size.z;
                    break;
                default:
                    length = 0;
                    break;
            }

            return length;
        }
    }
}
