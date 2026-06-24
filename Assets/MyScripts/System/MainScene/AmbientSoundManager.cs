using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmbientSoundManager : MonoBehaviour
{
    [Header("BGM")]
    public AudioSource BGMSource;
    public AudioClip NormalBGM;
    public AudioClip ChaseBGM;
    public float BGMFadeDuration = 2.0f;
    
    [Header("Ambient")]
    public AudioSource AmbientSource;
    public AudioClip AmbientClip;

    private bool isChasing = false;

    void Start()
    {
        AmbientSource.clip = AmbientClip;
        AmbientSource.loop = true;
        AmbientSource.Play();

        BGMSource.clip = NormalBGM;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    void Update()
    {
        
    }

    public void SetChasing(bool chasing)
    {
        Debug.Log("SetChasing呼ばれた: " + chasing);
        if(isChasing == chasing)
        {
            return;
        }
        isChasing = chasing;
        StopAllCoroutines();
        StartCoroutine(FadeBGM(chasing ? ChaseBGM : NormalBGM));
    }

    public void StopAllBGM()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutBGM());
    }

    IEnumerator FadeBGM(AudioClip nextClip)
    {
        float startVolume = BGMSource.volume;
        float elapsed = 0f;

        while(elapsed < BGMFadeDuration / 2f)
        {
            elapsed += Time.deltaTime;
            BGMSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / (BGMFadeDuration / 2f));
            yield return null;
        }

        BGMSource.clip = nextClip;
        BGMSource.Play();

        elapsed = 0f;
        while(elapsed < BGMFadeDuration / 2f)
        {
            elapsed += Time.deltaTime;
            BGMSource.volume = Mathf.Lerp(0f, startVolume, elapsed / (BGMFadeDuration / 2f));
            yield return null;
        }
    }

    IEnumerator FadeOutBGM()
    {
        float startVolume = BGMSource.volume;
        float elapsed = 0f;

        while(elapsed < BGMFadeDuration)
        {
            elapsed += Time.deltaTime;
            if(BGMSource != null)
            {
                BGMSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / BGMFadeDuration);
            }
            yield return null;
        }

        if(BGMSource != null)
        {
            BGMSource.Stop();
        }
        if(AmbientSource != null)
        {
            AmbientSource.Stop();
        }
    }
}
