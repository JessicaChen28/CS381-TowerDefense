using UnityEngine;

/// <summary>On-screen instructions, selected tower stats and towers-remaining counter (IMGUI, no Canvas setup needed).</summary>
public class TowerHud : MonoBehaviour
{
    [SerializeField] TowerPlacer placer;

    GUIStyle title, body, small;

    void Awake()
    {
        if (placer == null) placer = FindAnyObjectByType<TowerPlacer>();
    }

    void OnGUI()
    {
        if (placer == null) return;
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        float scale = Screen.height / 720f;
        if (title == null)
        {
            title = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
            body = new GUIStyle(GUI.skin.label);
            small = new GUIStyle(GUI.skin.label);
        }
        title.fontSize = Mathf.RoundToInt(26 * scale);
        body.fontSize = Mathf.RoundToInt(20 * scale);
        small.fontSize = Mathf.RoundToInt(16 * scale);
        title.normal.textColor = body.normal.textColor = small.normal.textColor = Color.white;

        float w = 520 * scale, x = Screen.width - w - 15 * scale, y = 15 * scale;
        GUI.Box(new Rect(x - 10 * scale, y - 5 * scale, w + 20 * scale, 235 * scale), GUIContent.none);

        GUI.Label(new Rect(x, y, w, 36 * scale), "Towers remaining: " + placer.Remaining + " / " + placer.MaxTowers, title);
        y += 38 * scale;
        GUI.Label(new Rect(x, y, w, 30 * scale), "HOW TO PLAY: Left-click the ground to place a tower.", body);
        y += 28 * scale;
        GUI.Label(new Rect(x, y, w, 30 * scale), "Press 1 / 2 to choose the tower type.", body);
        y += 34 * scale;

        var types = placer.Types;
        if (types != null)
        {
            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];
                if (t == null) continue;
                string mark = i == placer.SelectedIndex ? "> " : "   ";
                GUI.Label(new Rect(x, y, w, 24 * scale),
                    mark + (i + 1) + ": " + t.displayName + "  (fire " + t.fireRate.ToString("0.0") + "/s, speed " +
                    t.projectileSpeed.ToString("0") + ", height " + t.height.ToString("0.0") + ")", small);
                y += 24 * scale;
            }
        }
        y += 6 * scale;
        GUI.Label(new Rect(x, y, w, 26 * scale), placer.Message, small);
    }
}
