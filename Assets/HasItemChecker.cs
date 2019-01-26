using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasItemChecker : MonoBehaviour
{
    public MemorableItem memorableItem;

    private void Start()
    {
        if(PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + memorableItem.ID, 0) == 1)
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
