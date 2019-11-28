using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = false;

    public float speed = 50.0f;
    public float rotationDirection = 1.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotateX)
        {
            transform.Rotate(Vector3.right * speed * rotationDirection  * Time.fixedDeltaTime);
        }

        if (rotateY)
        {
            transform.Rotate(Vector3.up * speed * rotationDirection * Time.fixedDeltaTime);
        }

        if (rotateZ)
        {
            transform.Rotate(Vector3.forward * speed * rotationDirection * Time.fixedDeltaTime);
        }
    }
}