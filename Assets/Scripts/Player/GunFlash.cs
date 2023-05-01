using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFlash : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();
    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    private float started = 0f;
    private float spriteTime = 0.32f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Mathf.Min(currentSpriteIndex, sprites.Count - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.main.GameOver) return;

        if (currentSpriteIndex < sprites.Count)
        {
            spriteRenderer.sprite = sprites[Mathf.Min(currentSpriteIndex, sprites.Count - 1)];
            if (Time.time - started > spriteTime * currentSpriteIndex)
            {
                currentSpriteIndex++;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
