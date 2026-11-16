using UnityEngine;

/// <summary>
/// Shared contract (Teammate A owns). Singleton game state.
/// Enemy mover calls GameOver() when an enemy reaches the final waypoint.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int maxBaseHealth = 15;

    public bool IsGameOver { get; private set; }
    public int MaxBaseHealth => maxBaseHealth;
    public int BaseHealth { get; private set; }

    // Statics survive between plays when Domain Reload is disabled, so reset them explicitly.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics() => Instance = null;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        IsGameOver = false;
        BaseHealth = maxBaseHealth;
        Time.timeScale = 1f;
    }

    /// <summary>An enemy reached the end: subtract its HP from the base. Game over at 0.</summary>
    public void DamageBase(int amount)
    {
        if (IsGameOver) return;
        BaseHealth = Mathf.Max(0, BaseHealth - amount);
        Debug.Log("Base health: " + BaseHealth + " / " + maxBaseHealth);
        if (BaseHealth <= 0) GameOver();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Time.timeScale = 1f;
        }
    }

    /// <summary>Freezes the game and shows the Game Over screen.</summary>
    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Time.timeScale = 0f;
        Debug.Log("GAME OVER");
    }

    // Placeholder Game Over screen; Teammate A can replace with a Canvas later.
    void OnGUI()
    {
        if (!IsGameOver) return;
        var style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 64,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        style.normal.textColor = Color.red;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "GAME OVER", style);
    }
}
