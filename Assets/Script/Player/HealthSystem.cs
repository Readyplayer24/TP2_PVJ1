using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Configuración de Salud")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Interfaz (UI) Opcional")]
    [SerializeField] private Image barraDeVida;

    [SerializeField] private bool isDead;
    public bool IsDead => isDead;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    private void Update()
    {
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = currentHealth / maxHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"<color=yellow>Salud actual de {gameObject.name}: {currentHealth}</color>");

        if (currentHealth <= 0f)
        {
            Dead();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log($"<color=green>Salud recuperada: {currentHealth}</color>");
    }

    private void Dead()
    {
        isDead = true;
        Debug.Log($"<color=red>{gameObject.name} ha sido eliminado.</color>");

        if (CompareTag("Player"))
        {
            // Opcional: Desactivar control o reiniciar
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }
    }
}