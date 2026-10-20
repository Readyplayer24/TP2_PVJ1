using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private int pointsValue = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(pointsValue);
            }
            Destroy(gameObject);
        }
    }
}