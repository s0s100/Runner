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
    private AmmoController ammoController;

    // Mobile touch info
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    // X-Movement variables
    private float dashSpeed = 10.0f;
    private float dashCooldown = 0.2f;
    private float curDashCooldown = 0.0f;

    private float moveSpeed = 0.0f;

    private bool isRightDash = false;
    private bool canDash = false;

    // Player attack
    [SerializeField]
    private GameObject projectile;
    private float attackCooldown = 0.5f;
    private float xProjectileShift = 0.5f;
    private float curAttackCooldown = 0.0f;

    // Y-Movement variables
    private float jumpForce = 400.0f;
    private  float fallForce = 200.0f;

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
        levelGenerator = FindObjectOfType<LevelGenerator>();
        camera = FindObjectOfType<Camera>();
        ammoController = FindObjectOfType<AmmoController>();

        moveSpeed = gameController.GetGameSpeed();
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
        
        MakeAction(moveDir);
        MovePlayer();
        DashPlayer();

        // Reduce cooldown in the end
        ReduceCooldown();
    }

    private void ReduceCooldown()
    {
        if (curAttackCooldown > 0.0f)
        {
            curAttackCooldown -= Time.deltaTime;
        }
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
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            return MoveDirection.Middle;
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
            }

            if (touch.phase == TouchPhase.Ended && startTouchPos != Vector2.zero)
            {
                endTouchPos = touch.position;
                return GetMobileTouchDirection();
            }
        }

        return MoveDirection.None;
    }

    private MoveDirection GetMobileTouchDirection()
    {
        Vector2 diff = endTouchPos - startTouchPos;
        Vector2 absDiff = new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        bool xDiffBigger = absDiff.x > absDiff.y;

        if (diff.magnitude < MAX_TOUCH_VECTOR_MAGNITUDE)
        {
            return MoveDirection.Middle;
        }

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

    private void MakeAction(MoveDirection dir)
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
            case MoveDirection.Middle:
                if (curAttackCooldown <= 0.0f)
                {
                    Attack();
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
                    animator.SetBool("IsShifting", false);
                } else
                {
                    animator.SetBool("IsDodging", false);
                }
            }
        }
    }

    private void Attack()
    {
        if (ammoController.IsAttackPossible())
        {
            animator.SetTrigger("IsAttacking");
            curAttackCooldown = attackCooldown;
            MakeProjectile();
        }
    }

    private void MakeProjectile()
    {
        GameObject newProjectile = Instantiate(projectile);
        Vector3 position = transform.position;
        position.x += xProjectileShift;
        newProjectile.transform.position = position;
        levelGenerator.SetProjectileParent(newProjectile);
        ammoController.RemoveAmmo();
    }

    // Creates projectile after some delay
    public void MakeLateProjectile(float waitDuration)
    {
        StartCoroutine(WaitBeforeThrowing(waitDuration));
    }

    IEnumerator WaitBeforeThrowing(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);
        MakeProjectile();
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
        Vector2 force = (Vector2.down * fallForce * rigidbody.mass);
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
        animator.SetBool("IsShifting", true);
    }
}
