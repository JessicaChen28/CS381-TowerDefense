using UnityEngine;

/// <summary>Predictive aim: solves |targetPos + targetVel*t - muzzle| = speed*t for the smallest positive t.</summary>
public static class AimSolver
{
    /// <summary>Returns the world point to aim at. Falls back to the target's current position if no intercept exists.</summary>
    public static Vector3 PredictAimPoint(Vector3 muzzle, Vector3 targetPos, Vector3 targetVel, float projectileSpeed)
    {
        Vector3 d = targetPos - muzzle;
        float a = Vector3.Dot(targetVel, targetVel) - projectileSpeed * projectileSpeed;
        float b = 2f * Vector3.Dot(d, targetVel);
        float c = Vector3.Dot(d, d);

        float t = -1f;
        if (Mathf.Abs(a) < 1e-4f)
        {
            if (Mathf.Abs(b) > 1e-6f) t = -c / b; // linear case (target as fast as projectile)
        }
        else
        {
            float disc = b * b - 4f * a * c;
            if (disc >= 0f)
            {
                float sq = Mathf.Sqrt(disc);
                float t1 = (-b - sq) / (2f * a);
                float t2 = (-b + sq) / (2f * a);
                if (t1 > 0f && t2 > 0f) t = Mathf.Min(t1, t2);
                else t = Mathf.Max(t1, t2); // one positive root at most
            }
        }

        if (t <= 0f) return targetPos; // no solution: aim directly
        return targetPos + targetVel * t;
    }
}
