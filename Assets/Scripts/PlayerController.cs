using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Moving, FallingBack, FoundObject}

public class PlayerController : MonoBehaviour
{
    public PlayerState playerState;

    public float speed;      
    public Rigidbody2D rb;
    

    void Start()
    {
        playerState = PlayerState.Moving;
        rb = GetComponent<Rigidbody2D>();
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
                break;
            default:
                break;
        }
    }

    public void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

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
        rb.velocity = new Vector2(moveX, moveY) * speed;
        Debug.Log("" + rb.velocity.x);
    }

    public void FallBack()
    {

    }

    public void UpdateState()
    {

    }
}