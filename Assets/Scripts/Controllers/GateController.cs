﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public SwitchController connectedSwitch;
    private Animator animator;
    public bool isOpen;
    void Start()
    {
        animator = GetComponent<Animator>();
        connectedSwitch.OnSwitchActivated += OnSwitchActivated;
    }

    private void OnDestroy()
    {
        connectedSwitch.OnSwitchActivated -= OnSwitchActivated;
    }

    private void OnSwitchActivated(int ID)
    {
        OpenDoor();
    }

    public void OpenDoor()
    {
        if(animator != null)
        {
            animator.SetTrigger("Open");
            isOpen = true;
        }
    }
}
