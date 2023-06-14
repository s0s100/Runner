using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private Button skipButton;
    [SerializeField]
    private Image backImage;
    [SerializeField]
    private Image frontImage;

    private void Awake()
    {
        skipButton.onClick.AddListener(SkipTutorial);
    }

    private void SkipTutorial()
    {
        Destroy(this.gameObject);
    }
}
