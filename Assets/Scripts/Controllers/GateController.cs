using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public SwitchController connectedSwitch;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        connectedSwitch.OnSwitchActivated += OnSwitchActivated;
    }

    private void OnSwitchActivated(int ID)
    {
        animator.SetTrigger("Open");
    }
}
