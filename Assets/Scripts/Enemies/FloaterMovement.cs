using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float sin0_1 = (Mathf.Sin(Time.time * 2f) + 1f) / 2f;
        transform.localPosition = new Vector2(0, Mathf.Lerp(-0.1f, 0.1f, sin0_1));
    }
}
