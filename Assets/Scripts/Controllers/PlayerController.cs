﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerState { Moving, FallingBack, FoundObject, CarryItem}

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public GameObject foyer;

    public PlayerState playerState;

    public float movementSpeed;
    public float carrySpeed;
    public float normalSpeed;
    private Rigidbody2D rb;

    public float fallingBackTime;
    private float currentFallingBackTime;

    private bool isNextToFoyer;
    public float timeToPushFoyer;
    private float pushTimer = 0;
    private bool isCarryingItem;
    public float carryingDistance;

    private GameObject carriedItem;
    public Vector3 carryOffset;

    public bool isCarryingFoyer = false;

    public GameObject dust;
    public GameObject dustSpawnPos;
    public float timeBTWDustSpawn;
    private float currentTimeBTWDustSpawn;

    // Player events
    public delegate void ItemMoveDelegate(GameObject item);
    public static event ItemMoveDelegate OnItemCarried;
    public static event ItemMoveDelegate OnItemDropped;
    public static event ItemMoveDelegate OnItemPlacedInFoyer;

    public delegate void EnterFoyerDelegate();
    public static event EnterFoyerDelegate OnFoyerEnter;

    private Animator anim;
    public GameObject speechBubble;
    public GameObject bubbleSpawnPos;
    public bool hasSpeechBubble = false;

    public float maxTimeAwayFromHome;
    private float timerHomeSick = 0;
    public Image homesickFader;
    public float homesickSlowAmount;
    private float homesickSlowAmountInEffect = 1;
    public MemorableItem homesickHouse;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        currentTimeBTWDustSpawn = timeBTWDustSpawn;
        anim = gameObject.GetComponent<Animator>();

        movementSpeed = normalSpeed;
        playerState = PlayerState.Moving;
        rb = GetComponent<Rigidbody2D>();

        FoyerPushCollisionController.OnPushTriggerEvent += OnNextToFoyer;
        ItemController.OnItemPickedUp += OnMemorableItemPickedUp;

        if(SceneManager.GetActiveScene().name == "Main")
        {
            StartCoroutine("CountdownHomesick");
        }
    }
    bool showHouseBubble = false;
    public IEnumerator CountdownHomesick()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            timerHomeSick += Time.deltaTime;
            if(timerHomeSick > maxTimeAwayFromHome / 1.5)
            {
                if (!showHouseBubble)
                {
                    PopSpeechBubble(homesickHouse, 4f);
                }
                showHouseBubble = true;
            }
            if (timerHomeSick > maxTimeAwayFromHome)
            {
                timerHomeSick = maxTimeAwayFromHome;
                homesickSlowAmountInEffect = homesickSlowAmount;
            }
            homesickFader.material.SetFloat("_fadeScale", 1-(timerHomeSick / maxTimeAwayFromHome)*1.6f);
        }
    }

    private void OnDestroy()
    {
        FoyerPushCollisionController.OnPushTriggerEvent -= OnNextToFoyer;
        ItemController.OnItemPickedUp -= OnMemorableItemPickedUp;
    }

    private void OnNextToFoyer(bool entered)
    {
        isNextToFoyer = entered;
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }
        switch (playerState)
        {
            case PlayerState.Moving:
                PlayerMovingState();
                break;
            case PlayerState.FallingBack:
                FallBackState();
                break;
            case PlayerState.FoundObject:
                FoundObjectState();
                break;
            case PlayerState.CarryItem:
                CarryingItemState();
                break;
            default:
                break;
        }
    }

    private void SetStateTo(PlayerState nextState)
    {
        playerState = nextState;
    }

    private void CarryingItemState()
    {
        if (Input.GetButtonUp("Jump"))
        {
            if(!isCarryingFoyer && isNextToFoyer && isCarryingItem)
            {
                //Place the item in the house
                GameObject item = carriedItem;
                DropItem();
                OnItemPlacedInFoyer?.Invoke(item);
                SetStateTo(PlayerState.Moving);
                Destroy(item);
                EnterTheFoyer();
            }
            else if (isCarryingItem)
            {
                // TODO: Make this check if placement is valid
                if (isCarryingFoyer)
                {
                    foyer.GetComponent<Collider2D>().isTrigger = false;
                }
                DropItem();
                SetStateTo(PlayerState.Moving);
            }
        }

        Vector3 moveDirection = GetMoveDirection();
        TurnPlayer(moveDirection);
        MovePlayer(moveDirection);

        if (isCarryingItem)
        {
            CarryItem(moveDirection);
        }
    }


    private void OnMemorableItemPickedUp(GameObject pickedUpItem)
    {
        SetStateTo(PlayerState.CarryItem);
        PickUpItem(pickedUpItem);
    }

    private void DropItem()
    {
        isCarryingFoyer = false;   // No need to check, what ever we were carrying, right now we are not carrying anything
        isCarryingItem = false;
        anim.SetBool("IsCarrying", isCarryingItem);
        OnItemDropped?.Invoke(carriedItem);
        carriedItem = null;
        movementSpeed = normalSpeed;
    }

    private void PickUpItem(GameObject pickedUpItem)
    {
        carriedItem = pickedUpItem;
        isCarryingItem = true;
        anim.SetBool("IsCarrying", isCarryingItem);
        movementSpeed = carrySpeed;
        OnItemCarried?.Invoke(pickedUpItem);
    }

    public void PlayerMovingState()
    {
        if (Input.GetButtonDown("Jump"))
        {
           if (isNextToFoyer)
            {
                // Enter the foyer
                EnterTheFoyer();
            }
        }

        Vector3 moveDirection = GetMoveDirection();
        TurnPlayer(moveDirection);
        MovePlayer(moveDirection);

        if (!isCarryingItem)
        {
            CheckForFoyerCarry(moveDirection);
        }
    }

    private void MovePlayer(Vector3 moveDirection)
    {
        if(moveDirection.x == 0 && moveDirection.y == 0)
        {
            anim.SetBool("IsMoving", false);
        }
        else
        {
            anim.SetBool("IsMoving", true);
            SpawnDust();
        }
        rb.velocity = moveDirection * movementSpeed * homesickSlowAmountInEffect;
    }

    private Vector3 GetMoveDirection()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        return new Vector2(moveX, moveY);
    }

    private void SpawnDust()
    {
        currentTimeBTWDustSpawn -= Time.deltaTime;
        if (currentTimeBTWDustSpawn <= 0)
        {
            currentTimeBTWDustSpawn += timeBTWDustSpawn;
            Instantiate(dust, dustSpawnPos.transform.position, Quaternion.identity);
        }
    }

    private void TurnPlayer(Vector3 moveDirection)
    {
        Vector3 temp = transform.localScale;

        if (moveDirection.x > 0)
        {
            temp.x = Mathf.Abs(temp.x);
        }
        else if (moveDirection.x < 0)
        {
            temp.x = -Mathf.Abs(temp.x);
        }

        transform.localScale = temp;
    }

    private void EnterTheFoyer()
    {
        OnFoyerEnter?.Invoke();
        SceneManager.LoadScene("Foyer");
    }

    /// <summary>
    /// Place the item in front of the player when carrying the item
    /// </summary>
    /// <param name="dir"></param>
    private void CarryItem(Vector3 dir)
    {
        if(!(dir.x == 0 && dir.y == 0))
        {
            dir.y = 0;
            if(dir.x == 0)
            {
                dir.x = this.transform.localScale.x;
            }
            carriedItem.transform.position = this.transform.position + carryOffset + dir.normalized * carryingDistance;
        }
    }

    /// <summary>
    /// Check if the player is walking into the foyer and pick it up
    /// </summary>
    /// <param name="moveDirection"></param>
    private void CheckForFoyerCarry(Vector3 moveDirection)
    {
        if (isNextToFoyer)
        {
            if (!(moveDirection.x == 0 && moveDirection.y == 0))
            {
                // Get the direction to the foyer
                Vector3 dirToFoyer = (foyer.transform.position - this.transform.position).normalized;
                float isWalkingToFoyer = Vector3.Dot(dirToFoyer, moveDirection);
                if (isWalkingToFoyer < 0.5)
                {
                    pushTimer = 0;
                }

                pushTimer += Time.deltaTime;
                if (pushTimer > timeToPushFoyer)
                {
                    // Pick up the foyer and move to the carrying state

                    pushTimer = 0;
                    isCarryingFoyer = true;
                    foyer.GetComponent<Collider2D>().isTrigger = true;
                    PickUpItem(foyer);
                    SetStateTo(PlayerState.CarryItem);
                }
            }
            else
            {
                pushTimer = 0;
            }
        }
    }

    public void FallBackState()
    {
        currentFallingBackTime -= Time.deltaTime;

        if (currentFallingBackTime <= 0)
        {
            playerState = PlayerState.Moving;
        }
        else if (currentFallingBackTime <= fallingBackTime/2f)
        {
            rb.velocity *= 0;
        }
    }

    public void FoundObjectState()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        MemorableHolder memoHolder = collision.collider.GetComponent<MemorableHolder>();

        if (memoHolder) {
            MemorableItem item = memoHolder.memorableItem;
            if (collision.collider.CompareTag("Water"))
            {
                if (!MemorableItemManager.instance.ItemFound(item))
                {
                    playerState = PlayerState.FallingBack;
                    currentFallingBackTime = fallingBackTime;
                    rb.velocity *= -0.6f;

                    if (!hasSpeechBubble)
                    {
                        PopSpeechBubble(item, 2);
                    }
                }
            }
            else if(collision.collider.CompareTag("Mountain"))
            {
                if (!MemorableItemManager.instance.ItemFound(item))
                {
                    if (!hasSpeechBubble)
                    {
                        PopSpeechBubble(item, 2);
                    }
                }
            }
            else if (collision.collider.CompareTag("High grass"))
            {
                if (!MemorableItemManager.instance.ItemFound(item))
                {
                    if (!hasSpeechBubble)
                    {
                        PopSpeechBubble(item, 2);
                    }
                }
            }
        }
    }

    public void PopSpeechBubble(MemorableItem item, float time)
    {
        GameObject bubble = Instantiate(speechBubble, bubbleSpawnPos.transform.position, Quaternion.identity);
        bubble.transform.GetChild(0).GetComponent<SpeechBubbleController>().SetBubble(item);
        bubble.transform.GetChild(0).GetComponent<SpeechBubbleController>().destroyTime = time;
        Vector3 temp = bubble.transform.localScale;
        temp.x *= Mathf.Sign(this.transform.localScale.x);
        bubble.transform.localScale = temp;
        bubble.transform.parent = transform;
        hasSpeechBubble = true;
    }
}