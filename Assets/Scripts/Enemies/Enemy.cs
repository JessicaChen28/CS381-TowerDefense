using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shared contract (Teammate A owns). Towers read Velocity for predictive aim
/// and call TakeDamage from projectiles.
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>All living enemies. Towers should iterate this instead of searching the scene.</summary>
    public static readonly List<Enemy> All = new List<Enemy>();

    public float speed = 2f;
    public int maxHp = 3;
    [Tooltip("Placeholder: walk along transform.forward. Turn off when a waypoint mover is attached.")]
    public bool moveForward = true;

    int hp;
    Vector3 lastPosition;
    Vector3 velocity;

    /// <summary>Current world-space velocity (measured from actual movement, so it works with any mover).</summary>
    public Vector3 Velocity => velocity;
    public bool IsDead { get; private set; }

    void OnEnable()
    {
        All.Add(this);
        hp = maxHp;
        IsDead = false;
        lastPosition = transform.position;
        velocity = Vector3.zero;
    }

    void OnDisable()
    {
        All.Remove(this);
    }

    void Update()
    {
        // Placeholder movement so towers can be tested. Disable once a waypoint mover drives the enemy.
        if (moveForward)
            transform.position += transform.forward * (speed * Time.deltaTime);

        if (Time.deltaTime > 0f)
            velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    /// <summary>Reduces HP; destroys the enemy when HP reaches 0.</summary>
    public void TakeDamage(int amount)
    {
        if (IsDead) return;
        hp -= amount;
        if (hp <= 0)
        {
            IsDead = true;
            All.Remove(this); // leave the list immediately so towers stop targeting it this frame
            Destroy(gameObject);
        }
    }
}
