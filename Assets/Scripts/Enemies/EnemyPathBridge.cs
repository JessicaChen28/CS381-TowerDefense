using UnityEngine;

/// <summary>
/// Glue between the waypoint mover (BasicEnemy) and the shared Enemy contract, so BasicEnemy.cs stays almost untouched:
/// - copies Enemy.speed onto BasicEnemy so each enemy type can move at its own speed
/// - triggers GameOver when the enemy is removed without having been killed (i.e. it reached the end of the path)
/// </summary>
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(BasicEnemy))]
public class EnemyPathBridge : MonoBehaviour
{
    Enemy enemy;
    static bool quitting;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        GetComponent<BasicEnemy>().speed = enemy.speed;
    }

    void OnApplicationQuit() => quitting = true;

    void OnDestroy()
    {
        if (quitting || enemy == null || enemy.IsDead) return; // killed by a tower, or shutting down
        if (!gameObject.scene.isLoaded) return;                 // scene unloading
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}
