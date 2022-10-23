//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using Common;

public class ObstacleProperties : MonoBehaviour
{
    [SerializeField]
    private int _cost;
    public int Cost { get { return _cost; } }

    [SerializeField]
    private int[] _size = new int[2]; // contains the X and Z values of the boundary 2D box that contains the obstacle. used to avoid spawning and closed paths issues
    public int[] Size { get { return _size; } }

    [SerializeField]
    private bool _isEditing = false;


    //TODO: create and store different effects




    // For correct sizing in editor, enabled by setting isEditing
    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Vector3 gizPosition;
        Vector3 gizSize;

        if (_isEditing == true)
        {
            gizSize = new Vector3(Size[0], 0, Size[1]);
            Gizmos.color = Color.yellow;

            float minX, maxX;
            float minZ, maxZ;

            minX = this.GetComponent<MeshRenderer>().bounds.min.x;
            maxX = this.GetComponent<MeshRenderer>().bounds.max.x;
            minZ = this.GetComponent<MeshRenderer>().bounds.min.z;
            maxZ = this.GetComponent<MeshRenderer>().bounds.max.z;

            for (int i = 0; i < this.transform.childCount; i++)
            {
                Bounds childBounds = this.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().bounds;
                if (childBounds.min.x < minX) minX = childBounds.min.x;
                if (childBounds.max.x > maxX) maxX = childBounds.max.x;
                if (childBounds.min.z < minZ) minZ = childBounds.min.z;
                if (childBounds.max.z > maxZ) maxZ = childBounds.max.z;
            }

            float centerX = (maxX + minX) /2;
            float centerZ = (maxZ + minZ) / 2;
            gizPosition = new Vector3(centerX, 0, centerZ);

            Gizmos.DrawWireCube(gizPosition, gizSize);
        }
    }
}
    