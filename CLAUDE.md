# Tower Defense (Unity 3D) — Project Guide for Claude Code

CSCI Video Game Design, Assignment 2. Due Tuesday Nov 17. Target: **A grade** (all C + B + A requirements).
Two teammates work in parallel, each with their own Claude Code session. **Follow the ownership and
contracts below so both halves fit together without merge conflicts.**

## Requirements checklist (all must work)
- [ ] Player can place towers (click on ground)
- [ ] Limited number of towers can be placed (show count remaining)
- [ ] Text overlay clearly explains how to place towers
- [ ] At least 2 tower types with different **rate of fire, projectile speed, and tower height**
- [ ] Towers fire continuously at a set rate, using **3D physics** projectiles (Rigidbody)
- [ ] Towers aim at enemies, using **predicted target position** (current position + velocity)
- [ ] Enemies follow a predetermined **non-straight** path (waypoints)
- [ ] Multiple enemy types with different **speed and HP**
- [ ] Game ends (Game Over screen) when an enemy reaches the end of the path
- [ ] Non-basic assets (imported models/materials, not just default primitives)

## Ownership (only edit files in your own area)
| Area | Owner | Folders |
|---|---|---|
| Enemies, path, spawner, game state, main scene | Teammate A | `Assets/Scripts/Enemies/`, `Assets/Scripts/Core/`, `Assets/Prefabs/Enemies/`, `Assets/Scenes/Main.unity` |
| Towers, projectiles, placement, aiming, UI | Teammate B | `Assets/Scripts/Towers/`, `Assets/Scripts/UI/`, `Assets/Prefabs/Towers/`, `Assets/Scenes/TowerTest.unity` |

- Only Teammate A edits `Main.unity`. Teammate B builds and tests in `TowerTest.unity`, then hands over prefabs.
- If you need a change in the other person's area, ask them instead of editing it.

## Shared contracts (do not change without telling your teammate)

```csharp
// Assets/Scripts/Enemies/Enemy.cs   (Teammate A implements)
public class Enemy : MonoBehaviour {
    public float speed;
    public int maxHp;
    public Vector3 Velocity { get; }        // current world-space velocity, used for predictive aim
    public bool IsDead { get; }
    public void TakeDamage(int amount);      // destroys enemy when HP <= 0
    public static readonly List<Enemy> All;  // all living enemies (register in OnEnable, remove in OnDisable)
}

// Assets/Scripts/Core/GameManager.cs   (Teammate A implements)
public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; }
    public bool IsGameOver { get; }
    public void GameOver();                  // called by Enemy on reaching final waypoint; freezes game, shows UI
}
```

- Enemies are on Layer **"Enemy"** and have a Collider + kinematic Rigidbody.
- Ground is on Layer **"Ground"** (towers are placed by raycasting against this layer only).
- Projectiles (Teammate B) deal damage via `OnCollisionEnter` / `OnTriggerEnter` -> `GetComponentInParent<Enemy>()?.TakeDamage(dmg)`.
- Towers find targets via `Enemy.All` (no `FindObjectsOfType` every frame).
- Towers stop firing when `GameManager.Instance.IsGameOver` is true.

## Implementation notes
- Unity 6 LTS, Universal 3D (URP) template. C#. New Input System if available, otherwise legacy `Input`.
- Tower stats live on a `TowerData` ScriptableObject (fireRate, projectileSpeed, height, range, damage, cost/limit).
- Enemy stats live on an `EnemyData` ScriptableObject (speed, maxHp, model prefab).
- Path = ordered list of empty Transforms (`Waypoint` children of a `Path` object). Draw them with Gizmos.
- Predictive aim: solve for intercept time `t` where `|targetPos + targetVel*t - muzzlePos| = projectileSpeed * t`
  (quadratic). Use the smallest positive root; fall back to direct aim if no solution.
- Keep scripts small, one responsibility each. Expose tunables with `[SerializeField]`.
- After writing scripts, give step-by-step Unity Editor instructions (what to create, which components to add, what to drag where).

## Git rules
- Pull before starting, commit + push small working steps.
- Always commit `.meta` files with their assets.
- Never commit `Library/`, `Temp/`, `Logs/`, `UserSettings/` (covered by Unity .gitignore).
