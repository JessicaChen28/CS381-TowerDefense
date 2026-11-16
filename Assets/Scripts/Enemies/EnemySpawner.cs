using UnityEngine;

/// <summary>Spawns the given enemy prefabs in rotation at this object's position (put it on the EnemySpawn marker).</summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float firstDelay = 3f;
    [SerializeField] float interval = 3f;

    float timer;
    int next;

    void Start() => timer = firstDelay;

    void Update()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = interval;

        var prefab = enemyPrefabs[next++ % enemyPrefabs.Length];
        if (prefab != null) Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
