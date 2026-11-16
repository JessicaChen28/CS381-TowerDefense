using UnityEngine;

/// <summary>Physics projectile: launched by setting Rigidbody velocity, damages an Enemy on impact.</summary>
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float lifetime = 5f;

    Rigidbody rb;
    int damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // straight-line flight so predictive aim stays accurate
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void Launch(Vector3 direction, float speed, int dmg)
    {
        damage = dmg;
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision) => Hit(collision.collider);
    void OnTriggerEnter(Collider other) => Hit(other);

    void Hit(Collider other)
    {
        if (other.GetComponentInParent<Projectile>() != null) return; // ignore other projectiles
        var enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null) enemy.TakeDamage(damage);
        Destroy(gameObject);
    }
}
