using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageVignette : MonoBehaviour
{
    [Header("References")]
    public Image VignetteImage;

    [Header("Settings")]
    public float flashAlpha = 0.6f;     
    public float falshDuration = 0.1f;  
    public float fadeDuration = 0.5f;   

    public float baseAlpha = 0f;
    private Coroutine vignetteCoroutine;

    void Start()
    {
        SetAlpha(0f);
    }

    void Update()
    {
        
    }

    public void OnDamage(int CurrentHP, int MaxHP)
    {
        baseAlpha = Mathf.Lerp(0f, 0.45f, 1f - (float)CurrentHP / MaxHP);

        if(vignetteCoroutine != null)
        {
            StopCoroutine(vignetteCoroutine);
        }

        vignetteCoroutine = StartCoroutine(FlashVignette());
    }

    IEnumerator FlashVignette()
    {
        SetAlpha(flashAlpha);

        yield return new WaitForSeconds(falshDuration);

        float elapsed = 0f;
        float startAlpha = flashAlpha;

        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            SetAlpha(Mathf.Lerp(startAlpha, baseAlpha, t));
            yield return null;
        }

        SetAlpha(baseAlpha);
    }

    void SetAlpha(float alpha)
    {
        Color c = VignetteImage.color;
        c.a = alpha;
        VignetteImage.color = c;
    }
}
