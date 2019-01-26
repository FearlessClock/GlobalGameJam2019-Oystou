using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseItemPlacement : MonoBehaviour
{
    public Transform[] itemPlacement;
    private MemorableItemManager memorableItemManager;
    private void Awake()
    {
        memorableItemManager = MemorableItemManager.instance;
        foreach (GameObject itemObj in memorableItemManager.memorableItems)
        {
            MemorableItem item = itemObj.GetComponent<ItemController>().itemSettings;
            bool hasFound = PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + item.ID, 0) == 1 ? true: false;
            if (hasFound)
            {
                GameObject itemObjIns = Instantiate<GameObject>(item.worldItem, itemPlacement[item.ID].position, Quaternion.identity);
            }
        }
    }
}
