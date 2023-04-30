using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterShooter : MonoBehaviour
{
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        Vector3 vec = GameManager.main.GetPlayer().transform.position - transform.position;
        float distanceToTarget = vec.magnitude;
        Vector3 moveDir = (vec).normalized;

        lineRenderer.enabled = true;

        int pointCount = 6;
        Vector3[] points = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            Vector3 v = Vector3.zero;
            if (i == 0)
            {
                v = transform.position;
            }
            else
            {
                v = points[i - 1] + moveDir * i;
            }

            points[i] = v;
        }


    }
}
