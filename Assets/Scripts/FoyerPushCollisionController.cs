using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoyerPushCollisionController : MonoBehaviour
{
    public delegate void PushTriggerEvent(bool entered);
    public static event PushTriggerEvent OnPushTriggerEvent;

    private bool isCarried = false;

    //private void Start()
    //{
    //    PlayerController.OnItemCarried += OnSelfCarried;
    //    PlayerController.OnItemDropped += OnSelfDropped;
    //}

    //private void OnDestroy()
    //{
    //    PlayerController.OnItemCarried -= OnSelfCarried;
    //    PlayerController.OnItemDropped -= OnSelfDropped;
    //}

    //private void OnSelfCarried(GameObject item)
    //{
    //    if (item.CompareTag(this.tag))
    //    {
    //        isCarried = true;
    //        OnPushTriggerEvent?.Invoke(false);
    //    }
    //}
    //private void OnSelfDropped(GameObject item)
    //{
    //    if (item.CompareTag(this.tag))
    //    {
    //        isCarried = false;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCarried && collision.CompareTag("Player"))
        {
            OnPushTriggerEvent?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isCarried && collision.CompareTag("Player"))
        {
            OnPushTriggerEvent?.Invoke(false);
        }
    }
}
