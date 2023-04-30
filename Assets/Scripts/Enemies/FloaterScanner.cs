using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterScanner : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> circlePoints = new();

    [SerializeField]
    private float scanCD;

    [SerializeField]
    private float maxScanRange;

    [SerializeField]
    private float scanTime; // how many seconds scanning takes

    [SerializeField]
    private FloaterShooter shooter;

    private LineRenderer lineRenderer;
    private float currentRadius = 0f;
    private float lastScan = 0f;
    private bool startScan = false;
    private float scanT = 0f;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        UpdateCircle(1f);
    }

    private void UpdateCircle(float radius)
    {
        circlePoints = new List<Vector3>();
        for (int i = 0; i < 16; i++)
        {
            float moveAngle = 360f / 16f;

            float vertical = radius * Mathf.Sin((moveAngle * i) * Mathf.PI / 180);
            float horizontal = radius * Mathf.Cos((moveAngle * i) * Mathf.PI / 180);

            circlePoints.Add(new Vector3(horizontal, vertical, 0f) + transform.position);
        }

        lineRenderer.SetPositions(circlePoints.ToArray());
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 vec = GameManager.main.GetPlayer().transform.position - shooter.transform.position;
        float distanceToTarget = vec.magnitude;
        Vector2 moveDir = (vec).normalized;

        UpdateCircle(currentRadius);

        if (Time.time - scanCD > lastScan)
        {
            currentRadius = 0f;
            lineRenderer.enabled = true;
            lastScan = Time.time;
            startScan = true;
        }

        if (startScan)
        {
            scanT += Time.deltaTime / scanTime;
            currentRadius = Mathf.Lerp(0, maxScanRange, scanT);
        }

        if (currentRadius >= maxScanRange)
        {
            Debug.Log(scanT);
            currentRadius = 0;
            lineRenderer.enabled = false;
            startScan = false;
            lastScan = Time.time;
            scanT = 0;

            if (distanceToTarget <= maxScanRange)
            {
                shooter.Shoot();
            }
        }
    }
}
