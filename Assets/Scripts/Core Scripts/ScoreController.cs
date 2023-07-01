using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{    
    private static string MAX_SCORE_STORAGE = "MaxScore";
    private static string LAST_LEVEL_SCORE_STORAGE = "LastLevelScore";

    private new Camera camera;
    private PlayerDataScreen playerDataScreen;
    private LevelGenerator levelGenerator;
    private int curScore;
    private int curRequiredScore;

    [SerializeField]
    private TMP_Text scoreText;

    [Space(50)]
    [Header("Score before boss")]
    [SerializeField]
    private int maxBiomeScore = 1000;

    private void Start()
    {  
        playerDataScreen = FindObjectOfType<PlayerDataScreen>();
        levelGenerator = GetComponent<LevelGenerator>();
        camera = Camera.main;

        curRequiredScore = (int) (maxBiomeScore * GameController.GetSpeedModifier());

        // Debug.Log("Score[" + curScore + "]  RequiredScore[" + curRequiredScore + "] SavedScore[" + GetLastRoundScore() + "]");
    }

    private void Update()
    {
        if (curScore >= curRequiredScore)
        {
            UpdateMaxScore();
            levelGenerator.StartBossStage();
            this.enabled = false;
        } else
        {
            ModifyScore();
        }
    }

    private void ModifyScore()
    {
        float travelledDistance = camera.transform.position.x;
        curScore = (int)(travelledDistance * 10);
        scoreText.text = curScore.ToString();
        playerDataScreen.UpdateScoreBar(curScore, curRequiredScore);
    }

    public int GetScore()
    {
        return curScore;
    }

    public static int GetMaxScore()
    {
        return PlayerPrefs.GetInt(MAX_SCORE_STORAGE, 0);
    }
    
    public bool UpdateMaxScore()
    {
        float scoreCheck = curScore + GetLastRoundScore();
        if (scoreCheck > GetMaxScore())
        {
            PlayerPrefs.SetInt(MAX_SCORE_STORAGE ,curScore);
            return true;
        }

        return false;
    }

    public int GetLastRoundScore()
    {
        int result = PlayerPrefs.GetInt(LAST_LEVEL_SCORE_STORAGE);

        return result;
    }

    public void SaveScoreForNextRound()
    {
        int prevScore = PlayerPrefs.GetInt(LAST_LEVEL_SCORE_STORAGE);
        prevScore += curScore;

        PlayerPrefs.SetInt(LAST_LEVEL_SCORE_STORAGE, prevScore);
    }

    public static void ResetLastRoundScore()
    {
        PlayerPrefs.SetInt(LAST_LEVEL_SCORE_STORAGE, 0);
    }
}