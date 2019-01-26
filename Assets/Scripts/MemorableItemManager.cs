using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorableItemManager : MonoBehaviour
{
    public static MemorableItemManager instance;
    public List<GameObject> memorableItems;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

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

    public bool HasItem(MemorableItem item)
    {
        bool hasitem = PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + item.ID) == 0 ? false : true;
        return hasitem;
    }
}
