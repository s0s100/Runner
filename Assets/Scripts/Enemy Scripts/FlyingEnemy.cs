using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FlyingEnemy : Enemy
{
    private const float DEFAULT_SOUND_VOLUME = 0.02f;

    [SerializeField]
    private AudioClip flyingSound;

    [SerializeField]
    private AudioClip flyDeth;

    private AudioSource audioSource;

    // Sin() function movement (cos() acceleration coz of derivative)
    // y = A*sin(xtC)
    private bool leftToRightMovement = false;
    private bool isSinMoving = false;
    private float sinMoveAmplitude = 2.0f;
    private float sinMoveTimeCoef = 2.0f;
    private float destroyUponDeath = 5.0f;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        Destroy(this.gameObject, existanceTime);
        CheckMoveDirection();
    }

    private void CheckMoveDirection()
    {
        bool isChangingDirection = this.transform.position.x < 
            Camera.main.transform.position.x;
        if (isChangingDirection)
        {
            SetLeftToRightMovement();
        }
    }

    private void Start()
    {
        DefineIfSin();
        StartAudioSounds();
    }

    private void StartAudioSounds()
    {
        audioSource.volume = DEFAULT_SOUND_VOLUME;
        audioSource.loop = true;
        audioSource.clip = flyingSound;
        audioSource.Play();
    }

    private void StartDethSounds()
    {
        audioSource.clip = flyDeth;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void DefineIfSin()
    {
        if (Random.value > 0.5f)
        {
            SetSinMovement();
        }
    }

    public void SetSinMovement()
    {
        isSinMoving = true;
    }

    public void SetLeftToRightMovement()
    {
        leftToRightMovement = true;
        speed = gameController.GetGameSpeed() * 2;
        this.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    protected override void Movement()
    {
        // X movement
        Vector3 speedIncrement;
        if (leftToRightMovement)
        {
            speedIncrement = Vector3.right * speed * Time.deltaTime; // Result speed equal to player speed
        }
        else
        {
            speedIncrement = (Vector3.left * speed) * Time.deltaTime; // Result speed equal to player speed x2
        }

        // Y movement
        if (isSinMoving)
        {
            // Depends on the X, the faster it moves, the less it depends on the x speed
            float sinCoef = this.transform.position.x * (sinMoveTimeCoef / speed);
            speedIncrement += Mathf.Cos(sinCoef) * sinMoveAmplitude * Vector3.up * Time.deltaTime;
        }

        this.transform.position += speedIncrement;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && health > 0)
        {
            PlayerHealthScript playerHealthScript =
                collision.gameObject.GetComponent<PlayerHealthScript>();

            playerHealthScript.GetDamage();
        }
    }

    protected override void Kill()
    {
        base.Kill();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Destroy(this.gameObject, destroyUponDeath);
        StartDethSounds();
    }
}