using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private static int MAX_BIOME_SCORE = 2000;
    private static string MAX_SCORE_STORAGE = "MaxScore";

    private new Camera camera;
    private PlayerDataScreen playerDataScreen;
    private int curScore;

    [SerializeField]
    private TMP_Text scoreText;
    
    
    private void Start()
    {
        playerDataScreen = FindObjectOfType<PlayerDataScreen>();
        camera = Camera.main;
    }

    private void Update()
    {
        float travelledDistance = camera.transform.position.x;
        curScore = (int) (travelledDistance * 10);
        scoreText.text = curScore.ToString();
        playerDataScreen.UpdateScoreBar(curScore, MAX_BIOME_SCORE);
    }

    public int GetScore()
    {
        return curScore;
    }

    public static int GetMaxScore()
    {
        return PlayerPrefs.GetInt(MAX_SCORE_STORAGE);
    }
    
    public bool UpdateMaxScore()
    {
        if (curScore > GetMaxScore())
        {
            PlayerPrefs.SetInt(MAX_SCORE_STORAGE ,curScore);
            return true;
        }

        return false;
    }
}
