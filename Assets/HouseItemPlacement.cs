using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseItemPlacement : MonoBehaviour
{
    public Transform[] itemPlacement;
    public MemorableItem[] items;
    private void Awake()
    {
        foreach (MemorableItem item in items)
        {
            bool hasFound = PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + item.ID, 0) == 1 ? true: false;
            if (hasFound)
            {
                GameObject itemObj = Instantiate<GameObject>(item.worldItem, itemPlacement[item.ID].position, Quaternion.identity);
            }
        }
    }
}
