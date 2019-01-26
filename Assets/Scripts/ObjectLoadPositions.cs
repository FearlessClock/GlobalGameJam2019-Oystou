using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoadPositions : MonoBehaviour
{
    public MemorableItemManager memorableItemManager;

    public Transform playerPosition;
    public Transform foyerPosition;

    private void Awake()
    {
        foreach (GameObject item in memorableItemManager.memorableItems)
        {
            ItemController itemController = item.GetComponent<ItemController>();
            if (itemController)
            {
                string ppString = PlayerPrefsStrings.savePosition + itemController.itemSettings.ID;
                if (PlayerPrefs.HasKey(ppString))
                {
                    SavePositionInformation position = JsonUtility.FromJson<SavePositionInformation>(PlayerPrefs.GetString(ppString, "{x:0, y:0, active: true}"));
                    if (position.active)
                    {
                        item.transform.position = new Vector3(position.x, position.y);
                    }
                    else
                    {
                        memorableItemManager.memorableItems.Remove(item);   // Remove items that are not used anymore when they aren't active to save calculation time
                    }
                }
            }
            else
            {
                Debug.LogError("Wrong item in memorableItem list: " + item.name);
            }
            if (PlayerPrefs.HasKey(PlayerPrefsStrings.playerPosition))
            {
                SavePositionInformation playerInformation = JsonUtility.FromJson<SavePositionInformation>(PlayerPrefs.GetString(PlayerPrefsStrings.playerPosition));
                playerPosition.position = new Vector3(playerInformation.x, playerInformation.y);
            }
            if (PlayerPrefs.HasKey(PlayerPrefsStrings.foyerPosition))
            {
                SavePositionInformation foyerInformation = JsonUtility.FromJson<SavePositionInformation>(PlayerPrefs.GetString(PlayerPrefsStrings.foyerPosition));
                foyerPosition.position = new Vector3(foyerInformation.x, foyerInformation.y);
            }
        }
    }
}
