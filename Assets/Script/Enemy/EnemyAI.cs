using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Attack }

    [Header("States")]
    [SerializeField] private AIState currentState = AIState.Patrol;

    [Header("Detect Settings")]
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private float attackRadius = 2f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtPoint = 2f;
    private int currentPatrolIndex;
    private bool isWaitingAtPoint;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int damageAmount = 20;
    private bool isAttacking = false;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private HealthSystem healthSystemPlayer;
    private Attack attack;
    private bool isStunned;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attack = GetComponent<Attack>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            healthSystemPlayer = playerObj.GetComponent<HealthSystem>();
        }
        else
        {
            Debug.LogError("No hay GameObjects con tag Player en la escena.");
        }

        if (patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }
    }

    private void Update()
    {
        if (agent == null) return;

        if (isStunned || playerTransform == null || (healthSystemPlayer != null && healthSystemPlayer.IsDead))
        {
            agent.isStopped = true;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case AIState.Patrol:
                UpdatePatrolState(distanceToPlayer);
                break;

            case AIState.Chase:
                UpdateChaseState(distanceToPlayer);
                break;

            case AIState.Attack:
                UpdateAttackState(distanceToPlayer);
                break;
        }
    }

    private void UpdatePatrolState(float distanceToPlayer)
    {
        agent.isStopped = false;

        if (distanceToPlayer <= detectionRadius)
        {
            currentState = AIState.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaitingAtPoint)
            {
                StartCoroutine(WaitAndMoveToNextPoint());
            }
        }
    }

    private void UpdateChaseState(float distanceToPlayer)
    {
        agent.isStopped = false;
        agent.SetDestination(playerTransform.position);

        if (distanceToPlayer <= attackRadius)
        {
            currentState = AIState.Attack;
            return;
        }

        if (distanceToPlayer > detectionRadius * 1.2f)
        {
            currentState = AIState.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void UpdateAttackState(float distanceToPlayer)
    {
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));

        if (distanceToPlayer > attackRadius && !isAttacking)
        {
            currentState = AIState.Chase;
            return;
        }

        if (!isAttacking)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        agent.isStopped = true;
        ExecuteAttack();
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        agent.isStopped = false;
    }

    private void ExecuteAttack()
    {
        Debug.Log("Ataque contra el jugador");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage(damageAmount);
        }

        if (attack != null)
        {
            attack.Melee();
        }
    }

    private IEnumerator WaitAndMoveToNextPoint()
    {
        isWaitingAtPoint = true;
        yield return new WaitForSeconds(waitTimeAtPoint);

        if (patrolPoints.Length > 0)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            GoToNextPatrolPoint();
        }

        isWaitingAtPoint = false;
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0 || agent == null) return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        if (agent != null) agent.isStopped = true;
        Debug.Log($"[ENEMIGO] Aturdido por {duration} segundos.");

        yield return new WaitForSeconds(duration);

        isStunned = false;
        if (agent != null) agent.isStopped = false;
        Debug.Log("[ENEMIGO] Recuperado del aturdimiento.");
    }
}