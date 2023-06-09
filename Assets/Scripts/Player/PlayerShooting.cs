using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    private GameObject gunSmokePrefab;
    [SerializeField]
    private GameObject shotHitPrefab;
    [SerializeField]
    private AudioSource shotSound;
    // [SerializeField]
    // private Transform gunPoint;

    private PlayerAiming aiming;

    private float shootCD = 1f;
    private float lastShot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        aiming = GetComponent<PlayerAiming>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.main.GameOver) return;

        Transform gunPoint = aiming.CurrentGunpoint;
        // Vector2 rayOrigin2 = new Vector2(gunPoint.position.x, gunPoint.position.y);
        // 
        // float angleRadians2 = aiming.ShootAngle * Mathf.Deg2Rad;
        // Vector2 rayDir2 = new Vector2(Mathf.Cos(angleRadians2), Mathf.Sin(angleRadians2));
        // Debug.DrawRay(rayOrigin2, rayDir2);

        bool shootOffCD = Time.time - lastShot > shootCD;
        if(Input.GetKeyDown(KeyCode.Mouse0) && shootOffCD)
        {
            shotSound.PlayOneShot(shotSound.clip);
            lastShot = Time.time;
            GameObject smoke = Instantiate(gunSmokePrefab);
            smoke.transform.rotation = Quaternion.AngleAxis(aiming.ShootAngle, Vector3.forward);
            smoke.transform.position = aiming.CurrentGunpoint.position;

            Vector2 rayOrigin = new Vector2(gunPoint.position.x, gunPoint.position.y);

            float angleRadians = aiming.ShootAngle * Mathf.Deg2Rad;
            Vector2 rayDir = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
            int shootMask = LayerMask.GetMask(new[] { "Ground", "Enemy" });

            RaycastHit2D hitInfo = Physics2D.Raycast(rayOrigin, rayDir, 10f, shootMask);

            // Debug.DrawRay(rayOrigin, rayDir);

            if (hitInfo.collider != null)
            {
                GameObject hitParticle = Instantiate(shotHitPrefab);
                hitParticle.transform.rotation = Quaternion.AngleAxis(aiming.ShootAngle - 180, Vector3.forward);
                hitParticle.transform.position = hitInfo.point;

                if (hitInfo.transform.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.TakeDamage();
                }
            }
        }
    }
}
