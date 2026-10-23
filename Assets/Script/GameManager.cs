using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class GameManager : MonoBehaviour
{
    [Header("Estadísticas del Jugador")]
    [SerializeField] private int maxHealth = 100;
    public int currentHealth { get; private set; }
    public int score { get; private set; }
    public int enemiesKilled { get; private set; }

    [Header("Interfaz (UI) Opcional")]
    [SerializeField] private TextMeshProUGUI scoreText;

    public static GameManager Instance { get; private set; }

    public enum GameState { Start, Playing, Paused, GameOver, Victory }
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        score = 0;
        enemiesKilled = 0;
        SetGameState(GameState.Playing);
        UpdateScoreUI();
    }

    private void Update()
    {
        if (Keyboard.current != null && (Keyboard.current.pKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            if (CurrentState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
            }
            else if (CurrentState == GameState.Paused)
            {
                SetGameState(GameState.Playing);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("¡Daño recibido! Vida actual: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            SetGameState(GameState.GameOver);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("¡Ítem de datos recolectado! Puntaje actual: " + score);
        UpdateScoreUI();
    }

    public void AddKills(int amount)
    {
        enemiesKilled += amount;
        Debug.Log($"<color=cyan>¡Enemigo abatido! Bajas totales: {enemiesKilled}</color>");
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"BAJAS: {enemiesKilled} | PUNTOS: {score}";
        }
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Start:
                Time.timeScale = 1f;
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("Juego Pausado");
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("Game Over");
                break;

            case GameState.Victory:
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("¡Victoria!");
                break;
        }
    }
}