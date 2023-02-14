using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Animations;

public enum MoveDirection
{
    None, Up, Down, Left, Right, Middle
}

public class PlayerController : MonoBehaviour
{
    private const float DEFAULT_GRAVITY_SCALE = 1.0f;
    private const float MAX_TOUCH_VECTOR_MAGNITUDE = 50.0f;

    // Attached objects
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

    // Particles
    [SerializeField]
    private ParticleSystem footstepParticles;
    [SerializeField]
    private GameObject fallParticlesObject;

    // Move
    private float moveSpeed = 0.0f;
    private bool isMoving = false;

    // Dash
    private bool canDash = true;
    private bool canDashCurrently = true;
    private float dashSpeed = 8.0f;
    private float dashStopCooldown = 0.3f;
    private float dashCooldown = 0.5f;
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

    public bool IsMoveParticles()
    {
        return isMoving;
    }

    public void DisableDash()
    {
        canDash = false;
    }

    public void EnableDash()
    {
        canDash = true;
    }

    public void EnableCurDash()
    {
        canDashCurrently = true;
    }

    public void DisableCurDash()
    {
        canDashCurrently = false;
    }

    // Used when the player lands on the ground
    public void EnableJump()
    {
        if (canDashCurrently)
        {
            canDash = true;
        }

        canJump = true;
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
    }

    void Awake()
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
        MoveDirection moveDir = GetPCAction();
        if (moveDir == MoveDirection.None)
        {
            moveDir = GetAction();
        }

        MakeAction(moveDir);
        MovePlayer();
        ReduceCooldowns();
    }

    private MoveDirection GetPCAction()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return MoveDirection.Up;
        } else 
        if (Input.GetKeyDown(KeyCode.S))
        {
            return MoveDirection.Down;
        } else
        if (Input.GetKey(KeyCode.A))
        {
            return MoveDirection.Left;
        } else
        if (Input.GetKeyDown(KeyCode.D))
        {
            return MoveDirection.Right;
        } else
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return MoveDirection.Middle;
        }

        return MoveDirection.None;
    }

    // Use Queue to prevent double touch error (ignoring one of the commands)
    private MoveDirection GetAction()
    {
        if (Input.touchCount == 0)
        {
            isTrackingTouch = false;
            AllowMovement();
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
            MoveDirection directionResult = GetMoveDirection();
            if (directionResult != MoveDirection.None)
            {
                return directionResult;
            }
        }

        // Also track second touch for shooting with a second hand
        bool doesSecondTouchStarted = Input.touchCount > 1;
        if (doesSecondTouchStarted)
        {
            Attack();
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
                    startTouchPos = Input.touches[0].position;
                    return MoveDirection.Right;
                } else
                {
                    return MoveDirection.Left;
                }
            } else
            {
                if (diff.y > 0)
                {
                    startTouchPos = Input.touches[0].position;
                    return MoveDirection.Up;
                } else
                {
                    startTouchPos = Input.touches[0].position;
                    return MoveDirection.Down;
                }
            }
        }

        return MoveDirection.None;
    }

    private void MakeAction(MoveDirection moveDirection)
    {
        // If dashing nothing can be activated
        if (curDashCooldown >= dashStopCooldown) return;

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
                if (canJump)
                {
                    StopPlayer();
                }
                break;
            case MoveDirection.Right:
                if (curDashCooldown <= 0.0f && canDash && canDashCurrently)
                {
                    Dash();
                }
                break;
            case MoveDirection.Middle:
                Attack();
                break;
        }
    }

    private void AllowMovement()
    {
        isMoving = true;
        animator.SetBool("IsMoving", true);
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
        animator.SetBool("IsMoving", false);
    }

    private void Attack()
    {
        if (weapon != null)
        {
            weapon.Shoot();
        }
        animator.SetTrigger("IsAttacking");
    }

    private void MovePlayer()
    {
        if (isMoving)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if (curDashCooldown >= dashStopCooldown)
        {
            transform.position += Vector3.right * dashSpeed * Time.deltaTime;
        }
    }

    private void ReduceCooldowns()
    {
        if (curDashCooldown >= 0.0f)
        {
            curDashCooldown -= Time.deltaTime;
            if (curDashCooldown <= dashStopCooldown)
            {
                SetGravity(true);
                animator.SetBool("IsDashing", false);
            }
        } 
    }

    public void SetGravity(bool isEnabled)
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

    public void SetFootstepParticleState(bool isActive)
    {
        if (isActive)
        {
            footstepParticles.Play();
        } else
        {
            footstepParticles.Stop();
        }
    }

    public void CreateFallParticles()
    {
        GameObject tracksParticles = Instantiate(fallParticlesObject);
        tracksParticles.transform.position = transform.position;
        AddColliderDifference(tracksParticles.transform);

        GameObject levelParent = levelGenerator.GetAnimationParent();
        tracksParticles.transform.parent = levelParent.transform;
    }

    private void AddColliderDifference(Transform objTransform)
    {
        BoxCollider2D collider = transform.GetComponent<BoxCollider2D>();
        float yDiff = collider.size.y / 2;
        objTransform.transform.position -= Vector3.up * yDiff;
    }

    public void SetCurrentAnimator(AnimatorController animatorController)
    {
        animator.runtimeAnimatorController = animatorController as RuntimeAnimatorController;
    }
}
