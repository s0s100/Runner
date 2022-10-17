using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private Animator animator;

    private Vector2 defaultPosition = new Vector2(-6.0f, 1.0f);

    public float jumpForce = 400.0f;
    public float fallForce = 200.0f;
    private bool isGrounded = false;

    public float slideTime = 1.0f;
    private float slideHoldCurrent = 0.0f;
    private bool isSliding = false;



    // Start is called before the first frame update
    void Start()
    {
        SetDefaultPosition();
        rigidbody = this.GetComponent<Rigidbody2D>();
        collider = this.GetComponent<BoxCollider2D>();
        animator = this.GetComponent<Animator>();
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
    }

    private void Jump()
    {
        rigidbody.AddForce(Vector2.up * jumpForce);
        isGrounded = false;
        animator.SetBool("IsJumping", true);
    }

    private void Fall()
    {
        rigidbody.AddForce(Vector2.down * jumpForce);
    }

    private void Slide()
    {
        isSliding = true;
        SetBoxColliderSizeSlide();
        slideHoldCurrent += Time.deltaTime;
        animator.SetBool("IsSliding", true);
    }

    private void Unslide()
    {
        isSliding = false;
        SetBoxColliderSizeNormal();
        slideHoldCurrent = 0.0f;
        animator.SetBool("IsSliding", false);
    }

    private void SetDefaultPosition()
    {
        this.transform.position = defaultPosition;
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Allow jumping
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
        }
    }
}
