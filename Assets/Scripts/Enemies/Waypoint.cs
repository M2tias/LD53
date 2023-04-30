using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private List<Waypoint> connectedPoints = new List<Waypoint>();

    public List<Waypoint> ConnectedPoints { get { return connectedPoints; } }

    void Start()
    {
        foreach (var point in connectedPoints)
        {
            if (point == this)
            {
                Debug.LogError("Waypoint can't be connected to itself!");
            }
        }

        if (connectedPoints.Count == 0)
        {
            Debug.LogError("No waypoints connected!");
        }
    }

    void Update()
    {
        
    }

    public float DistanceTo(Vector3 point)
    {
        return (point - transform.position).magnitude;
    }
}
