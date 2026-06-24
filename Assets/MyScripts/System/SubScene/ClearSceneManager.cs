using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ClearSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public Image FadeImage;
    public Button BackToTitleButton;

    [Header("Timing")]
    public float fadeInDuration = 2.0f;
    public float fadeOutDuration = 2.0f;

    void Start()
    {
        BackToTitleButton.onClick.AddListener(OnBacktoTitleButton);
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

    void OnBacktoTitleButton()
    {
        StartCoroutine(BackToTitle());
    }

    IEnumerator BackToTitle()
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

        SceneManager.LoadScene("TitleScene");
    }

}
