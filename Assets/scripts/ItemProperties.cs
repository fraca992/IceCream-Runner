using UnityEngine;

public class ItemProperties : MonoBehaviour
{
    // We use private variables so we can expose them in the Editor
    [SerializeField]
    private int cost = 1;
    [SerializeField]
    private int costDistance = 1;

    public int Cost { get; private set; }
    public int CostDistance { get; private set; }

    private void Awake()
    {
        Cost = cost;
        CostDistance = costDistance;
    }
}
