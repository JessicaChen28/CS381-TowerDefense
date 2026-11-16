using UnityEngine;

/// <summary>Stats for one tower type. Create assets via Assets > Create > TowerDefense > Tower Data.</summary>
[CreateAssetMenu(menuName = "TowerDefense/Tower Data", fileName = "TowerData")]
public class TowerData : ScriptableObject
{
    public string displayName = "Tower";
    [Tooltip("Shots per second.")] public float fireRate = 1f;
    [Tooltip("Projectile speed in units/second.")] public float projectileSpeed = 15f;
    [Tooltip("Visual height of the tower (world units). Muzzle sits at this height.")] public float height = 1.5f;
    public float range = 8f;
    public int damage = 1;
    public Color color = Color.white;
    public Tower towerPrefab;
}
