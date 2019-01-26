using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLightController : MonoBehaviour
{
    public SwitchBlinker switchBlinker;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        switchBlinker.OnBlink += OnBlink;
        PlayerController.OnItemCarried += OnItemCarried;
        PlayerController.OnItemDropped += OnItemDropped;
    }

    private void OnDestroy()
    {
        PlayerController.OnItemCarried -= OnItemCarried;
        PlayerController.OnItemDropped -= OnItemDropped;
    }

    private void OnItemDropped(GameObject item)
    {
        if (item.CompareTag("Foyer"))
        {
            animator.SetTrigger("Blink");
        }
    }

    private void OnItemCarried(GameObject item)
    {
        if (item.CompareTag("Foyer"))
        {
            animator.SetTrigger("Blink");
        }
    }

    private void OnBlink(bool isOn, float speed)
    {
        animator.SetFloat("BlinkSpeed", switchBlinker.slowestBlinkTime - speed +1); // Inverse the timer time to get a multiplier
    }
}
