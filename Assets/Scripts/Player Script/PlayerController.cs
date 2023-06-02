using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Animations;

public enum MoveDirection
{
    None, Up, Down, Left, Right, Middle
}

public enum PowerUpEffect
{
    None, DoubleJump
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject disappearingDiamond;
    private Vector3 dissapearingDiamondPosition = new (0.0f, 0.5f, 0.0f);

    private const float REQUIRED_FALL_SPEED_FALL_PARTICLES = 0.5f;
    private const float DEFAULT_GRAVITY_SCALE = 1.0f;
    private const float MAX_TOUCH_VECTOR_MAGNITUDE = 50.0f;
    private const float PUSH_WAIT_BEFORE_CONTROL = 0.75f;

    // Attached objects
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new BoxCollider2D collider;
    private Animator animator;
    private GameController gameController;
    private LevelGenerator levelGenerator;
    private UIController uiController;
    private PlayerDataScreen playerDataScreen;

    // Mobile touch info
    private Vector2 startTouchPos;
    private bool isTrackingTouch;

    // Particles prefabs attached
    [SerializeField]
    private ParticleSystem footstepParticles;
    [SerializeField]
    private GameObject fallParticlesObject;

    // Move
    private float moveSpeed = 0.0f;
    private bool isMoving = false;
    private bool isControllable = true;

    // Dash
    private bool canDash = true;
    private bool IsNotBlocked = true;
    private float dashSpeed = 8.0f;
    private float dashStopCooldown = 0.3f;
    private float dashCooldown = 0.5f;
    private float curDashCooldown = 0.0f;

    // Jump
    private float timeBetweenJumps = 0.5f;
    private bool canDoubleJump = false;
    private bool canJump = true;
    private float timeBeforeJump = 0.0f;
    private float jumpForce = 400.0f;

    // Fall
    private bool canFall = true;
    private float fallForce = 200.0f;
    private float slowedMovementCoef = 0.6f;

    // Next action buffer
    private MoveDirection savedAction = MoveDirection.None;
    private float saveActionTime = 0.25f;
    private float curSaveActionTime = 0.0f;

    // Attack
    private Color attackColor;
    private float attackCooldown = 5.0f;
    private float curAttackCooldown = 0.0f;

    // Played pushed
    private float xPushForce = 0.0f;
    private float yPushForce = 0.0f;
    private float curPushTime = 0.0f;

    // Power up
    PowerUpEffect powerUpEffect = PowerUpEffect.None;
    private float powerUpDuration = 0.0f;
    private float maxPowerUpDuration = 0.0f;

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
        IsNotBlocked = true;
    }

    public void DisableCurDash()
    {
        IsNotBlocked = false;
    }

    // Used when the player lands on the ground
    public void EnableJump()
    {
        if (IsNotBlocked)
        {
            canDash = true;
        }

        if (powerUpEffect == PowerUpEffect.DoubleJump)
        {
            canDoubleJump = true;
        }

        canJump = true;
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsJumping", false);
    }

    // Used when the player falls off the land or used a jump
    public void DisableJump()
    {
        //Debug.Log("2: Can jump: " + canJump + " , canDoubleJump: " + canDoubleJump + " , timeBeforeJump: " + timeBeforeJump + " , Cur effect: " + powerUpEffect.ToString() +
        //            " , duration: " + powerUpDuration);

        if (canDoubleJump)
        {
            timeBeforeJump = timeBetweenJumps;
            canDoubleJump = false;
            canJump = true;
        } else if (timeBeforeJump < 0)
        {
            canJump = false;
        }
        
        canFall = true;
        animator.SetBool("IsJumping", true);
    }

    public void EnableMovement()
    {
        isMoving = true;
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
        levelGenerator = FindObjectOfType<LevelGenerator>();
        uiController = FindObjectOfType<UIController>();
        playerDataScreen = FindObjectOfType<PlayerDataScreen>();
        camera = Camera.main;

        SetStartingAttackSpeed();
        moveSpeed = gameController.GetGameSpeed();
        isMoving = false;
        enabled = false;
        isControllable = true;
    }

    void Update()
    {
        if (isControllable)
        {
            MoveDirection moveDir = GetPCAction();
            if (moveDir == MoveDirection.None)
            {
                moveDir = GetAction();
            }

            MakeStoredAction(); // Make action stored in a buffer 
            MakeAction(moveDir);
        }

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
        }

        return MoveDirection.None;
    }

    // Use Queue to prevent double touch error (ignoring one of the commands)
    private MoveDirection GetAction()
    {
        if (Input.touchCount == 0)
        {
            moveSpeed = gameController.GetGameSpeed();
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

    private void UpdatePowerUpState()
    {
        if (powerUpDuration > 0)
        {
            powerUpDuration -= Time.deltaTime;
            if (powerUpDuration <= 0)
            {
                playerDataScreen.SetAmmoCounter(curAttackCooldown, attackCooldown);
                playerDataScreen.SetEmptyHandIcon();
                powerUpEffect = PowerUpEffect.None;
                canDoubleJump = false;
            }
        }
    }

    private void MakeStoredAction()
    {
        if (curSaveActionTime > 0.0f)
        {
            MakeAction(savedAction);
        }
    }

    private void MakeAction(MoveDirection moveDirection)
    {
        // If dashing nothing can be activated
        if (curDashCooldown >= dashStopCooldown) return;

        switch (moveDirection)
        {
            case MoveDirection.Up:
                //Debug.Log("1: Can jump: " + canJump + " , canDoubleJump: " + canDoubleJump + " , timeBeforeJump: " + timeBeforeJump + " , Cur effect: " + powerUpEffect.ToString() +
                //    " , duration: " + powerUpDuration);

                if (canJump && timeBeforeJump <= 0.0f)
                {
                    Jump();
                } else
                {
                    StoreAction(moveDirection);
                }
                break;
            case MoveDirection.Down:
                if (canFall)
                {
                    Fall();
                } else
                {
                    StoreAction(moveDirection);
                }
                break;
            case MoveDirection.Left:
                if (canJump)
                {
                    StopPlayer();
                } else
                {
                    SlowMovement();
                }
                break;
            case MoveDirection.Right:
                if (curDashCooldown <= 0.0f && canDash && IsNotBlocked)
                {
                    Dash();
                } else
                {
                    StoreAction(moveDirection);
                }
                break;
        }
    }

    private void StoreAction(MoveDirection moveDirection)
    {
        if (curSaveActionTime <= 0.0f)
        {
            savedAction = moveDirection;
            curSaveActionTime = saveActionTime;
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
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(force);
        DisableJump();

        curSaveActionTime = 0.0f;
    }

    private void SlowMovement()
    {
        moveSpeed = gameController.GetGameSpeed() * slowedMovementCoef;
    }

    private void Fall()
    {
        Vector2 force = Vector2.down * fallForce;
        rigidbody.AddForce(force);
        canFall = false;
        canJump = false;
        animator.SetBool("IsFalling", true);

        curSaveActionTime = 0.0f;
    }

    private void Dash()
    {
        SetGravity(false);
        canDash = false;
        curDashCooldown = dashCooldown;
        animator.SetBool("IsDashing", true);

        curSaveActionTime = 0.0f;
    }

    private void StopPlayer()
    {
        isMoving = false;
        animator.SetBool("IsMoving", false);
    }

    private void MovePlayer()
    {
        if (isMoving && IsNotBlocked)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if (curDashCooldown >= dashStopCooldown)
        {
            transform.position += Vector3.right * dashSpeed * Time.deltaTime;
        }

        if (curPushTime > 0.0f)
        {
            transform.position += Vector3.right * xPushForce * Time.deltaTime;
            transform.position -= Vector3.up * yPushForce * Time.deltaTime;
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

        if (curSaveActionTime >= 0.0f)
        {
            curSaveActionTime -= Time.deltaTime;
        }

        // Show time between attacks, if powerup is used show it instead
        if (powerUpEffect != PowerUpEffect.None)
        {
            playerDataScreen.SetAmmoCounter((maxPowerUpDuration - powerUpDuration), maxPowerUpDuration);    
        } else 
        {
            if (curAttackCooldown >= 0.0f)
            {
                playerDataScreen.SetAmmoCounter(curAttackCooldown, attackCooldown);
                curAttackCooldown -= Time.deltaTime;
            }
        }

        if (curPushTime > 0.0f)
        {
            curPushTime -= Time.deltaTime;
        }

        if (timeBetweenJumps > 0)
        {
            timeBeforeJump -= Time.deltaTime;   
        }

        UpdatePowerUpState();
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
        if (-rigidbody.velocity.y > REQUIRED_FALL_SPEED_FALL_PARTICLES)
        {
            GameObject tracksParticles = Instantiate(fallParticlesObject);
            tracksParticles.transform.position = transform.position;
            AddColliderDifference(tracksParticles.transform);

            GameObject levelParent = levelGenerator.GetAnimationParent();
            tracksParticles.transform.parent = levelParent.transform;
        }
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

    public void SetAttackColor(Color color)
    {
        AttackCheckScript attackCheckScript = GetComponentInChildren<AttackCheckScript>();
        attackCheckScript.SetAttackColor(color);
    }

    public void Attack()
    {
        if (CanAttack())
        {
            animator.SetTrigger("IsAttacking");
            curAttackCooldown = attackCooldown;
            playerDataScreen.FillAmmoBar();
        }
    }

    public bool CanAttack()
    {
        return curAttackCooldown <= 0.0f;
    }

    // If player is not on the ground, make him fall and prevent player from movement
    public void DisableControl()
    {
        if (!canJump)
        {
            curDashCooldown = 0.0f;
            Fall();
        }

        isMoving = true;
        isControllable = false;
    }

    // Called to completely stop player
    public void FullStop()
    {
        animator.SetBool("IsMoving", false);
        this.enabled = false;
    }

    public void EnablePlayerAndControls()
    {
        this.enabled = true;
        isMoving = true;
        isControllable = true;
    }

    public void SetControllable(bool isControllable)
    {
        this.isControllable = isControllable;
    }

    private void CreatePush(float xForce, float yForce)
    {
        curPushTime = PUSH_WAIT_BEFORE_CONTROL;
        xPushForce = xForce;
        yPushForce = yForce;
    }

    public void PushPlayerBack(float xForce, float yForce)
    {
        CreatePush(xForce, yForce);
        DisableControl();
        StartCoroutine(ReturnControl());
    }

    private IEnumerator ReturnControl()
    {
        yield return new WaitForSeconds(PUSH_WAIT_BEFORE_CONTROL);
        EnablePlayerAndControls();
    }

    public void MakeQuickShift(float shiftDistance)
    {
        transform.position -= Vector3.right * shiftDistance;
    }

    public void AllowDoubleJump(float duration, Sprite newSprite, Color color)
    {
        powerUpDuration = duration;
        maxPowerUpDuration = duration;
        powerUpEffect = PowerUpEffect.DoubleJump;
        playerDataScreen.UpdateWeaponImage(newSprite);
        playerDataScreen.SetIndicatorColor(color);

        if (canJump)
        {
            canDoubleJump = true;
        } else
        {
            canJump = true;
        }
    }

    public void CreateDisappearingDiamond()
    {
        GameObject newDiamond = Instantiate(disappearingDiamond);
        newDiamond.transform.SetParent(this.transform);
        newDiamond.transform.localPosition = dissapearingDiamondPosition;
    }

    private void SetStartingAttackSpeed()
    {
        UpgradeData upgradeData = gameController.GetUpgradeData("Attack Speed");
        int upgradeLevel = upgradeData.GetUpgradeStatus();
        if (upgradeLevel != 0)
        {
            attackCooldown = 2.0f;
        }
    }
}
