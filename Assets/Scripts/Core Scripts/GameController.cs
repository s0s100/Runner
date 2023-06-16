using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor.Animations;

// Control general game states such as defeat and overall game speed
public class GameController : MonoBehaviour
{
    private const string SPEED_MODIFIER_STORAGE = "Speed modifier";
    private static float SPEED_MODIFIER_INCREMENT = 0.1f; // If it is equal to 0.1, it will become 110% after first level

    private PrefabData startPosition;
    private PlayerController playerMovement;
    private CameraController cameraFollowPlayer;
    private LevelGenerator levelGenerator;
    private UIController uiController;
    private BackgroundController backgroundController;
    private CoinController coinController;
    private ScoreController scoreController;

    private bool isGameRunning = false;
    private bool isDefeated = false;

    [SerializeField]
    private float moveSpeeed = 2.0f;

    [SerializeField]
    private StoragePlayerData storagePlayerData;

    private void Awake()
    {
        UpdateSpeedUsingModifier();
        // Debug.Log("Speed[" + moveSpeeed + "]  Modifier[" + GetSpeedModifier() + "]");
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerController>();
        coinController = FindObjectOfType<CoinController>();
        cameraFollowPlayer = FindObjectOfType<CameraController>();

        scoreController = GetComponent<ScoreController>();
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
        int skinIndex = SkinDisplayControl.GetLastSkinIndex();
        SkinData skin = storagePlayerData.GetBoughtIndexSkin(skinIndex);
        RuntimeAnimatorController runtimeAnimatorController = skin.GetRuntimeAnimator();
        Color attackColor = storagePlayerData.GetAttackColor(skinIndex);

        // Select animator for the player
        playerMovement.SetCurrentAnimator(runtimeAnimatorController);
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
        SetEveryEnemyGenerator(true);
        ActivatePrefabDestruction();
        Time.timeScale = 1.0f;
    }

    // Not the best solution, for now..
    public void SetEveryEnemyGenerator(bool isActive)
    {
        EnemyGenerator[] generators = FindObjectsOfType<EnemyGenerator>();
        foreach (EnemyGenerator generator in generators)
        {
            generator.enabled = isActive;
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

    public void StartNextLevel()
    {
        scoreController.SaveScoreForNextRound();
        coinController.SaveForNextLevelCoins();
        IncreaseSpeedModifier();
        uiController.StartNextLevel();
    }

    public static void ResetSpeedModifier()
    {
        PlayerPrefs.SetFloat(SPEED_MODIFIER_STORAGE, 1.0f);
    }

    public void IncreaseSpeedModifier()
    {
        float curModifier = PlayerPrefs.GetFloat(SPEED_MODIFIER_STORAGE);
        curModifier += SPEED_MODIFIER_INCREMENT;
        PlayerPrefs.SetFloat(SPEED_MODIFIER_STORAGE, curModifier);
    }

    public static float GetSpeedModifier()
    {
        return PlayerPrefs.GetFloat(SPEED_MODIFIER_STORAGE);
    }

    public void UpdateSpeedUsingModifier()
    {
        moveSpeeed *= GetSpeedModifier();
    }

    public UpgradeData GetUpgradeData(string upgradeName)
    {
        return storagePlayerData.GetUpgradeData(upgradeName);
    }
}