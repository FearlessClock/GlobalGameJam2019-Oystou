using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePlayerPrefSettings : MonoBehaviour
{
    private void Start()
    {
        RemoveEverything();
    }
    public void RemoveEverything()
    {
        PlayerPrefs.DeleteAll();
    }
}
