using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState { Moving, FallingBack, FoundObject, CarryItem}

public class PlayerController : MonoBehaviour
{
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

    bool isCarryingFoyer = false;

    // Player events
    public delegate void ItemMoveDelegate(GameObject item);
    public static event ItemMoveDelegate OnItemCarried;
    public static event ItemMoveDelegate OnItemDropped;
    public static event ItemMoveDelegate OnItemPlacedInFoyer;

    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();

        movementSpeed = normalSpeed;
        playerState = PlayerState.Moving;
        rb = GetComponent<Rigidbody2D>();

        FoyerPushCollisionController.OnPushTriggerEvent += OnNextToFoyer;
        ItemController.OnItemPickedUp += OnMemorableItemPickedUp;
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
                    foyer.GetComponent<Collider2D>().enabled = true;
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
        }
        rb.velocity = moveDirection * movementSpeed;
    }

    private Vector3 GetMoveDirection()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        return new Vector2(moveX, moveY);
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
                    foyer.GetComponent<Collider2D>().enabled = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Water"))
        {
            playerState = PlayerState.FallingBack;
            currentFallingBackTime = fallingBackTime;
            rb.velocity *= -0.25f;
        }
    }
}