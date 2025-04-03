using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;

    [Header("Navigation Settings")]
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderTimer = 5f;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackRange = 2f;

    private float timer;
    private bool isChasing = false;

    private void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = patrolSpeed;
        timer = wanderTimer;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if player is in detection radius
        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
            agent.speed = chaseSpeed;
            ChasePlayer();
        }
        else
        {
            isChasing = false;
            agent.speed = patrolSpeed;
            RandomWander();
        }

        // Update animations
        UpdateAnimations();
    }

    private void RandomWander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        // Check if in attack range
        if (agent.remainingDistance <= attackRange)
        {
            // TODO: Implement attack behavior
            // For now, just stop moving
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    private void UpdateAnimations()
    {
        // Set walking/running animation based on speed
        if (agent.velocity.magnitude > 0.1f)
        {
            if (isChasing)
                animator.SetBool("IsRunning", true);
            else
                animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw wander radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
} 