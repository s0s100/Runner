using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public TMP_Text scoreText;
    // Score is connected to the camera position
    private GameObject camera;

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        float travelledDistance = camera.transform.position.x;
        int newScore = (int) (travelledDistance * 10);
        scoreText.text = newScore.ToString();
    }
}
