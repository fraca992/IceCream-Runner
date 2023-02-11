using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class describing an Item spawner. This object manages the lists of Items (obstacles, power-ups, etc.), selects new items once a new ground segment spawns
// and places them on the segment.
// HACK: for now the item placement is completely random. implement an algorithm to avoid path blocking and unbalanced positioning!
public class ItemSpawner : MonoBehaviour
{
    private string ItemPath;
    private List<GameObject> allItems = new List<GameObject>();
    private List<GameObject> selectedItems = new List<GameObject>();
    public List<ObstacleProperties> spawnedObstacles = new List<ObstacleProperties>();
    private int minCost;

    // Constructor
    public ItemSpawner(string path)
    {
        ItemPath = path;
        allItems.AddRange(Resources.LoadAll<GameObject>(ItemPath));

        //minCost of allItems is found to ensure we don't get stuck while selecting items if remaining budget is too low
        minCost = allItems[0].GetComponent<ObstacleProperties>().Cost;
        for (int i = 1; i < allItems.Count; i++)
        {
            int tempCost = allItems[i].GetComponent<ObstacleProperties>().Cost;
            if (tempCost < minCost)
            {
                minCost = tempCost;
            }
        }
    }

    // this function fills selectedItems with the Items that will be placed on the segment.
    public void FillItemList(int maxItems, int totBudget)
    {
        int budget = totBudget;

        selectedItems.Clear();

        // we add items to the list as long as we don't reach maxItems or the budget is too low to afford even the cheaper item
        while (selectedItems.Count < maxItems && budget >= minCost)
        {
            int rndIndex = UnityEngine.Random.Range(0, allItems.Count);
            bool canAfford = budget > allItems[rndIndex].GetComponent<ObstacleProperties>().Cost;

            if (canAfford)
            {
                selectedItems.Add(allItems[rndIndex]);
                budget -= allItems[rndIndex].GetComponent<ObstacleProperties>().Cost;
            }
        }
        return;
    }

    // this function places all the selected Items on the Ground segment
    public List<ObstacleProperties> PlaceObstacles(List<CellProperties> cells)
    {
        spawnedObstacles.Clear();


        int rndCellIndex;
        int rndItemIndex;

        // we place items until we have 0 items and/or no space
        while (selectedItems.Count > 0 && cells.Count > 0)
        {
            rndCellIndex = UnityEngine.Random.Range(0, cells.Count);
            rndItemIndex = UnityEngine.Random.Range(0, selectedItems.Count);

            GameObject itm = selectedItems[rndItemIndex];
            CellProperties cl = cells[rndCellIndex];

            // TODO: Must eventually account for different size/shape of items
            if (cl.isOccupied == false)
            {
                spawnedObstacles.Add(Instantiate(itm, cl.Coordinates, Quaternion.identity).GetComponent<ObstacleProperties>());
                cells.RemoveAt(rndCellIndex);
                selectedItems.RemoveAt(rndItemIndex);
            }
        }
        return spawnedObstacles;
    }
}
