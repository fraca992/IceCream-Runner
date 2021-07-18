using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class ItemManager : Object
    {
        private List<GameObject> obstacles = new List<GameObject>();

        // Constructor
        public ItemManager()
        {
            // Loading all obstacle prefabs
            obstacles.AddRange(Resources.LoadAll<GameObject>("Obstacles"));
        }

        void SpawnObstacles(int budget, Vector3[] sidewalkCoords)
        {
            // buy obstacles using budget
            int buyIndex;
            List<GameObject> boughtObstacles = new List<GameObject>();

            while (budget > 0) // REVIEW: could be useful to put a counter of sorts to break the while if it takes too long to find a suitable item?
            {
                buyIndex = Random.Range(0, obstacles.Count);
                if (budget > obstacles[buyIndex].GetComponent<ItemProperties>().Cost)
                {
                    boughtObstacles.Add(obstacles[buyIndex]);
                    budget -= obstacles[buyIndex].GetComponent<ItemProperties>().Cost;
                }
            }
            
            // spawn obstacles
            while (boughtObstacles.Count > 0)
            {
                //choose a random cell
                int cellIndex = Random.Range(0, sidewalkCoords.Length); // TOFIX: Implement distributed probability for random cell picker

                // TODO: spawn item
                //if ()
                //{

                //}

                // TODO: update cell points

            }
        }
    } 
}
