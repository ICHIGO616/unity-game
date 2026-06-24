using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("UI References")]
    public Image FadeImage;
    public Image BackgroundImage;
    public TextMeshProUGUI TitleText;
    public GameObject TText;
    public Button StartButton;
    public GameObject SButton;
    public Button CreditButton;
    public GameObject CButton;
    public Button ExitButton;
    public GameObject EButton;
    public GameObject CreditPanel;

    [Header("BGM")]
    public AudioSource BGMSource;
    public AudioClip TitleBGMClip;

    [Header("Timing")]
    public float fadeInDuration = 2.0f;

    void Start()
    {
        StartButton.onClick.AddListener(OnStartButton);
        ExitButton.onClick.AddListener(OnExitButton);
        CreditButton.onClick.AddListener(OnCreditButton);

        CreditPanel.SetActive(false);

        if(TitleBGMClip != null)
        {
            BGMSource.clip = TitleBGMClip;
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
            FadeImage.color = new Color(0,0,0,1 - t);
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
        while(elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInDuration;
            FadeImage.color = new Color(0,0,0,t);
            yield return null;
        }

        SceneManager.LoadScene("StoryScene");
    }

    void OnExitButton()
    {
        Application.Quit();
    }

    void OnCreditButton()
    {
        CreditPanel.SetActive(true);
        TText.SetActive(false);
        SButton.SetActive(false);
        EButton.SetActive(false);
        CButton.SetActive(false);
    }

    public void CloseCreditPanel()
    {
        CreditPanel.SetActive(false);
        TText.SetActive(true);
        SButton.SetActive(true);
        EButton.SetActive(true);
        CButton.SetActive(true);
    }
}
