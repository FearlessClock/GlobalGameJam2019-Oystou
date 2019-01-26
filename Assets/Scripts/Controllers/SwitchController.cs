using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public delegate void SwitchDelegate(int ID);
    public event SwitchDelegate OnSwitchActivated;
    public event SwitchDelegate OnSwitchItemFound;
    public int ID;
    private bool hasActivatedMechanisme = false;
    private bool hasItemFound = false;
    public MemorableItem neededItem;
    public float switchFoyerMaxDistance;

    public GameObject blinker;

    private int checkCounter = 0;
    private void Start()
    {
        hasItemFound = PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + neededItem.ID, 0) == 0? false: true;
        if (hasItemFound)
        {
            GameObject blink = Instantiate<GameObject>(blinker, this.transform);
            blink.GetComponent<SwitchLightController>().switchBlinker = this.GetComponent<SwitchBlinker>();
        }
        PlayerController.OnItemPlacedInFoyer += OnItemPlacedInFoyer;
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
                if (hit.CompareTag("Foyer"))
                {
                    OnSwitchActivated?.Invoke(ID);
                    
                    hasActivatedMechanisme = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, switchFoyerMaxDistance);
    }
}
