using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Raycast Shoot Settings")]
    [SerializeField] private Transform gun;
    [SerializeField] private float rayDistance = 50f;
    [SerializeField] private Vector3 offsetGun;

    [Header("Melee Settings")]
    [SerializeField] private Transform knife;
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private Vector3 offsetKnife;

    [Header("Attack Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float cooldownAttack = 1f;
    private bool canAttack = true;
    private Vector3 origin;
    private Vector3 dir;
    private bool isCharacter;
    private RaycastHit hit;

    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isMelee;

    private PlayerController pC;

    private void Awake()
    {
        pC = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (isPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
            {
                if (isMelee)
                {
                    Melee();
                }
                else
                {
                    Shoot();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!isMelee)
        {
            if (hit.collider == null) return;
            Gizmos.color = isCharacter ? Color.green : Color.red;
            Gizmos.DrawLine(origin, hit.point);
        }
        else
        {
            Gizmos.color = isCharacter ? Color.green : Color.red;
            Gizmos.DrawWireSphere(origin, radius);
        }
    }

    public void Shoot()
    {
        RaycastInfo(offsetGun);

        if (Physics.Raycast(origin, dir, out hit, rayDistance))
        {
            ApplyDamage(hit.collider.gameObject);
        }
        StartCoroutine(AttackCooldown());
    }

    public void Melee()
    {
        RaycastInfo(offsetKnife);
        Collider[] colliders = Physics.OverlapSphere(origin, radius);
        if (colliders.Length > 0)
        {
            foreach (Collider col in colliders)
            {
                if (col.gameObject.CompareTag("Character") || col.gameObject.CompareTag("Player"))
                {
                    ApplyDamage(col.gameObject);
                }
            }
        }
        else
        {
            isCharacter = false;
        }
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        float currentSpeed = 0f;
        if (isPlayer && pC != null)
        {
            currentSpeed = pC.Speed;
            pC.Speed = 0f;
            canAttack = false;

            yield return new WaitForSeconds(cooldownAttack);

            pC.Speed = currentSpeed;
            canAttack = true;
        }
        else
        {
            canAttack = false;
            yield return new WaitForSeconds(cooldownAttack);
            canAttack = true;
        }
    }

    private void ApplyDamage(GameObject hitObj)
    {
        if (hitObj != null && (hitObj.CompareTag("Player") || hitObj.CompareTag("Character")))
        {
            HealthSystem health = hitObj.GetComponent<HealthSystem>();
            if (health != null)
            {
                Debug.Log("<color=red>Se aplicó daño</color>");
                health.TakeDamage(damage);
                isCharacter = true;
            }
        }
        else
        {
            isCharacter = false;
        }
    }

    private void RaycastInfo(Vector3 offset)
    {
        if (knife != null)
        {
            origin = knife.TransformPoint(offset);
        }
        else
        {
            origin = transform.position;
        }
        dir = transform.forward;
    }
}