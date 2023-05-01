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
        if (GameManager.main.GameOver) return;

    }

    public void Shoot()
    {
        Vector3 playerPos = GameManager.main.GetPlayer().transform.position;
        Vector3 vec = playerPos - transform.position;
        float distanceToTarget = vec.magnitude;
        Vector3 moveDir = (vec).normalized;

        lineRenderer.enabled = true;

        int pointCount = 6;
        Vector3[] points = new Vector3[pointCount];
        Vector3[] randomizedPoints = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            Vector3 v = Vector3.zero;
            Vector3 vrand = Vector3.zero;
            if (i == 0)
            {
                v = transform.position;
                vrand = v;
            }
            else if (i == 5)
            {
                v = playerPos; //points[i - 1] + moveDir * (distanceToTarget / pointCount);
                vrand = v;
            }
            else
            {
                v = points[i - 1] + moveDir * (distanceToTarget / pointCount);
                Vector2 unitV = Random.insideUnitCircle.normalized * 0.3f;
                vrand = v + new Vector3(unitV.x, unitV.y, 0);
            }

            points[i] = v;
            randomizedPoints[i] = vrand;
        }

        GameManager.main.TakeDamage();

        lineRenderer.SetPositions(randomizedPoints);
        Invoke("HideZap", .5f);
    }

    void HideZap()
    {
        lineRenderer.enabled = false;
    }
}
