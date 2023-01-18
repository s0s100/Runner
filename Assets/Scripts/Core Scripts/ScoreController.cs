using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private int curScore;

    [SerializeField]
    private TMP_Text scoreText;
    // Score is connected to the camera position
    private new Camera camera;

    private void Start()
    {
        // camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera = Camera.main;
    }

    private void Update()
    {
        float travelledDistance = camera.transform.position.x;
        curScore = (int) (travelledDistance * 10);
        scoreText.text = curScore.ToString();
    }

    public int GetScore()
    {
        return curScore;
    }
}
