using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorableItemManager : MonoBehaviour
{
    public List<GameObject> memorableItems;
    
    private void Start()
    {
        PlayerController.OnItemPlacedInFoyer += OnItemPlacedInFoyer;
    }

    private void OnDestroy()
    {
        PlayerController.OnItemPlacedInFoyer -= OnItemPlacedInFoyer;
    }

    private void OnItemPlacedInFoyer(GameObject item)
    {
        MakeItemFound(item);
    }

    public void MakeItemFound(GameObject item)
    {
        ItemController itemController = item.GetComponent<ItemController>();
        if (itemController)
        {
            PlayerPrefs.SetInt(PlayerPrefsStrings.itemId + itemController.itemSettings.ID, 1);
        }
    }
}
