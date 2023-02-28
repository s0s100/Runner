using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Animations;

// Control general game states such as defeat and overall game speed
public class GameController : MonoBehaviour
{
    private PrefabData startPosition;
    private PlayerController playerMovement;
    private CameraController cameraFollowPlayer;
    private LevelGenerator levelGenerator;
    private UIController uiController;
    private BackgroundController backgroundController;

    private bool isGameRunning = false;
    private bool isDefeated = false;
    [SerializeField]
    private float moveSpeeed = 2.0f;

    [SerializeField]
    private StoragePlayerData storagePlayerData;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerController>();
        cameraFollowPlayer = FindObjectOfType<CameraController>();
        levelGenerator = GetComponent<LevelGenerator>();
        uiController = GetComponent<UIController>();
        backgroundController = GetComponent<BackgroundController>();
        GameObject lastGeneratedPrefab = levelGenerator.GetLastGeneratedPrefab();
        startPosition = lastGeneratedPrefab.GetComponent<PrefabData>();
        SetCurrentPlayerSkin();

        // Increase max FPS to 60 (for every platform for now)
        Application.targetFrameRate = 60;
    }
    
    void Update()
    {
        bool isLegit = Input.anyKey || (Input.touchCount > 0 && !uiController.ShouldDiscardSwipe(Input.touches[0].position));

        if (isLegit)
        {
            StartGame();
            this.enabled = false;
        }
    }

    // Updates current skin according to the saved settings
    private void SetCurrentPlayerSkin()
    {
        int skinIndex = GameMachineControl.GetLastSkinIndex();
        SkinData skin = storagePlayerData.GetIndexSkin(skinIndex);
        AnimatorController animator = skin.GetAnimator();
        Color attackColor = storagePlayerData.GetAttackColor(skinIndex);

        // Select animator for the player
        playerMovement.SetCurrentAnimator(animator);
        playerMovement.SetAttackColor(attackColor);
    }

    public void GameDefeat()
    {
        isDefeated = true;
        playerMovement.enabled = false;
        cameraFollowPlayer.enabled = false;
        backgroundController.enabled = false;
        playerMovement.SetGravity(true);
        uiController.GameDefeatMenu();
    }

    private void StartGame()
    {
        isGameRunning = true;
        playerMovement.EnableMovement();

        playerMovement.enabled = true;
        cameraFollowPlayer.enabled = true;
        levelGenerator.enabled = true;
        backgroundController.enabled = true;
        uiController.DisableStartGameText();
        startPosition.LateDestroy();
        ActivateEveryEnemyGenerator();
        ActivatePrefabDestruction();
        Time.timeScale = 1.0f;
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
        PrefabData[] prefabHolders = FindObjectsOfType<PrefabData>();
        foreach (PrefabData prefabHolder in prefabHolders)
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

    public void SetLiftStop(float xPosition)
    {
        playerMovement.FullStop();
        cameraFollowPlayer.SetStopPosition(xPosition);
    }
    
    public void PrepareBossLocation()
    {
        levelGenerator.ResetPlayerAndLocationPositions();
        cameraFollowPlayer.ResetCamera();
        playerMovement.EnablePlayerAndControls();
        levelGenerator.CreateBoss();
    }
}
