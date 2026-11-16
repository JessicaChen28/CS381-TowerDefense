using UnityEngine;

/// <summary>
/// Glue between the waypoint mover (BasicEnemy) and the shared Enemy contract, so BasicEnemy.cs stays almost untouched:
/// - copies Enemy.speed onto BasicEnemy so each enemy type can move at its own speed
/// - when the enemy reaches the final waypoint, damages the base by this enemy's maxHp and removes it
/// </summary>
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(BasicEnemy))]
public class EnemyPathBridge : MonoBehaviour
{
    // Slightly larger than BasicEnemy's own 0.05 arrive distance so we always see the arrival first.
    const float ArriveDistance = 0.15f;

    Enemy enemy;
    bool reachedEnd;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        GetComponent<BasicEnemy>().speed = enemy.speed;
    }

    void Update()
    {
        if (reachedEnd || enemy.IsDead) return;
        var points = Waypoints.points;
        if (points == null || points.Length == 0) return;

        if (Vector3.Distance(transform.position, points[points.Length - 1].position) > ArriveDistance) return;

        reachedEnd = true;
        if (GameManager.Instance != null) GameManager.Instance.DamageBase(enemy.maxHp);
        Destroy(gameObject);
    }
}
