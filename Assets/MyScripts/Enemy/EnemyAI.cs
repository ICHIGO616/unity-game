using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] Waypoints;
    public FieldOfView FOV;
    public Transform Player;

    private NavMeshAgent Agent;
    private Animator Anim;
    private int currentIndex = 0;

    public float attackDistance = 1.5f;
    public float idleWaitTime = 4.0f;

    [Header("速度設定")]
    public float patrolSpeed = 1.5f; // 巡回速度
    public float chaseSpeed  = 3.5f; // 追跡速度

    public AmbientSoundManager AmbientSoundManager;
    
    private float idleTimer = 0f;
    private enum State {Patrol, Idle, Chase, Attack};
    private State state = State.Patrol;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();

        Agent.updateRotation = false;
        Agent.stoppingDistance = 0f;
        GoToNextPoint();
    }
    
    void Update()
    {
        switch(state)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Idle:
                Idle();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    void Patrol()
    {
        Anim.SetFloat("Speed", Agent.velocity.magnitude);

        if(!Agent.pathPending && Agent.remainingDistance < 0.5f)
        {
            state = State.Idle;
            idleTimer = 0f;
            Agent.isStopped = true;
            Anim.SetFloat("Speed", 0f);
            return;
        }

        if(FOV.CanSeePlayer())
        {
            state = State.Chase;
            Agent.speed = chaseSpeed;
            Anim.SetBool("IsChasing", true);

            if(AmbientSoundManager != null)
            {
                AmbientSoundManager.SetChasing(true);
            }
        }

        RotateTowards(Agent.steeringTarget);
    }

    void Idle()
    {
        idleTimer += Time.deltaTime;

        if(idleTimer >= idleWaitTime)
        {
            Agent.isStopped = false;
            GoToNextPoint();
            state = State.Patrol;
        }

        if(FOV.CanSeePlayer())
        {
            state = State.Chase;
            Agent.isStopped = false;
            Agent.speed = 3.5f;
            Anim.SetBool("IsChasing", true);

            if(AmbientSoundManager != null)
            {
            AmbientSoundManager.SetChasing(true);
            }
        }
    }

    void Chase()
    {
        Agent.isStopped = false;
        Agent.stoppingDistance = 0f;
        
        Agent.SetDestination(Player.position);
        Anim.SetFloat("Speed", Agent.velocity.magnitude);

        float distance = Vector3.Distance(transform.position, Player.position);

        if(distance <= attackDistance + 0.1f)
        {
            state = State.Attack;
            Agent.isStopped = true;
            Anim.SetBool("IsAttacking", true);
            return;
        }

        Agent.isStopped = false;
        Agent.SetDestination(Player.position);
        RotateTowards(Player.position);

        if(!FOV.CanSeePlayer())
        {
            state = State.Patrol;
            Agent.speed = patrolSpeed;
            Anim.SetBool("IsChasing", false);
            if(AmbientSoundManager != null)
            {
                AmbientSoundManager.SetChasing(false);
            }

            Agent.stoppingDistance = 0f;
            GoToNextPoint();
        }
    }

    void Attack()
    {
        RotateTowards(Player.position);

        float distance = Vector3.Distance(transform.position, Player.position);

        if(distance > attackDistance)
        {       
            state = State.Chase;
            Agent.isStopped = false;
            Anim.SetBool("IsAttacking", false);
            Anim.SetBool("IsChasing", true);
        }
    }

    void GoToNextPoint()
    {
        if(Waypoints.Length == 0)
        {
            return;
        }
        Agent.destination = Waypoints[currentIndex].position;
        currentIndex = (currentIndex + 1) % Waypoints.Length;

    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;

        if(direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * 8f
            );
        }
    }

    public void AttackHit()
    {
        float distance = Vector3.Distance(transform.position, Player.position);

        if(distance <= attackDistance + 0.3f)
        {
            PlayerHealth playerHealth = Player.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }
}
