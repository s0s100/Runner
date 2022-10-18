using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private Camera camera;
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private Animator animator;
    private GameController gameController;

    // private Vector2 defaultPosition = new Vector2(-6.0f, 1.0f);

    // Y-axis movement
    public float jumpForce = 400.0f;
    public float fallForce = 200.0f;
    private bool isGrounded = false;
    //private bool isSliding = false;

    // X-axis movement
    public float collisionReduction = 0.1f;
    public float accelerationBoost = 0.01f;
    private float currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //SetDefaultPosition();

        rigidbody = this.GetComponent<Rigidbody2D>();
        collider = this.GetComponent<BoxCollider2D>();
        animator = this.GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
        camera = FindObjectOfType<Camera>();

        enabled = false;
        currentSpeed = gameController.moveSpeeed;
    }

    public void enableJump()
    {
        isGrounded = true;
    }

    public void enablePlayerAnimations()
    {
        animator.SetBool("GameStarted", true);
    }

    // Update is called once per frame
    void Update()
    {
        //Press jump/slide
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Jump();
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            Fall();
        }

        //Slide
        if (Input.GetKey(KeyCode.S) && isGrounded)
        {
            Slide();
        } else {
            Unslide();   
        }

        boostSpeed();
        float xPosition = transform.position.x + (currentSpeed * Time.deltaTime);
        Vector3 newPosition = new Vector3(xPosition, transform.position.y, 0);
        transform.position = newPosition;
    }

    // Called if player X position is less then Camera speed
    private void boostSpeed()
    {

        if (this.transform.position.x < camera.transform.position.x)
        {
            currentSpeed += currentSpeed * accelerationBoost;
        }
        currentSpeed = gameController.moveSpeeed;
    }

    private void Jump()
    {
        Vector2 force = (Vector2.up * jumpForce * rigidbody.mass);
        rigidbody.AddForce(force);
        isGrounded = false;
        animator.SetBool("IsJumping", true);
    }

    private void Fall()
    {
        Vector2 force = (Vector2.down * jumpForce * rigidbody.mass);
        rigidbody.AddForce(force);
    }

    private void Slide()
    {
        //isSliding = true;
        SetBoxColliderSizeSlide();
        animator.SetBool("IsSliding", true);
    }

    private void Unslide()
    {
        //isSliding = false;
        SetBoxColliderSizeNormal();
        animator.SetBool("IsSliding", false);
    }

    //private void SetDefaultPosition()
    //{
    //    this.transform.position = defaultPosition;
    //}

    private void SetBoxColliderSizeSlide()
    {
        collider.offset = new Vector2(collider.offset.x, 0.0f);
        collider.size = new Vector2(collider.size.x, 0.1f);
    }

    private void SetBoxColliderSizeNormal()
    {
        collider.offset = new Vector2(collider.offset.x, -0.015f);
        collider.size = new Vector2(collider.size.x, 0.13f);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    string collisionTag = collision.gameObject.tag;
    //    // Allow jumping
    //    if (collisionTag == "Ground")
    //    {
    //        isGrounded = true;
    //        animator.SetBool("IsJumping", false);
    //    } else if (collisionTag == "Obstacle")
    //    {
    //        currentSpeed *= (1.0f - collisionReduction);
    //    }
    //}
}
