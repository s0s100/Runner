using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    private float curSpeed = 0.0f;
    private Vector3 moveVector = Vector3.zero;
    private float creationTime;

    [SerializeField]
    private bool isXMovement = true;

    [SerializeField]
    private float sinCoef = 0.5f;

    [SerializeField]
    private float timeCoef = 2.0f;

    [SerializeField]
    private float sinTimeShift = 0.0f;

    private GameObject attachedPlayer = null;

    private void Awake()
    {
        creationTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        Movement();
    }

    private void UpdateSpeed()
    {
        curSpeed = sinCoef * Mathf.Sin((Time.time - creationTime) * timeCoef + ((sinTimeShift * Mathf.PI) / 180));

        if (isXMovement)
        {
            moveVector = Vector3.right * curSpeed * Time.deltaTime;
        } else
        {
            moveVector = Vector3.up * curSpeed * Time.deltaTime;
        }
    }

    private void Movement()
    {
        this.transform.position += moveVector;

        // If any player is on the collision move him as well
        if (attachedPlayer != null)
        {
            AddVelocityToPlayer(attachedPlayer);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject playerObject = collision.gameObject;
            attachedPlayer = playerObject;
        }   
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            attachedPlayer = null;
        }
    }

    private void AddVelocityToPlayer(GameObject playerObject)
    {
        playerObject.transform.position += moveVector;
    }
}
