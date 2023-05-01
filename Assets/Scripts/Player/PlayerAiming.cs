using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField]
    List<AimingSprite> upperTorsoSprites;

    [SerializeField]
    SpriteRenderer upperTorsoSprite;
    [SerializeField]
    SpriteRenderer lowerTorsoSprite;

    public bool IsFlipped { get; private set; }
    public Transform CurrentGunpoint { get; private set; }
    public float ShootAngle { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.main.GameOver) return;

        Vector2 m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos = transform.position;
        Vector2 direction = m - new Vector2(pos.x, pos.y);
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        ShootAngle = angle;

        foreach (var aimingSprite in upperTorsoSprites)
        {
            if (aimingSprite.MinAngle < angle && aimingSprite.MaxAngle > angle || aimingSprite.MinAngle == angle) {
                upperTorsoSprite.sprite = aimingSprite.Sprite;
                upperTorsoSprite.flipX = aimingSprite.Flipped;
                lowerTorsoSprite.flipX = aimingSprite.Flipped;
                IsFlipped = aimingSprite.Flipped;
                CurrentGunpoint = aimingSprite.GunPoint;
                break;
            }
        }
    }
}

[Serializable]
class AimingSprite
{
    public float MinAngle;
    public float MaxAngle;
    public Sprite Sprite;
    public bool Flipped;
    public Transform GunPoint;
}