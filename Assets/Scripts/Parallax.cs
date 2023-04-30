using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private float amountOfParallax;

    [SerializeField]
    Camera cam;

    private float startingPosX;

    // Start is called before the first frame update
    void Start()
    {
        startingPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = cam.transform.position;
        float temp = position.x * (1 - amountOfParallax);
        float distance = position.x * amountOfParallax;

        Vector3 newPosition = new Vector3(startingPosX + distance, transform.position.y, transform.position.z);

        transform.position = newPosition;
    }
}
