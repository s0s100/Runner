using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum MoveDirection
{
    None, Up, Down, Left, Right, Middle
}

public class PlayerController : MonoBehaviour
{
    private const float DEFAULT_GRAVITY_SCALE = 1.0f;
    private const float MAX_TOUCH_VECTOR_MAGNITUDE = 50.0f;

    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new BoxCollider2D collider;
    private Animator animator;
    private GameController gameController;
    private LevelGenerator levelGenerator;
    private UIController uiController;
    private Weapon weapon;

    // Mobile touch info
    private Vector2 startTouchPos;
    private bool isTrackingTouch;

    // Move
    private float moveSpeed = 0.0f;
    private bool isMoving = false;

    // Dash
    private bool canDash = true;
    private float dashSpeed = 8.0f;
    private float dashCooldown = 0.25f;
    private float curDashCooldown = 0.0f;

    // Jump
    private bool canJump = true;
    private float jumpForce = 400.0f;

    // Fall
    private bool canFall = true;
    private float fallForce = 200.0f;

    public void UpdateWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    // Used when the player lands on the ground
    public void EnableJump()
    {
        canJump = true;
        canDash = true;
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsJumping", false);
    }

    // Used when the player falls off the land
    public void DisableJump()
    {
        canJump = false;
        canFall = true;
        animator.SetBool("IsJumping", true);
    }

    public void EnableMovement()
    {
        isMoving = true;
        animator.SetBool("GameStarted", true);
    }

    void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
        levelGenerator = FindObjectOfType<LevelGenerator>();
        uiController = FindObjectOfType<UIController>();
        camera = Camera.main;

        moveSpeed = gameController.GetGameSpeed();
        isMoving = false;
        enabled = false;
    }
    
    void Update()
    {
        MoveDirection moveDir = GetAction();
        MakeAction(moveDir);
        MovePlayer();
        ReduceCooldowns();
    }

    // Use Queue to prevent double touch error (ignoring one of the commands)
    private MoveDirection GetAction()
    {
        if (Input.touchCount == 0)
        {
            animator.SetBool("IsStopping", false);
            isTrackingTouch = false;
            isMoving = true;
        }

        bool doesTouchStarted = Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began
            && !uiController.ShouldDiscardSwipe(Input.touches[0].position);
        if (doesTouchStarted)
        {
            startTouchPos = Input.touches[0].position;
            isTrackingTouch = true;
        }        

        if (isTrackingTouch)
        {
            return GetMoveDirection();
        }

        // Also track second touch for shooting with a second hand
        bool doesSecondTouchStarted = Input.touchCount > 1;
        if (doesSecondTouchStarted)
        {
            return MoveDirection.Middle;
        }

        return MoveDirection.None;
    }

    private MoveDirection GetMoveDirection()
    {
        Vector2 diff = Input.touches[0].position - startTouchPos;

        if (diff.magnitude > MAX_TOUCH_VECTOR_MAGNITUDE)
        {
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
                } else
                {
                    return MoveDirection.Down;
                }
            }
        }

        return MoveDirection.None;
    }

    private void MakeAction(MoveDirection moveDirection)
    {
        switch (moveDirection)
        {
            case MoveDirection.Up:
                if (canJump)
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
                StopPlayer();
                break;
            case MoveDirection.Right:
                if (curDashCooldown <= 0.0f && canDash)
                {
                    Dash();
                }
                break;
            case MoveDirection.Middle:
                Attack();
                break;
        }
    }

    private void Jump()
    {
        Vector2 force = Vector2.up * jumpForce;
        rigidbody.AddForce(force);
        DisableJump();
    }

    private void Fall()
    {
        Vector2 force = Vector2.down * fallForce;
        rigidbody.AddForce(force);
        canFall = false;
        animator.SetBool("IsFalling", true);
    }

    private void Dash()
    {
        SetGravity(false);
        canDash = false;
        curDashCooldown = dashCooldown;
        animator.SetBool("IsDashing", true);
    }

    private void StopPlayer()
    {
        isMoving = false;
        animator.SetBool("IsStopping", true); 
    }

    private void Attack()
    {
        if (weapon != null)
        {
            weapon.Shoot();
        }
        animator.SetBool("IsAttacking", true);
    }

    private void MovePlayer()
    {
        if (isMoving)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if (curDashCooldown >= 0.0f)
        {
            transform.position += Vector3.right * dashSpeed * Time.deltaTime;
        }
    }

    private void ReduceCooldowns()
    {
        if (curDashCooldown >= 0.0f)
        {
            curDashCooldown -= Time.deltaTime;
            if (curDashCooldown <= 0.0f)
            {
                SetGravity(true);
                animator.SetBool("IsDashing", false);
            }
        } 
    }

    private void SetGravity(bool isEnabled)
    {
        if (isEnabled)
        {
            rigidbody.gravityScale = DEFAULT_GRAVITY_SCALE;
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0.0f;
        }
    }
}
