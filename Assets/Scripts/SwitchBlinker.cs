using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlinker : MonoBehaviour
{
    SwitchController switchController;
    PlayerController player;
    private bool isCarried = false;
    public float maxBlinkDistanceSqr;
    public float slowestBlinkTime;
    public float fastestBlinkTime;
    private bool blinkState = false;
    private float timer = 0;

    public delegate void SwitchBlinkDelegate(bool isOn, float speed);
    public event SwitchBlinkDelegate OnBlink;

    private void Start()
    {
        switchController = GetComponent<SwitchController>();
        player = FindObjectOfType<PlayerController>();
        if(player == null)
        {
            Debug.LogError("Can't find the player object");
        }
        PlayerController.OnItemCarried += OnItemCarried;
        PlayerController.OnItemDropped += OnItemDropped;
    }

    private void OnItemDropped(GameObject item)
    {
        isCarried = false;
        timer = 0;
    }

    private void OnItemCarried(GameObject item)
    {
        if (item.CompareTag("Foyer"))
        {
            isCarried = true;
        }
    }

    private void Update()
    {
        if (isCarried)
        {
            float distanceToFoyerSqr = (this.transform.position - player.transform.position).sqrMagnitude;
            float blinkTime = 0;
            if(distanceToFoyerSqr > maxBlinkDistanceSqr)
            {
                blinkTime = slowestBlinkTime;
            }
            else
            {
                blinkTime = (distanceToFoyerSqr / maxBlinkDistanceSqr) * slowestBlinkTime;
                if(blinkTime < fastestBlinkTime)
                {
                    blinkTime = fastestBlinkTime;
                }
            }

            timer += Time.deltaTime;
            if(timer > blinkTime)
            {
                timer = 0;
                blinkState = !blinkState;
            }
            OnBlink?.Invoke(blinkState, blinkTime);
        }
    }
}
