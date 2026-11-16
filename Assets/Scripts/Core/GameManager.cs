using UnityEngine;

/// <summary>
/// Shared contract (Teammate A owns). Singleton game state.
/// Enemy mover calls GameOver() when an enemy reaches the final waypoint.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        IsGameOver = false;
        Time.timeScale = 1f;
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
