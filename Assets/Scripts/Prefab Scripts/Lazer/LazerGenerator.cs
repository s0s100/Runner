using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGenerator : MonoBehaviour
{
    private const float MAX_LAZER_LENGTH = 25.0f;

    [SerializeField]
    private bool isActive;
    [SerializeField]
    private GameObject lazerStart;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private GameObject lazerEnd;

    private float curLazerWidth = 0.0f;
    private float maxLazerWidth = 1.0f;
    private float lazerIncreasementSpeed = 1.5f;

    private void Update()
    {
        if (isActive)
        {
            GenerateLazer();
        } else
        {
            StopLazerGeneration();
        }
    }

    private void GenerateLazer()
    {
        SetLazerPositions();
        lazerStart.GetComponent<ParticleSystem>().Play();

        if (curLazerWidth < maxLazerWidth)
        {
            curLazerWidth += lazerIncreasementSpeed * Time.deltaTime;
            lineRenderer.startWidth = curLazerWidth;
        }
    }

    private void StopLazerGeneration()
    {
        lazerStart.GetComponent<ParticleSystem>().Stop();
        lazerEnd.GetComponent<ParticleSystem>().Stop();

        if (curLazerWidth > 0.0f)
        {
            curLazerWidth -= lazerIncreasementSpeed * Time.deltaTime;
            lineRenderer.startWidth = curLazerWidth;
        } else
        {
            curLazerWidth = 0.0f;
        }
    }

    private void SetLazerPositions()
    {
        // Use default start position
        Vector3 startPos = lazerStart.transform.position;
        lineRenderer.SetPosition(0, startPos);

        Vector3 direction = GetLazerDirection();
        Vector3 endPos = CalculateEndPosition(startPos, direction);
        lineRenderer.SetPosition(1, endPos);
    }

    private Vector3 CalculateEndPosition(Vector2 startPosition, Vector2 direction)
    {
        // If doesn't hit anything set value to MAX_LAZER_LENGTH, otherwise to hit position
        float hitDistance = MAX_LAZER_LENGTH;

        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction);
        bool hitStatus = hit.collider != null && (hit.collider.tag == "Ground" || hit.collider.tag == "Player");
        if (hitStatus) { hitDistance = hit.distance; }
        Vector2 resultPosition = startPosition + direction * hitDistance;

        if (hitStatus) {
            HitActivity(resultPosition);
        } else
        {
            HitDisable();
        }

        return resultPosition;
    }

    private Vector3 GetLazerDirection()
    {
        return lazerStart.transform.right;
    }

    private void HitActivity(Vector2 hitPosition)
    {
        lazerEnd.GetComponent<ParticleSystem>().Play();
        lazerEnd.transform.rotation = lazerStart.transform.rotation;
        lazerEnd.transform.position = hitPosition;
    }

    private void HitDisable()
    {
        lazerEnd.GetComponent<ParticleSystem>().Pause();
    }

    private void CheckPlayerDamage(RaycastHit2D hit)
    {
        string tag = hit.collider.tag;
        if (tag == "Player")
        {
            PlayerHealthScript healthScript = 
                hit.collider.gameObject.GetComponent<PlayerHealthScript>();
            healthScript.GetDamage();
        }
    }

    public void Activate()
    {
        if (isActive == false)
        {
            isActive = true;
        }
    }

    public void Deactivate()
    {
        if (isActive == true)
        {
            isActive = false;

        }
    }
}
