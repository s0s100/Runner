using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private Image[] frontImage;
    [SerializeField]
    private GameObject nextTutorial;
    [SerializeField]
    private Button skipButton;
    [SerializeField]
    private Button nextButton;
    
    private int curImage = 0;

    private void Awake()
    {
        skipButton.onClick.AddListener(SkipTutorial);
        nextButton.onClick.AddListener(NextHint);

        if (frontImage.Length > 0)
        {
            frontImage[0].enabled = true;
        } else
        {
            NextTutorial();
        }
    }

    private void NextHint()
    {
        frontImage[curImage].enabled = false;
        curImage++;

        AudioController.instance.PlayButtonClickTwo();

        if (frontImage.Length > curImage)
        {
            // Debug.Log("Showing hint number: " + curImage);
            frontImage[curImage].enabled = true;
        } else
        {
            NextTutorial();
        }
    }

    private void SkipTutorial()
    {
        Destroy(this.gameObject);

        BannerAds bannerAds = AdsInitializer.instance.gameObject.GetComponent<BannerAds>();
        bannerAds.ShowBannerAd();
    }

    private void NextTutorial()
    {
        if (nextTutorial != null)
        {
            Instantiate(nextTutorial);
        } else
        {
            // Debug.Log("No other tutorial available!");
            BannerAds bannerAds = AdsInitializer.instance.gameObject.GetComponent<BannerAds>();
            bannerAds.ShowBannerAd();
        }

        Destroy(this.gameObject);
    }
}
