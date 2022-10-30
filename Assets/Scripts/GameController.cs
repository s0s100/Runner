using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject pauseMenu;

    public float moveSpeeed = 10; // Increases every 100 meters
    private PlayerMovement playerMovement;
    private CameraFollowPlayer cameraFollowPlayer;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        cameraFollowPlayer = FindObjectOfType<CameraFollowPlayer>();
    }
    
    void Update()
    {
        if (Input.anyKey)
        {
            StartGame();
            this.enabled = false;
        }
    }

    public void PauseGame() 
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void StartGame()
    {
        playerMovement.enablePlayerAnimations();
        playerMovement.enabled = true;
        cameraFollowPlayer.enabled = true;
    }
}
