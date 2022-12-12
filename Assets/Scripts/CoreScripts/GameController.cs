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
    private UIController UIController;
    private BackgroundController backgroundController;

    private bool isDefeated = false;
    [SerializeField]
    private float moveSpeeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerController>();
        cameraFollowPlayer = FindObjectOfType<CameraController>();

        levelGenerator = GetComponent<LevelGenerator>();
        UIController = GetComponent<UIController>();
        backgroundController = GetComponent<BackgroundController>();

        // This is the first generated object: Start location
        GameObject lastGeneratedPrefab = levelGenerator.GetLastGeneratedPrefab();
        startPosition = lastGeneratedPrefab.GetComponent<PrefabHolder>();
    }
    
    void Update()
    {
        if (Input.anyKey)
        {
            StartGame();
            this.enabled = false;
        }
    }
    
    public float GetGameSpeed() { return moveSpeeed; }

    public void GameDefeat()
    {
        isDefeated = true;
        playerMovement.enabled = false;
        UIController.GameDefeatMenu();
    }

    private void StartGame()
    {
        playerMovement.EnablePlayerAnimations();

        playerMovement.enabled = true;
        cameraFollowPlayer.enabled = true;
        levelGenerator.enabled = true;
        backgroundController.enabled = true;
        UIController.DisableStartGameText();
        startPosition.LateDestroy();
    }

    public bool IsDefeated()
    {
        return isDefeated;
    }
}
