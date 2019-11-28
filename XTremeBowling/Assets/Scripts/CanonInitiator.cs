using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonInitiator : MonoBehaviour
{
    float t = 0;
    Vector3 startPosition;
    Vector3 startScale;
    public Vector3 target;
    float timeToReachTarget = 4.0f;
    public PosOrScale choice;

    void Start()
    {
        startPosition = transform.localPosition;
        startScale = transform.localScale;
    }
    void Update()
    {
        t += Time.deltaTime / timeToReachTarget;

        switch (choice)
        {
            case PosOrScale.pos:
                transform.localPosition = Vector3.Lerp(startPosition, target, t);
                if (transform.localPosition == target)
                {
                    GetComponent<Rotator>().enabled = false;
                }
                break;
            case PosOrScale.scale:
                transform.localScale = Vector3.Lerp(startScale, target, t);
                if (transform.localScale == target)
                {
                    GetComponent<Rotator>().enabled = false;
                }
                break;
        }
        
    }

    public enum PosOrScale
    {
        pos,
        scale
    }
}
