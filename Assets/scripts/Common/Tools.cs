using UnityEngine;

namespace Common
{
    public static class Tools
    {
        public static float GetSize(GameObject obj, char direction, char useColliderorRenderer = 'r')
        {
            //returns size along the chosen direction of a GameObject using its MeshRenderer or Box collider
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

        static int counterValue = 0;
        public static int GetNextValue()
        {
            return counterValue++;
        }
    }
}
