using UnityEngine;

namespace Properties
{
    public class ItemProperties : MonoBehaviour
    {
        // We use private variables so we can expose them in the Editor
        [SerializeField]
        private int costValue = 1;
        [SerializeField]
        private int costDistance = 1;

        public int CostValue { get; private set; }
        public int CostDistance { get; private set; }

        public void InitializeItemProperties()
        {
            CostValue = costValue;
            CostDistance = costDistance;
        }
    }

}