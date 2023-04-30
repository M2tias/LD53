using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var v = rb.velocity;
        // var floatV = Mathf.Lerp(-1f, 1f, Mathf.Sin(Time.time));
        float sin0_1 = (Mathf.Sin(Time.time * 2f) + 1f) / 2f;
        rb.velocity = new Vector2(v.x, Mathf.Lerp(-0.66f, 0.66f, sin0_1));
        Debug.Log(rb.velocity);
    }
}
