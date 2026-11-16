using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Click ground to place the selected tower type. Keys 1..N pick the type. Limited by maxTowers.</summary>
public class TowerPlacer : MonoBehaviour
{
    public static TowerPlacer Instance { get; private set; }

    [SerializeField] TowerData[] towerTypes;
    [SerializeField] int maxTowers = 5;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float minSpacing = 1.2f;
    [SerializeField] Camera cam;

    readonly List<Vector3> placed = new List<Vector3>();
    int selected;
    public string Message { get; private set; } = "";

    public int MaxTowers => maxTowers;
    public int Remaining => maxTowers - placed.Count;
    public TowerData Selected => towerTypes != null && towerTypes.Length > 0 ? towerTypes[selected] : null;
    public int SelectedIndex => selected;
    public TowerData[] Types => towerTypes;

    void Awake()
    {
        Instance = this;
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        var kb = Keyboard.current;
        var mouse = Mouse.current;
        if (kb == null || mouse == null || towerTypes == null || towerTypes.Length == 0) return;

        if (kb.digit1Key.wasPressedThisFrame && towerTypes.Length > 0) selected = 0;
        if (kb.digit2Key.wasPressedThisFrame && towerTypes.Length > 1) selected = 1;
        if (kb.digit3Key.wasPressedThisFrame && towerTypes.Length > 2) selected = 2;

        if (mouse.leftButton.wasPressedThisFrame) TryPlace(mouse.position.ReadValue());
    }

    public void Select(int index)
    {
        if (towerTypes != null && index >= 0 && index < towerTypes.Length) selected = index;
    }

    public void TryPlace(Vector2 screenPos)
    {
        if (Remaining <= 0) { Message = "No towers left!"; return; }
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(screenPos);
        if (!Physics.Raycast(ray, out RaycastHit hit, 500f, groundMask)) { Message = "Click on the ground to place a tower."; return; }

        foreach (var p in placed)
            if ((p - hit.point).sqrMagnitude < minSpacing * minSpacing) { Message = "Too close to another tower."; return; }

        var data = Selected;
        if (data == null || data.towerPrefab == null) { Message = "Tower type has no prefab assigned."; return; }

        Tower t = Instantiate(data.towerPrefab, hit.point, Quaternion.identity);
        t.Setup(data);
        placed.Add(hit.point);
        Message = Remaining > 0 ? "Placed " + data.displayName + "." : "That was your last tower!";
    }
}
