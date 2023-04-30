using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mailbox : MonoBehaviour
{
    [SerializeField]
    private Sprite deliveredSprite;

    private SpriteRenderer spriteRenderer;
    private bool active = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && active)
        {
            active = false;
            GameManager.main.Deliver();
            spriteRenderer.sprite = deliveredSprite;
        }
    }
}
