using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrulla, Busqueda, Ataque }

    [Header("Estado Actual")]
    public EnemyState currentState = EnemyState.Patrulla;

    [Header("Componentes y Jugador")]
    public NavMeshAgent navMeshAgent;
    public Transform[] destinations;
    private GameObject player;

    [Header("Rangos de Detección")]
    public float searchRadius = 80f;
    public float attackRadius = 30f;

    [Header("Configuración de Patrulla")]
    public bool followPlayer = true;
    private int currentDestinationIndex = 0;
    private float distanceToPlayer;

    [Header("Configuración de Búsqueda Aleatoria")]
    public float searchMoveRadius = 40f;
    private float searchTimer;
    public float timeBetweenSearchPoints = 4f;

    [Header("Configuración de Ataque y Disparo")]
    public float timeBetweenShots = 1.5f;
    private float shootTimer;
    public Transform firePoint;

    void Start()
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();

        PlayerController pc = FindFirstObjectByType<PlayerController>();
        if (pc != null)
        {
            player = pc.gameObject;
        }

        if (destinations.Length > 0 && navMeshAgent != null)
        {
            navMeshAgent.destination = destinations[currentDestinationIndex].position;
        }
    }

    void Update()
    {
        if (player == null)
        {
            PatrolLogic();
            return;
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRadius)
        {
            currentState = EnemyState.Ataque;
            AttackLogic();
        }
        else if (distanceToPlayer <= searchRadius && followPlayer)
        {
            currentState = EnemyState.Busqueda;
            SearchLogic();
        }
        else
        {
            currentState = EnemyState.Patrulla;
            PatrolLogic();
        }
    }

    void PatrolLogic()
    {
        if (destinations.Length == 0 || navMeshAgent == null) return;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 5f)
        {
            currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Length;
            navMeshAgent.destination = destinations[currentDestinationIndex].position;
        }
    }

    void SearchLogic()
    {
        if (navMeshAgent != null)
        {
            searchTimer += Time.deltaTime;

            if (searchTimer >= timeBetweenSearchPoints || navMeshAgent.remainingDistance < 3f)
            {
                searchTimer = 0f;
                Vector3 randomPoint = GetRandomNavMeshPosition(transform.position, searchMoveRadius);
                navMeshAgent.destination = randomPoint;
            }
        }
    }

    Vector3 GetRandomNavMeshPosition(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);

        return navHit.position;
    }

    void AttackLogic()
    {
        if (navMeshAgent != null && player != null)
        {
            navMeshAgent.destination = transform.position;

            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            shootTimer += Time.deltaTime;
            if (shootTimer >= timeBetweenShots)
            {
                shootTimer = 0f;
                ShootAtPlayer();
            }
        }
    }

    void ShootAtPlayer()
    {
        Debug.Log("¡El enemigo disparó al jugador!");

        HealthSystem playerHealth = player.GetComponent<HealthSystem>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        switch (currentState)
        {
            case EnemyState.Patrulla:
                Gizmos.color = Color.cyan;
                break;
            case EnemyState.Busqueda:
                Gizmos.color = Color.yellow;
                break;
            case EnemyState.Ataque:
                Gizmos.color = Color.red;
                break;
        }

        Gizmos.DrawCube(transform.position + Vector3.up * 10f, new Vector3(3f, 3f, 3f));
    }
}