using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondController : MonoBehaviour
{
    public ParticleSystem partSys;

    private void Start()
    {
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
        partSys.Stop();
    }

    private void OnItemCarried(GameObject item)
    {
        partSys.Play();
    }
}
