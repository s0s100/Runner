using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    private float curSpeed = 0.0f;
    private Vector3 moveVector = Vector3.zero;

    [SerializeField]
    private bool isXMovement = true;

    [SerializeField]
    private float sinCoef = 0.5f;

    [SerializeField]
    private float timeCoef = 2.0f;

    [SerializeField]
    private float sinTimeShift = 0.0f;

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        Movement();
    }

    private void UpdateSpeed()
    {
        curSpeed = sinCoef * Mathf.Sin(Time.time * timeCoef + sinTimeShift);

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
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject playerObject = collision.gameObject;
            AddVelocityToPlayer(playerObject);
        }   
    }

    private void AddVelocityToPlayer(GameObject playerObject)
    {
        playerObject.transform.position += moveVector;
    }
}
