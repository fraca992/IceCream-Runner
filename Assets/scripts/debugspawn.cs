using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Manager;

public class debugspawn : MonoBehaviour
{
    public void OnButtonPress()
    {
        GameObject.Find("LevelManager").GetComponent<ItemManager>().Spawn = true;
    }
}
