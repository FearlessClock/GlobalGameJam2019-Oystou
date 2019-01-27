using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoyerPushCollisionController : MonoBehaviour
{
    public delegate void PushTriggerEvent(bool entered);
    public static event PushTriggerEvent OnPushTriggerEvent;

    private bool isCarried = false;

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
