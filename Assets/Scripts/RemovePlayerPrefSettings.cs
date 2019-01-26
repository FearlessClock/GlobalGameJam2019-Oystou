using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePlayerPrefSettings : MonoBehaviour
{
    public void RemoveEverything()
    {
        PlayerPrefs.DeleteAll();
    }
}
