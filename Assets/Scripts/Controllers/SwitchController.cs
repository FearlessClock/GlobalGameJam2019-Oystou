using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public delegate void SwitchDelegate(int ID);
    public event SwitchDelegate OnSwitchActivated;
    public event SwitchDelegate OnSwitchItemFound;
    public delegate void SwitchCloseDelegate(int ID, bool enter);
    public event SwitchCloseDelegate OnCloseToSwitch;
    public int ID;
    private bool hasActivatedMechanisme = false;
    private bool hasItemFound = false;
    public MemorableItem neededItem;
    public float switchFoyerMaxDistance;

    public GameObject foyer;
    private PlayerController playerController;
    public GameObject blinker;
    public GameObject powerCable;

    public CircleCollider2D powerCableActivator;

    private int checkCounter = 0;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        hasItemFound = PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + neededItem.ID, 0) == 0? false: true;
        if (hasItemFound)
        {
            GameObject blink = Instantiate<GameObject>(blinker, this.transform);
            blink.GetComponent<SwitchLightController>().switchBlinker = this.GetComponent<SwitchBlinker>();

            GameObject powerCableIns = Instantiate<GameObject>(powerCable);
            ParticleLinePlacer particleLinePlacer = powerCableIns.GetComponent<ParticleLinePlacer>();
            particleLinePlacer.activeSwitch = this;
            particleLinePlacer.point1 = this.transform;
            particleLinePlacer.point2 = foyer.transform;
            PlayerController.OnItemPlacedInFoyer += OnItemPlacedInFoyer;
            Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, powerCableActivator.radius);
            if (hits != null && hits.Length > 0)
            {
                foreach (Collider2D hit in hits)
                {
                    if (hit.CompareTag("Foyer"))
                    {
                        particleLinePlacer.parts.Play();
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        PlayerController.OnItemPlacedInFoyer -= OnItemPlacedInFoyer;
    }

    private void OnItemPlacedInFoyer(GameObject item)
    {
        MemorableItem placedItem = item.GetComponent<ItemController>().itemSettings;
        if (placedItem && placedItem.ID == neededItem.ID)
        {
            hasItemFound = PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + neededItem.ID, 0) == 0 ? false : true;
            OnSwitchItemFound?.Invoke(ID);
        }
    }

    public void ActivateSwitch()
    {
        OnSwitchActivated?.Invoke(ID);
    }

    private void Update()
    {
        if(hasActivatedMechanisme)
        {
            return;         //------- I can also remove the component, we will see
        }
        checkCounter++;
        if (checkCounter > 20)
        {
            checkCounter = 0;
            Collider2D[] overlapHits = Physics2D.OverlapCircleAll(this.transform.position, switchFoyerMaxDistance);
            foreach (Collider2D hit in overlapHits)
            {
                if (hit.CompareTag("Foyer") && !playerController.isCarryingFoyer)
                {
                    ActivateSwitch();

                    hasActivatedMechanisme = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Foyer"))
        {
            OnCloseToSwitch?.Invoke(ID, true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Foyer"))
        {
            OnCloseToSwitch?.Invoke(ID, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Foyer"))
        {
            OnCloseToSwitch?.Invoke(ID, false);
        }
    }
}
