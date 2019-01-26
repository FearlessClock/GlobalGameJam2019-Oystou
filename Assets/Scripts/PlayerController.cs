using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState { Moving, FallingBack, FoundObject}

public class PlayerController : MonoBehaviour
{
    public GameObject foyer;

    public PlayerState playerState;

    public float speed;
    private Rigidbody2D rb;

    public float fallingBackTime;
    private float currentFallingBackTime;

    private bool isNextToFoyer;
    public float timeToPushFoyer;
    private float pushTimer = 0;
    private bool isCarryingItem;
    public int carryingDistance;

    private GameObject carriedItem;
    

    void Start()
    {
        playerState = PlayerState.Moving;
        rb = GetComponent<Rigidbody2D>();

        FoyerPushCollisionController.OnPushTriggerEvent += OnNextToFoyer;
    }

    private void OnDestroy()
    {
        FoyerPushCollisionController.OnPushTriggerEvent -= OnNextToFoyer;
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
                MovePlayer();
                break;
            case PlayerState.FallingBack:
                FallBack();
                break;
            case PlayerState.FoundObject:
                FoundObject();
                break;
            default:
                break;
        }
    }

    public void MovePlayer()
    {
        if (Input.GetButtonUp("Jump"))
        {
            Debug.Log("Jump button pressed");
            if (isCarryingItem)
            {
                // TODO: Make this check if placement is valid
                isCarryingItem = false;
                carriedItem.transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
                carriedItem = null;
            }
            else if(isNextToFoyer)
            {
                // Enter the foyer
                EnterTheFoyer();
            }
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector2(moveX, moveY);
        Vector3 temp = transform.localScale;

        if(moveX > 0)
        {
            temp.x = Mathf.Abs(temp.x);
        }
        else if(moveX < 0)
        {
            temp.x = -Mathf.Abs(temp.x);
        }

        transform.localScale = temp;
        rb.velocity = moveDirection * speed;
        if (!isCarryingItem)
        {
            CheckForFoyerCarry(moveDirection);
        }

        if (isCarryingItem)
        {
            CarryItem(moveDirection);
        }
    }

    private void EnterTheFoyer()
    {
        SceneManager.LoadScene("Foyer");
    }

    private void CarryItem(Vector3 dir)
    {
        if(!(dir.x == 0 && dir.y == 0))
        {
            carriedItem.transform.position = this.transform.position + dir.normalized * carryingDistance;
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
                    isCarryingItem = true;
                    carriedItem = foyer;
                    foyer.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
                }
            }
            else
            {
                pushTimer = 0;
            }
        }
    }

    public void FallBack()
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

    public void FoundObject()
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