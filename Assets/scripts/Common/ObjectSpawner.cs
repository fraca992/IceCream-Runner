using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class ObjectSpawner : Object
    {
        private GameObject[] obstacles;

        public ObjectSpawner()
        {
            // Loading all obstacle prefabs
            obstacles = Resources.LoadAll<GameObject>("Obstacles");
        }

        void SpawnObstacles(int budget, Vector3[] sidewalkCoords)
        {
            // TODO: buy obstacles using budget
            int buyIndex;
            List<GameObject> boughtItems = new List<GameObject>();

            while (budget > 0)
            {
                buyIndex = Random.Range(0, obstacles.Length);
                boughtItems.Add(obstacles[buyIndex]);
                //budget -= obstacles[buyIndex].GetComponent<>();
            }
            
            // TODO: refctor using term ITEM as a way fo generalizing

            // TODO: choose a random obstacle
            
            // TODO: choose a random cell using distributed probability

            // TODO: update cell points

        }
    } 
}
