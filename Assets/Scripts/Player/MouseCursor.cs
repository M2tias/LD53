using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.main.GameOver) return;
        Vector2 m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(m.x, m.y, 0);
    }
}
