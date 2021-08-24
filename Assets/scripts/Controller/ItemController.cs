using System.Collections.Generic;
using UnityEngine;
using Common;
using Properties;

namespace Controller
{
    public class ItemController : Object
    {
        private List<GameObject> items = new List<GameObject>();

        // Constructor
        public ItemController(string itmType)
        {
            // Loading all items from Resources in itmType folder
            items.AddRange(Resources.LoadAll<GameObject>(itmType));
        }

        // Methods
        public void SpawnItems(int budget, List<Street.CellProperties> cells)
        {
            List<GameObject> boughtItems = BuyItems(ref budget);

            // spawn items
            while (boughtItems.Count > 0)
            {
                // choose a random cell
                int cellIndex = Random.Range(0, cells.Count); // TOFIX: Implement distributed probability for random cell picker
                Street.CellProperties chosenCell = cells[cellIndex];

                // spawn item, remove it from the list and remove the cell from cells list
                Instantiate(boughtItems[0], chosenCell.Coordinates, Quaternion.identity);
                boughtItems.RemoveAt(0);
                cells.RemoveAt(cellIndex);

                // TODO: update cell points of neighbouring cells, remember to differentiate between 2 sidewalks!
            }
        }

        // Helper Methods
        private List<GameObject> BuyItems(ref int budget)
        {
            // buy items using budget
            int watchDog = 0;
            int buyIndex;
            List<GameObject> boughtItems = new List<GameObject>();

            while (budget > 0 & watchDog < 10)
            {
                buyIndex = Random.Range(0, items.Count);
                items[buyIndex].GetComponent<ItemProperties>().InitializeItemProperties();
                int itemCost = items[buyIndex].GetComponent<ItemProperties>().CostValue;
                if (budget >= itemCost)
                {
                    boughtItems.Add(items[buyIndex]);
                    budget -= itemCost;
                    watchDog = 0;
                }
                else watchDog++;
            }

            return boughtItems;
        }
    } 
}
