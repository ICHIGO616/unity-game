using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StoryManager : MonoBehaviour
{
    [Header("UI References")]
    public Image BackgroundImage;
    public TextMeshProUGUI StoryText;
    public Button StartButton;
    public Image FadeImage;

    [Header("Story Text")]
    [TextArea(3, 19)]
    public string Story;

    [Header("Timing")]
    public float fadeInDuration = 1.5f;
    public float fadeOutDuration = 1.5f;

    [Header("遷移先シーン")]
    public string nextSceneName = "Scene";

    [Header("BGM")]
    public AudioSource BGMSource;
    public AudioClip StoryBGMClip;

    void Start()
    {
        StartButton.onClick.AddListener(OnStartButton);
        StoryText.text = Story;

        if(StoryBGMClip != null)
        {
            BGMSource.clip = StoryBGMClip;
            BGMSource.loop = true;
            BGMSource.Play();
        }

        StartCoroutine(FadeIn());
    }

    void Update()
    {
        
    }

    IEnumerator FadeIn()
    {
        FadeImage.color = new Color(0,0,0,1);

        float elapsed = 0f;
        while(elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInDuration;

            if(FadeImage != null)
            {
                FadeImage.color = new Color(0,0,0,1 - t);
            }

            yield return null;
        }

        FadeImage.color = new Color(0,0,0,0);
    }

    void OnStartButton()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        float elapsed = 0f;
        while(elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;

            if(FadeImage != null)
            {
                FadeImage.color = new Color(0,0,0,t);
            }

            yield return null;
        }

    SceneManager.LoadScene(nextSceneName);
    }
}
