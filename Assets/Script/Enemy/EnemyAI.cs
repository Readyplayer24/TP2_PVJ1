using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float patrolRadius = 15f;
    [SerializeField] private float patrolTimer = 5f;
    private float timer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = patrolTimer;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= visionRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= patrolTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, patrolRadius, -1);
                agent.SetDestination(newPos);
                timer = 0f;
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}