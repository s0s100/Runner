using UnityEngine;
using System.Collections.Generic;

public enum MoveDirection
{
    None, Up, Down, Left, Right
}

public class PlayerMovement : MonoBehaviour
{
    private const float DEFAULT_GRAVITY_SCALE = 1.0f;

    private Camera camera;
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private Animator animator;
    private GameController gameController;

    // Mobile touch info
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    // X-Movement variables
    public float dashSpeed = 100.0f;
    public float dashCooldown = 1.0f;
    private float curDashCooldown = 0.0f;

    private float moveSpeed = 0.0f;

    private bool isRightDash = false;
    private bool canDash = false;

    // Y-Movement variables
    public float jumpForce = 400.0f;
    public float fallForce = 200.0f;

    private bool isGrounded = false;
    private bool canFall = false;

    public void EnableJump()
    {
        canDash = true;
        isGrounded = true;
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsJumping", false);
    }

    public void DisableJump()
    {
        isGrounded = false;
        canFall = true;
        animator.SetBool("IsJumping", true);
    }

    public void EnablePlayerAnimations()
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

        moveSpeed = gameController.moveSpeeed;
        enabled = false;
    }
    
    void Update()
    {
        //MoveDirection moveDir = PlayerComputerControl();
        //MoveDirection moveDir = PlayerMobileControl();

        MoveDirection moveDir = PlayerComputerControl();
        if (moveDir == MoveDirection.None)
        {
            moveDir = PlayerMobileControl();
        }
        
        MoveByDirection(moveDir);
        MovePlayer();
        DashPlayer();
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
                /*
                    public float leftDashDuration = 1.0f; // Shows the length of the dash in seconds
                    public float leftDashDist = 50.0f; // Shows distance to move for the dash
                    public float leftDashUseTime = 1.0f; // Shows how often can be dash used

                    private float leftDashTimeCur = 0.0f; // Shows for how long does the left dash works
                    private float leftDashCooldown = 0.0f; // Shows for how long should player wait to use left dash
                */
                if (curDashCooldown <= 0.0f && canDash)
                {
                    LeftDash();
                }
                
                break;
            case MoveDirection.Right:
                if (curDashCooldown <= 0.0f && canDash)
                {
                    RightDash();
                }
                break;
        }
    }

    private void DashPlayer()
    {
        if (curDashCooldown > 0.0f)
        {
            float xPosition = transform.position.x;
            float yPosition = transform.position.y;

            if (isRightDash)
            {
                xPosition += dashSpeed * Time.deltaTime;
            } else
            {
                xPosition -= dashSpeed * Time.deltaTime;
            }

            Vector2 newPosition = new Vector2(xPosition, yPosition);
            transform.position = newPosition;

            curDashCooldown -= Time.deltaTime;
            if (curDashCooldown <= 0.0f)
            {
                SetGravity(true);
                if (isRightDash)
                {
                    animator.SetBool("IsAttacking", false);
                } else
                {
                    animator.SetBool("IsDodging", false);
                }
            }
        }
    }

    private void SetGravity(bool isEnabled)
    {
        if (isEnabled)
        {
            rigidbody.gravityScale = DEFAULT_GRAVITY_SCALE;
        } else
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0.0f;
        }
    }

    private void MovePlayer()
    {
        float xPosition = transform.position.x + (moveSpeed * Time.deltaTime);
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
        animator.SetBool("IsFalling", true);
        Vector2 force = (Vector2.down * jumpForce * rigidbody.mass);
        rigidbody.AddForce(force);
        canFall = false;
    }

    private void LeftDash()
    {
        SetGravity(false);
        canDash = false;
        isRightDash = false;
        curDashCooldown = dashCooldown;
        animator.SetBool("IsDodging", true);
    }

    private void RightDash()
    {
        SetGravity(false);
        canDash = false;
        isRightDash = true;
        curDashCooldown = dashCooldown;
        animator.SetBool("IsAttacking", true);
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
