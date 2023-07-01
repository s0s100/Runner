using System.Collections;
using UnityEngine;

public class LiftClosure : MonoBehaviour
{
    [SerializeField]
    private AudioClip closingSound;

    private GameController gameController;
    private UIController uIController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        uIController = FindObjectOfType<UIController>();
        
    }

    // Called upon lift closure
    public void UploadBossLocation()
    {
        StartCoroutine(SceneUpdate());
        AudioController.instance.PlayEffect(closingSound, transform.position);
    }

    private IEnumerator SceneUpdate()
    {
        float waitTime = uIController.IsBlackScreenInvisible(false);
        // After list closes call a black screening, update background, update player position and start generate boss blocks, after make screen white again
        yield return new WaitForSeconds(waitTime);

        gameController.PrepareBossLocation();
        uIController.IsBlackScreenInvisible(true);
    }
}
