using UnityEngine;
using System.Collections.Generic;

public enum MoveDirection
{
    None, Up, Down, Left, Right
}

public class PlayerMovement : MonoBehaviour
{
    private Camera camera;
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private Animator animator;
    private GameController gameController;

    // Mobile touch info
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    // Movement variables
    public float jumpForce = 400.0f;
    public float fallForce = 200.0f;
    public float leftDashForce = 200.0f;
    public float rightDashForce = 150.0f;
    private bool isGrounded = false;
    private bool canFall = false;
    private float currentSpeed;

    public void enableJump()
    {
        isGrounded = true;
        animator.SetBool("IsJumping", false);
    }

    public void enablePlayerAnimations()
    {
        animator.SetBool("GameStarted", true);
    }
    
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        collider = this.GetComponent<BoxCollider2D>();
        animator = this.GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
        camera = FindObjectOfType<Camera>();

        currentSpeed = gameController.moveSpeeed;
        enabled = false;
    }
    
    void Update()
    {
        MoveDirection moveDir = PlayerComputerControl();
        // MoveDirection moveDir = PlayerMobileControl();
        MoveByDirection(moveDir);
        MovePlayer();
    }

    private MoveDirection PlayerComputerControl()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return MoveDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            return MoveDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            return MoveDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            return MoveDirection.Right;
        }

        return MoveDirection.None;
    }

    private MoveDirection PlayerMobileControl()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
                //Debug.Log("Began: " + startTouchPos);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPos = touch.position;
                return GetMobileTouchDirection();
                //Debug.Log("Ended: " + endTouchPos + " , Diff: " + (endTouchPos - startTouchPos));
            }
        }

        return MoveDirection.None;
    }

    private MoveDirection GetMobileTouchDirection()
    {
        Vector2 diff = endTouchPos - startTouchPos;
        Vector2 absDiff = new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        bool xDiffBigger = absDiff.x > absDiff.y;

        if (xDiffBigger)
        {
            if (diff.x > 0)
            {
                return MoveDirection.Right;
            } else
            {
                return MoveDirection.Left;
            }
        } else
        {
            if (diff.y > 0)
            {
                return MoveDirection.Up;
            }
            else if (diff.y < 0)
            {
                return MoveDirection.Down;
            }
        }

        return MoveDirection.None;
    }

    private void MoveByDirection(MoveDirection dir)
    {
        switch (dir)
        {
            case MoveDirection.Up:
                if (isGrounded)
                {
                    Jump();
                }
                break;
            case MoveDirection.Down:
                if (canFall)
                {
                    Fall();
                }
                break;
            case MoveDirection.Left:
                LeftDast();
                break;
            case MoveDirection.Right:
                RightDash();
                break;
        }
    }

    private void MovePlayer()
    {
        float xPosition = transform.position.x + (currentSpeed * Time.deltaTime);
        float yPosition = transform.position.y;
        Vector2 newPosition = new Vector2(xPosition, yPosition);
        transform.position = newPosition;
    }

    private void Jump()
    {
        Vector2 force = (Vector2.up * jumpForce * rigidbody.mass);
        rigidbody.AddForce(force);
        isGrounded = false;
        canFall = true;
        animator.SetBool("IsJumping", true);
    }

    private void Fall()
    {
        Vector2 force = (Vector2.down * jumpForce * rigidbody.mass);
        rigidbody.AddForce(force);
        canFall = false;
    }

    private void LeftDast()
    {
        Vector2 force = (Vector2.left * leftDashForce * rigidbody.mass);
        rigidbody.AddForce(force);
    }

    private void RightDash()
    {
        Vector2 force = (Vector2.right * rightDashForce * rigidbody.mass);
        rigidbody.AddForce(force);
    }

    //private void Slide()
    //{
    //    //isSliding = true;
    //    SetBoxColliderSizeSlide();
    //    animator.SetBool("IsSliding", true);
    //}

    //private void Unslide()
    //{
    //    //isSliding = false;
    //    SetBoxColliderSizeNormal();
    //    animator.SetBool("IsSliding", false);
    //}

    //private void SetBoxColliderSizeSlide()
    //{
    //    collider.offset = new Vector2(collider.offset.x, 0.0f);
    //    collider.size = new Vector2(collider.size.x, 0.1f);
    //}

    //private void SetBoxColliderSizeNormal()
    //{
    //    collider.offset = new Vector2(collider.offset.x, -0.015f);
    //    collider.size = new Vector2(collider.size.x, 0.13f);
    //}
}
