using UnityEngine;
using Common;

namespace Controller
{
    public class StreetController : Object
    {
        // Controller for steet Spawning, Destroying and Moving
        private GameObject[] streets;
        private GameObject streetPrefab;
        private float streetLength;
        private int newestStreetIndex = 0;

        // Constructor
        public StreetController(GameObject[] streets, GameObject streetPrefab)
        {
            this.streets = streets;
            this.streetPrefab = streetPrefab;
            streetLength = Tools.GetSize(streetPrefab, 'z');
        }

        // Methods
        public void SpawnStreetIfNull()
        {
            // Instantiate new street segments
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

        
        public void DestroyStreetIfOld(int bufferStreetNumber = 2)
        {
            // Destroy old street segments
            float bufferLength = -bufferStreetNumber * streetLength;
            for (int i = 0; i < streets.Length; i++)
            {
                if (streets[i].transform.position.z < bufferLength)
                {
                    Destroy(streets[i]);
                }
            }
        }

        
        public void MoveStreets(float streetSpeed)
        {
            // Move street segments
            foreach (GameObject street in streets)
            {
                street?.transform.Translate(0, 0, -streetSpeed);
            }
        }
    }
}
