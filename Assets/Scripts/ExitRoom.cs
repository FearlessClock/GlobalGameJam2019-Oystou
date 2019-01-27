using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoom : MonoBehaviour
{
    public delegate void ExitRoomTrigger();
    public static event ExitRoomTrigger OnRoomExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnRoomExit?.Invoke();
        }
    }
}
