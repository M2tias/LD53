using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    private GameObject gunSmokePrefab;
    [SerializeField]
    private Transform gunPoint;

    private PlayerAiming aiming;

    // Start is called before the first frame update
    void Start()
    {
        aiming = GetComponent<PlayerAiming>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject smoke = Instantiate(gunSmokePrefab);
            smoke.transform.rotation = Quaternion.AngleAxis(aiming.ShootAngle, Vector3.forward);
            smoke.transform.position = aiming.CurrentGunpoint.position;
        }
    }
}
