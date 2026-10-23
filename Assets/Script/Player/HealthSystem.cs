using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    [Header("Configuración de Salud")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Interfaz (UI) Opcional")]
    [SerializeField] private Image barraDeVida;
    [SerializeField] private GameObject gameOverPanel;

    private bool isDead;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
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

        if (CompareTag("Player") && GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage(Mathf.RoundToInt(amount));
        }

        if (currentHealth <= 0f)
        {
            Dead();
        }
    }

    private void Dead()
    {
        isDead = true;
        Debug.Log($"<color=red>{gameObject.name} ha sido eliminado.</color>");

        if (CompareTag("Player"))
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
            }
        }
        else
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddKills(1);
            }

            Destroy(gameObject, 0.1f);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}