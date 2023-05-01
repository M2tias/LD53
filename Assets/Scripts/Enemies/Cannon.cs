using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private GameObject gunSmokePrefab;
    [SerializeField]
    private Transform pipe;
    [SerializeField]
    private float shootDistance;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float timeBeforeShoot;
    [SerializeField]
    private float shootCD;

    private AudioSource shootSFX;
    private LineRenderer lineRenderer;
    private float targetAcquiredTime = 0f;
    private bool targetAcquired = false;
    private float lastShot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        shootSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.main.GameOver) return;
        GameObject player = GameManager.main.GetPlayer();

        if (Vector3.Distance(transform.position, player.transform.position) <= shootDistance)
        {
            Vector3 dir = player.transform.position - pipe.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
            pipe.rotation = Quaternion.RotateTowards(pipe.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            float z = pipe.localRotation.eulerAngles.z;

            if (z > 180)
            {
                z = -360 + z;
            }

            bool clamped = false;
            if (z > 95 || z < -95)
            {
                clamped = true;
                targetAcquired = false;
                targetAcquiredTime = 0;
            }

            pipe.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(z, -90, 90));

            Vector2 rayOrigin = new Vector2(pipe.position.x, pipe.position.y);
            int shootMask = LayerMask.GetMask(new[] { "Ground", "Player" });

            RaycastHit2D hitInfo = Physics2D.Raycast(rayOrigin, -pipe.right, shootDistance, shootMask);

            // Debug.DrawRay(rayOrigin, -pipe.right);
            bool offCD = Time.time - lastShot > shootCD;

            if (hitInfo.collider != null && !clamped && offCD)
            {
                // Debug.Log(hitInfo.collider.tag);
                if (hitInfo.collider.tag == "Player")
                {
                    Vector3 firstLinePos = pipe.position;
                    Vector2 dir2 = new Vector2(dir.x, dir.y);
                    Vector2 dir2Norm = dir2.normalized;
                    Vector3 dir3 = new Vector3(dir2Norm.x, dir2Norm.y, 0);
                    Vector3 secondLinePos = firstLinePos + dir3 * dir2.magnitude;
                    lineRenderer.SetPositions(new[] { firstLinePos, secondLinePos });
                    lineRenderer.enabled = true;
                    if (!targetAcquired)
                    {
                        targetAcquiredTime = Time.time;
                    }
                    targetAcquired = true;
                }
                else
                {
                    targetAcquired = false;
                    targetAcquiredTime = 0;
                }
            }
            else
            {
                targetAcquired = false;
                targetAcquiredTime = 0;
            }

            if (!targetAcquired)
            {
                lineRenderer.enabled = false;
            }

            if (targetAcquired && Time.time - targetAcquiredTime > timeBeforeShoot)
            {
                targetAcquiredTime = 0f;
                // Debug.Log("SHOOTED");
                GameManager.main.TakeDamage();
                targetAcquired = false;
                shootSFX.PlayOneShot(shootSFX.clip);

                GameObject smoke = Instantiate(gunSmokePrefab);
                smoke.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                smoke.transform.position = -pipe.right * 0.5f + pipe.transform.position;// + pipe.localRotation * Vector2.up * 0.5f;
                lastShot = Time.time;
            }
        }
        else
        {
            targetAcquired = false;
            targetAcquiredTime = 0;
        }
    }
}
