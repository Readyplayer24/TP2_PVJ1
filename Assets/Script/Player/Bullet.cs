using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        HealthSystem health = collision.gameObject.GetComponent<HealthSystem>();

        if (health != null)
        {
            health.TakeDamage(25f);

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
