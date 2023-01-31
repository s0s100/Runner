using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Control general game states such as defeat and overall game speed
public class GameController : MonoBehaviour
{
    private PrefabHolder startPosition;
    private PlayerController playerMovement;
    private CameraController cameraFollowPlayer;
    private LevelGenerator levelGenerator;
    private UIController uiController;
    private BackgroundController backgroundController;

    private bool isGameRunning = false;
    private bool isDefeated = false;
    [SerializeField]
    private float moveSpeeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerController>();
        cameraFollowPlayer = FindObjectOfType<CameraController>();
        levelGenerator = GetComponent<LevelGenerator>();
        uiController = GetComponent<UIController>();
        backgroundController = GetComponent<BackgroundController>();
        GameObject lastGeneratedPrefab = levelGenerator.GetLastGeneratedPrefab();
        startPosition = lastGeneratedPrefab.GetComponent<PrefabHolder>();

        // Increase max FPS to 60 (for every platform for now)
        Application.targetFrameRate = 60;
    }
    
    void Update()
    {
        if (Input.anyKey && !uiController.ShouldDiscardSwipe(Input.touches[0].position))
        {
            StartGame();
            this.enabled = false;
        }
    }

    public void GameDefeat()
    {
        isDefeated = true;
        playerMovement.enabled = false;
        cameraFollowPlayer.enabled = false;
        backgroundController.enabled = false;
        uiController.GameDefeatMenu();
    }

    private void StartGame()
    {
        isGameRunning = true;
        playerMovement.EnablePlayerAnimations();

        playerMovement.enabled = true;
        cameraFollowPlayer.enabled = true;
        levelGenerator.enabled = true;
        backgroundController.enabled = true;
        uiController.DisableStartGameText();
        startPosition.LateDestroy();
        ActivateEveryEnemyGenerator();
        ActivatePrefabDestruction();
    }

    // Not the best solution, for now..
    private void ActivateEveryEnemyGenerator()
    {
        EnemyGenerator[] generators = FindObjectsOfType<EnemyGenerator>();
        foreach (EnemyGenerator generator in generators)
        {
            generator.enabled = true;
        }
    }

    private void ActivatePrefabDestruction()
    {
        PrefabHolder[] prefabHolders = FindObjectsOfType<PrefabHolder>();
        foreach (PrefabHolder prefabHolder in prefabHolders)
        {
            prefabHolder.LateDestroy();
        }
    }

    public float GetGameSpeed()
    {
        return moveSpeeed;
    }

    public bool IsGameRunning()
    {
        return isGameRunning;
    }

    public bool IsDefeated()
    {
        return isDefeated;
    }
}
