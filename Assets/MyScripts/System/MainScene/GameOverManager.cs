using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("References")]
    public Volume PostProcessVolume;
    public Image FadeImage;
    public Transform PlayerTransform;
    public AmbientSoundManager AmbientSoundManager;

    [Header("Timing")]
    public float fallDuration = 0.6f;   //倒れる時間
    public float deathWait = 0.8f;      //ブラーが始まる時間
    public float blurDuration = 2.0f;   //ぼやける時間
    public float fadeDuration = 2.0f;   //暗転する時間
    public float restartDelay = 1.0f;   //暗転してからリスタートまでの時間

    private DepthOfField DOF;

    void Start()
    {
        FadeImage.color = new Color(0,0,0,0);
        PostProcessVolume.profile.TryGet(out DOF);

        if(DOF != null)
        {
            DOF.nearFocusStart.value = 0f;
            DOF.nearFocusEnd.value = 0f;
        }
    }

    
    void Update()
    {
        
    }

    public void TriggerGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        //倒れる
        yield return StartCoroutine(FallOver());

        yield return new WaitForSeconds(deathWait);

        //ぼやける
        float elapsed = 0f;
        while(elapsed < blurDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / blurDuration;

            if(DOF != null)
            {
                DOF.nearFocusStart.value = 0f;
                DOF.nearFocusEnd.value = Mathf.Lerp(0f, 200f, t);
            }

            yield return null;
        }

        //暗転
        elapsed = 0f;
        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            if(FadeImage != null)
            {
                FadeImage.color = new Color(0, 0, 0, t);
            }
            
            yield return null;
        }

        //リスタート
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator FallOver()
    {
        Quaternion startRotation = PlayerTransform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, 90f);

        float elapsed = 0f;
        while(elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / fallDuration);
            PlayerTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        PlayerTransform.rotation = endRotation;
    }
}
