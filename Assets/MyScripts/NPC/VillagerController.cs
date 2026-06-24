using UnityEngine;
using UnityEngine.UI;

public class VillagerController : MonoBehaviour
{
    [Header("Move Settings")]
    public Transform DestinationPoint;
    public float rotateSpeed = 5.0f;

    [Header("Animation")]
    public Animator VillagerAnimator;

    private UnityEngine.AI.NavMeshAgent Agent;
    private enum State {Idle, Talk, Walk, Arrived}
    private State state = State.Idle;

    private Quaternion initalRotation;
    private Transform playerTransform;

    void Start()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Agent.isStopped = true;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        initalRotation = transform.rotation;

        UpdateAnimator();
    }

    void Update()
    {
        switch(state)
        {
            case State.Talk:
                FacePlayer();
                break;

            case State.Walk:
                if(!Agent.pathPending && Agent.remainingDistance < 0.3f)
                {
                    Agent.isStopped = true;
                    state = State.Arrived;
                    UpdateAnimator();
                }
                break;
            
            case State.Arrived:
                ReturnToInitialRotation();
                break;
        }
    }

    void FacePlayer()
    {
        if(playerTransform == null)
        {
            return;
        }

        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0;

        if(direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSpeed);
        }
    }

    void ReturnToInitialRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, initalRotation, Time.deltaTime * rotateSpeed);
    }

    public void OnTalkStart()
    {
        state = State.Talk;
        Agent.isStopped = true;
        UpdateAnimator();
    }

    public void OnTalkEnd()
    {
        if(DestinationPoint == null)
        {
            state = State.Arrived;
            UpdateAnimator();
            return;
        }
        state = State.Walk;
        Agent.isStopped = false;
        Agent.SetDestination(DestinationPoint.position);
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        VillagerAnimator.SetBool("IsTalking", state == State.Talk);
        VillagerAnimator.SetBool("IsWalking", state == State.Walk);
    }
}
