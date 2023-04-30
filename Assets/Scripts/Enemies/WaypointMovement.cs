using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField]
    private List<Waypoint> waypoints = new List<Waypoint>();

    [SerializeField]
    private float movementSpeed;

    private Waypoint currentWayPoint;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float smallestDistance = 1000f;
        foreach (var waypoint in waypoints)
        {
            Vector3 v = transform.position - waypoint.transform.position;
            float dist = v.magnitude;

            if (dist < smallestDistance)
            {
                smallestDistance = dist;
                currentWayPoint = waypoint;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec = currentWayPoint.transform.position - transform.position;
        float distanceToTarget = vec.magnitude;
        Vector2 moveDir = (vec).normalized;

        if (distanceToTarget > 0.15f)
        {
            rb.velocity = moveDir * movementSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        Vector3 playerPos = GameManager.main.GetPlayer().transform.position;
        Vector3 vecToPlayer = playerPos - transform.position;
        float distToPlayer = vecToPlayer.magnitude;

        foreach (var waypoint in currentWayPoint.ConnectedPoints)
        {
            if (waypoint.DistanceTo(playerPos) < distToPlayer)
            {
                currentWayPoint = waypoint;
            }
        }

    }
}
