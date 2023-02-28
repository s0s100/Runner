using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftClosure : MonoBehaviour
{
    private GameController gameController;
    private UIController uIController;

    private float waitTime = 1.5f;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        uIController = FindObjectOfType<UIController>();
        
    }

    // Called upon lift closure
    public void UploadBossLocation()
    {
        StartCoroutine(SceneUpdate());
    }

    private IEnumerator SceneUpdate()
    {
        uIController.IsBlackScreenInvisible(false);
        // After list closes call a black screening, update background, update player position and start generate boss blocks, after make screen white again
        yield return new WaitForSeconds(waitTime);

        gameController.PrepareBossLocation();
        uIController.IsBlackScreenInvisible(true);
    }
}
