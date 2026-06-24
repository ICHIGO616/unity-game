using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource VoiceSource;
    public AudioSource GameOverBGMSource;

    [Header("Audio Clips")]
    public AudioClip[] GrowlClips;
    public AudioClip DeathClip;
    public AudioClip GameOverBGMClip;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayGrowl()
    {
        if(GrowlClips.Length == 0)
        {
            return;
        }

        if(VoiceSource.isPlaying)
        {
            return;
        }

        VoiceSource.clip = GrowlClips[Random.Range(0, GrowlClips.Length)];
        VoiceSource.Play();
    }

    public void PlayDeath()
    {
        VoiceSource.Stop();
        VoiceSource.clip = DeathClip;
        VoiceSource.Play();

        GameOverBGMSource.clip = GameOverBGMClip;
        GameOverBGMSource.loop = false;
        GameOverBGMSource.Play();
    }
}
