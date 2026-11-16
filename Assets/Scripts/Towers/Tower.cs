using UnityEngine;

/// <summary>
/// Finds the nearest enemy in range via Enemy.All, aims with predicted intercept, and fires Rigidbody projectiles at a fixed rate.
/// Prefab layout: Root(Tower) > Body (visual) and Turret (rotates) > Muzzle (spawn point, child of Turret).
/// </summary>
public class Tower : MonoBehaviour
{
    [SerializeField] TowerData data;
    [SerializeField] Transform body;
    [SerializeField] Transform turret;
    [SerializeField] Transform muzzle;
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] float turnSpeed = 720f; // degrees per second

    float cooldown;

    public TowerData Data => data;

    /// <summary>Called by the placer so one prefab can serve any TowerData.</summary>
    public void Setup(TowerData towerData)
    {
        data = towerData;
        ApplyVisuals();
    }

    void Awake() => ApplyVisuals();

    void ApplyVisuals()
    {
        if (data == null) return;
        if (body != null)
        {
            Vector3 s = body.localScale;
            body.localScale = new Vector3(s.x, data.height, s.z);
            body.localPosition = new Vector3(0f, data.height * 0.5f, 0f);
            var r = body.GetComponent<Renderer>();
            if (r != null) r.material.color = data.color;
        }
        if (turret != null) turret.localPosition = new Vector3(0f, data.height, 0f);
    }

    void Update()
    {
        if (data == null || muzzle == null) return;
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        cooldown -= Time.deltaTime;

        Enemy target = FindTarget();
        if (target == null) return;

        Vector3 aimPoint = AimSolver.PredictAimPoint(muzzle.position, target.transform.position, target.Velocity, data.projectileSpeed);
        Vector3 dir = aimPoint - muzzle.position;

        RotateTurret(dir);

        if (cooldown <= 0f)
        {
            Fire(dir);
            cooldown = 1f / Mathf.Max(0.01f, data.fireRate);
        }
    }

    Enemy FindTarget()
    {
        Enemy best = null;
        float bestSqr = data.range * data.range;
        Vector3 pos = transform.position;
        foreach (var e in Enemy.All)
        {
            if (e == null || e.IsDead) continue;
            float sqr = (e.transform.position - pos).sqrMagnitude;
            if (sqr <= bestSqr) { bestSqr = sqr; best = e; }
        }
        return best;
    }

    void RotateTurret(Vector3 dir)
    {
        if (turret == null || dir.sqrMagnitude < 1e-6f) return;
        Quaternion want = Quaternion.LookRotation(dir);
        turret.rotation = Quaternion.RotateTowards(turret.rotation, want, turnSpeed * Time.deltaTime);
    }

    void Fire(Vector3 dir)
    {
        if (projectilePrefab == null) return;
        var p = Instantiate(projectilePrefab, muzzle.position, Quaternion.LookRotation(dir));
        p.Launch(dir, data.projectileSpeed, data.damage);
    }

    void OnDrawGizmosSelected()
    {
        if (data == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
}
