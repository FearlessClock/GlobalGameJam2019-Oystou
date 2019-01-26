using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoadPositions : MonoBehaviour
{
    public MemorableItemManager memorableItemManager;

    private Transform playerPosition;
    private Transform foyerPosition;

    private void Awake()
    {
        playerPosition = FindObjectOfType<PlayerController>().transform;
        foyerPosition = GameObject.FindGameObjectWithTag("Foyer").transform;
        foreach (GameObject item in memorableItemManager.memorableItems)
        {
            ItemController itemController = item.GetComponent<ItemController>();
            if (itemController)
            {
                string ppString = PlayerPrefsStrings.itemPosition + itemController.itemSettings.ID;
                if (PlayerPrefs.HasKey(ppString))
                {
                    Debug.Log("Has string");
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

    private void Start()
    {
        PlayerController.OnFoyerEnter += OnFoyerEnter;
    }

    private void OnDestroy()
    {
        PlayerController.OnFoyerEnter -= OnFoyerEnter;
    }

    private void OnFoyerEnter()
    {
        SaveObjects();
    }

    public void SaveObjects()
    {
        // Save the foyer data
        SavePositionInformation savePositionInformation = new SavePositionInformation();
        savePositionInformation.x = foyerPosition.position.x;
        savePositionInformation.y = foyerPosition.position.y;
        savePositionInformation.active = true;
        PlayerPrefs.SetString(PlayerPrefsStrings.foyerPosition, JsonUtility.ToJson(savePositionInformation));

        //Save the player data
        savePositionInformation = new SavePositionInformation();
        savePositionInformation.x = playerPosition.position.x;
        savePositionInformation.y = playerPosition.position.y;
        savePositionInformation.active = true;
        PlayerPrefs.SetString(PlayerPrefsStrings.playerPosition, JsonUtility.ToJson(savePositionInformation));

        foreach (GameObject item in memorableItemManager.memorableItems)
        {
            savePositionInformation = new SavePositionInformation();
            if(item != null)
            {
                savePositionInformation.x = item.transform.position.x;
                savePositionInformation.y = item.transform.position.y;
                savePositionInformation.active = true;
            }
            else
            {
                savePositionInformation.x = 0;
                savePositionInformation.y = 0;
                savePositionInformation.active = false;
            }
            PlayerPrefs.SetString(PlayerPrefsStrings.itemPosition + item.GetComponent<ItemController>().itemSettings.ID, JsonUtility.ToJson(savePositionInformation));
        }
    }
}
