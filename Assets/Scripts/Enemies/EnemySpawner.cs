using UnityEngine;

/// <summary>
/// Spawns the given enemy prefabs in rotation at this object's position (put it on the EnemySpawn marker).
/// Fixed spawn rate: one enemy every `interval` seconds inside a wave, `waveDelay` seconds of rest between waves.
/// Stops after `totalWaves` waves or `maxEnemies` enemies, whichever comes first.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float firstDelay = 3f;
    [Tooltip("Seconds between enemies inside a wave (fixed spawn rate).")]
    [SerializeField] float interval = 3f;
    [Tooltip("Enemies spawned in each wave.")]
    [SerializeField] int enemiesPerWave = 5;
    [Tooltip("Seconds of rest between the end of one wave and the start of the next.")]
    [SerializeField] float waveDelay = 8f;
    [Tooltip("Number of waves, then spawning stops. 0 = unlimited.")]
    [SerializeField] int totalWaves = 4;
    [Tooltip("Hard cap on enemies spawned in total. 0 = unlimited.")]
    [SerializeField] int maxEnemies = 20;

    float timer;
    int next;
    int spawnedInWave;

    public int Wave { get; private set; }          // current wave number (1-based, 0 before the first)
    public int TotalSpawned { get; private set; }
    public bool Finished { get; private set; }

    void Start()
    {
        timer = firstDelay;
        Wave = 0;
        TotalSpawned = 0;
        spawnedInWave = enemiesPerWave; // forces "start a new wave" on the first spawn
        Finished = false;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        // Everything spawned and everything cleared -> victory.
        if (Finished)
        {
            if (Enemy.All.Count == 0 && GameManager.Instance != null) GameManager.Instance.Win();
            return;
        }
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        if (spawnedInWave >= enemiesPerWave)
        {
            if (totalWaves > 0 && Wave >= totalWaves) { Finished = true; return; }
            Wave++;
            spawnedInWave = 0;
        }

        if (maxEnemies > 0 && TotalSpawned >= maxEnemies) { Finished = true; return; }

        var prefab = enemyPrefabs[next++ % enemyPrefabs.Length];
        if (prefab != null) Instantiate(prefab, transform.position, Quaternion.identity);
        TotalSpawned++;
        spawnedInWave++;

        bool waveDone = spawnedInWave >= enemiesPerWave;
        bool allDone = (totalWaves > 0 && Wave >= totalWaves && waveDone) ||
                       (maxEnemies > 0 && TotalSpawned >= maxEnemies);
        if (allDone) { Finished = true; return; }

        timer = waveDone ? waveDelay : interval;
    }
}
