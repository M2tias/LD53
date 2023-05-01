using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : MonoBehaviour
{
    [SerializeField]
    private AudioSource explosionSFX;

    // Start is called before the first frame update
    void Start()
    {
        explosionSFX.PlayOneShot(explosionSFX.clip);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.tag == "Player")
            {
                GameManager.main.TakeDamage();
            }
        }
    }
}
