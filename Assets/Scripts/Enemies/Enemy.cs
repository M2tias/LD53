using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int HP;

    [SerializeField]
    private GameObject deathPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        HP--;
        if (HP <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        HP = 0;
        if (deathPrefab != null)
        {
            GameObject deathObj = Instantiate(deathPrefab);
            deathObj.transform.position = transform.position;
        }
        Destroy(gameObject);
    }
}
