using UnityEngine;

namespace Common
{
    public static class Tools
    {
        public class StreetManager : Object
        {
            // Manages steet Spawning, Destroying and Moving
            // Attributes
            private GameObject[] streets;
            private GameObject streetPrefab;
            private float streetLength;

            // Constructor
            public StreetManager(GameObject[] streets, GameObject streetPrefab)
            {
                this.streets = streets;
                this.streetPrefab = streetPrefab;
                streetLength = GetSize(streetPrefab,'z');
            }

            // Instantiate new street segments
            private int newestStreetIndex = 0; // TODO: farthestStreetIndex might need to be static if i plan to instantiate a new StreetManager for new kind of streets
            public void SpawnStreetIfNull()
            {
                Vector3 nextStreetPosition = new Vector3();

                for (int i = 0; i < streets.Length; i++)
                {
                    if (streets[i] == null)
                    {
                        nextStreetPosition.z = streets[newestStreetIndex].transform.position.z + streetLength;
                        streets[i] = Instantiate(streetPrefab, nextStreetPosition, Quaternion.identity);
                        newestStreetIndex = i;
                    }
                }
            }

            // Destroy old street segments
            public void DestroyStreetIfOld(int bufferStreetNumber = 2)
            {
                float bufferLength = -bufferStreetNumber * streetLength;
                for (int i = 0; i < streets.Length; i++)
                {
                    if (streets[i].transform.position.z < bufferLength)
                    {
                        Destroy(streets[i]);
                    }
                }
            }

            // Move street segments
            public void MoveStreets(float streetSpeed)
            {
                foreach (GameObject street in streets)
                {
                    street?.transform.Translate(0, 0, -streetSpeed);
                }
            }
        }

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
