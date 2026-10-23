using UnityEngine;

public class Shot : MonoBehaviour
{
    [Header("References")]
    public Transform spawnPoint;
    public GameObject bullet;

    [Header("Shooting Settings")]
    public float shotForce = 100f;
    public float shotRate = 0.5f;

    private float shotRateTime = 0f;

    void Update()
    {
        bool shootInput = false;

        if (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.isPressed)
        {
            shootInput = true;
        }

        if (shootInput && Time.time >= shotRateTime)
        {
            shotRateTime = Time.time + shotRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (bullet == null || spawnPoint == null)
        {
            Debug.LogWarning("Falta asignar el prefab de la bala o el spawnPoint en el script Shot.");
            return;
        }

        GameObject newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(spawnPoint.right * shotForce, ForceMode.Impulse);
        }
    }
}