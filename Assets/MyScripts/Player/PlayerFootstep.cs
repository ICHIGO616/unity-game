using UnityEngine;
using FpsHorrorKit;

public class PlayerFootstep : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource FootstepSource;

    [Header("FootstepClips(by tag)")]
    public AudioClip[] DefaultClips;
    public AudioClip[] WoodClips;
    public AudioClip[] StoneClips;

    [Header("Settings")]
    public float walkInterval = 0.5f;
    public float runInterval = 0.3f;

    public float runSpeedThreshold = 3.0f;  //走っているかどうかを判定する値

    private int footstepIndex = 0;
    private float footstepTimer = 0f;
    private CharacterController controller;
    private Vector3 lastPosition;
    private float positionThreshold = 0.02f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        float moveDistance = new Vector3(currentPosition.x - lastPosition.x, 0, currentPosition.z - lastPosition.z).magnitude;

        lastPosition = currentPosition;

        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.2f);
        bool isMoving = moveDistance > positionThreshold * Time.deltaTime;

        if(!isGrounded || !isMoving)
        {
            footstepTimer = 0f;
            return;
        }

        float interval = moveDistance >  runSpeedThreshold * Time.deltaTime ? runInterval : walkInterval;

        footstepTimer += Time.deltaTime;

        if(footstepTimer >= interval)
        {
            footstepTimer = 0f;
            PlayFootStep();
        }
    }

    void PlayFootStep()
    {
        AudioClip[] clips = GetClipsForCurrentGround();

        if(clips == null || clips.Length == 0)
        {
            return;
        }

        footstepIndex = footstepIndex % clips.Length;
        FootstepSource.clip = clips[footstepIndex];
        FootstepSource.Play();

        footstepIndex++;
    }

    AudioClip[] GetClipsForCurrentGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray, out RaycastHit hit, 1.5f))
        {
            switch(hit.collider.tag)
            {
                case "Wood":    return WoodClips;
                case "Stone":   return StoneClips;
                default:        return DefaultClips;
            }
        }

        return DefaultClips;
    }
}
