﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoadPositions : MonoBehaviour
{
    private MemorableItemManager memorableItemManager;

    public GameObject[] gates;

    private Transform playerPosition;
    private Transform foyerPosition;

    private void Awake()
    {
        memorableItemManager = FindObjectOfType<MemorableItemManager>();
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
                    SavePositionInformation position = JsonUtility.FromJson<SavePositionInformation>(PlayerPrefs.GetString(ppString, "{x:0, y:0, active: true}"));
                    if (PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + itemController.itemSettings.ID, 0) == 0)
                    {
                        item.transform.position = new Vector3(position.x, position.y);
                    }
                    else
                    {
                        Destroy(item);
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

            for (int i = 0; i < gates.Length; i++)
            {
                if (PlayerPrefs.GetInt(PlayerPrefsStrings.gatePosition + i, 0) == 1)
                {
                    gates[i].GetComponent<GateController>().OpenDoor();
                }
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
                PlayerPrefs.SetString(PlayerPrefsStrings.itemPosition + item.GetComponent<ItemController>().itemSettings.ID, JsonUtility.ToJson(savePositionInformation));
            }
        }

        for (int i = 0; i < gates.Length; i++)
        {
            if(gates[i].GetComponent<GateController>().isOpen)
            {
                PlayerPrefs.SetInt(PlayerPrefsStrings.gatePosition + i, 1);
            }
        }
    }
}
