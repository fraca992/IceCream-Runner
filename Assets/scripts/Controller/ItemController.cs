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
            int buyIndex;
            List<GameObject> boughtItems = new List<GameObject>();

            while (budget > 0) // REVIEW: could be useful to put a counter of sorts to break the while if it takes too long to find a suitable item?
            {
                buyIndex = Random.Range(0, items.Count);
                int itemCost = items[buyIndex].GetComponent<ItemProperties>().CostValue;
                if (budget > itemCost)
                {
                    boughtItems.Add(items[buyIndex]);
                    budget -= itemCost;
                }
            }

            return boughtItems;
        }
    } 
}
