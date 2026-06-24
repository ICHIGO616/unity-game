using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject TutorialPanel;
    public Image FadeImage; // パネル自体のフェード用

    [Header("Timing")]
    public float displayDuration = 5.0f; // 表示時間
    public float fadeDuration    = 1.0f; // フェードアウト時間

    void Start()
    {
        TutorialPanel.SetActive(true);
        StartCoroutine(ShowTutorial());
    }

    IEnumerator ShowTutorial()
    {
        // 表示時間だけ待機
        yield return new WaitForSeconds(displayDuration);

        // フェードアウト
        float elapsed = 0f;
        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            if(FadeImage != null)
                FadeImage.color = new Color(
                    FadeImage.color.r,
                    FadeImage.color.g,
                    FadeImage.color.b,
                    Mathf.Lerp(1f, 0f, t)
                );

            yield return null;
        }

        TutorialPanel.SetActive(false);
    }
}