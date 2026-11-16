using UnityEngine;

/// <summary>
/// TowerTest scene only. Creates a GameManager (if none) and spawns test enemies at runtime,
/// so the scene holds no serialized references to teammate-owned scripts.
/// </summary>
public class TowerTestHarness : MonoBehaviour
{
    [SerializeField] float spawnInterval = 2.5f;
    [SerializeField] Vector3 spawnPoint = new Vector3(-14f, 0.5f, 0f);
    [SerializeField] float lane = 6f;      // enemies spread along z in [-lane, lane]
    [SerializeField] float lifetime = 30f;

    float timer;
    int count;

    void Start()
    {
        if (GameManager.Instance == null)
            new GameObject("GameManager (test)").AddComponent<GameManager>();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        timer -= Time.deltaTime;
        if (timer <= 0f) { Spawn(); timer = spawnInterval; }
    }

    void Spawn()
    {
        bool fast = count++ % 2 == 0;
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = fast ? "TestEnemy_Fast" : "TestEnemy_Tank";
        go.layer = LayerMask.NameToLayer("Enemy");
        go.transform.position = spawnPoint + new Vector3(0f, 0f, Random.Range(-lane, lane));
        go.transform.rotation = Quaternion.Euler(0f, 90f, 0f); // forward = +X
        go.transform.localScale = fast ? Vector3.one * 0.8f : Vector3.one * 1.3f;
        go.GetComponent<Renderer>().material.color = fast ? Color.green : Color.red;

        var rb = go.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        go.SetActive(false); // so Enemy.OnEnable sees the stats set below
        var enemy = go.AddComponent<Enemy>();
        enemy.speed = fast ? 4f : 1.5f;
        enemy.maxHp = fast ? 2 : 8;
        go.SetActive(true);

        Destroy(go, lifetime);
    }
}
