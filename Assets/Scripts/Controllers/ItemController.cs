using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public delegate void ItemPickUp(GameObject pickedUpItem);
    public static event ItemPickUp OnItemPickedUp;

    public MemorableItem itemSettings;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnItemPickedUp?.Invoke(this.gameObject);
        }
    }
}
