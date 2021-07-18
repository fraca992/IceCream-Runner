using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class debugspawn : MonoBehaviour
{
    protected virtual void OnSceneGUI()
    {
        float size = 2f;
        float pickSize = size * 2f;


        if (Handles.Button(this.transform.position, Quaternion.identity, size, pickSize, Handles.RectangleHandleCap))
            this.GetComponentInParent<StreetProperties>().;
    }
}
