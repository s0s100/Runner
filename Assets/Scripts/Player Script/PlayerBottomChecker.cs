using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomChecker : MonoBehaviour
{
    private PlayerController playerMovement;
    
    // System attached to the player
    [SerializeField]
    private ParticleSystem footstepParticles;
    [SerializeField]
    private GameObject fallParticlesReference;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Ground" && playerMovement.IsMoving())
        {
            CreateFallParticles(fallParticlesReference);
            footstepParticles.Play();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Ground" || tag == "Obstacle")
        {
            playerMovement.EnableJump();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Ground" || tag == "Obstacle")
        {
            playerMovement.DisableJump();
            footstepParticles.Stop();
        }
    }

    private void CreateFallParticles(GameObject particleObject)
    {
        GameObject tracksParticles = Instantiate(particleObject);
        tracksParticles.transform.position = transform.position;
        AddColliderDifference(tracksParticles.transform);

        ParticleSystem particleSystem = tracksParticles.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    private void AddColliderDifference(Transform objTransform)
    {
         BoxCollider2D collider = transform.GetComponent<BoxCollider2D>();
        float yDiff = collider.size.y / 2;
        objTransform.transform.position += Vector3.up * yDiff;
    }
}
