using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float speed = 1f;
    private Transform target;
    private int waypointIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = Waypoints.points[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized*speed*Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.position) <= 0.05f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length-1)
        {
            Destroy(gameObject);
            return;
        }

        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }
}
