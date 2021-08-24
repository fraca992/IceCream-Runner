using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class SpawnButton : MonoBehaviour
{
    [Range(0,4)]
    public int streetnum;

    public void OnButtonPress()
    {
        FindObjectOfType<LevelManager>().num = streetnum;
        FindObjectOfType<LevelManager>().dbSpawn = true;        
    }
}
