using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDoorBehavior : MonoBehaviour
{
    public Transform targetPosObject;

    bool openDoor = false;
    float doorSlidingSpeed = 20.0f;
    Vector3 targetPos;

    private void Start()
    {
        targetPos = targetPosObject.position;
    }

    private void Update()
    {
        if (!openDoor)
            return;
        else
            transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, targetPos, doorSlidingSpeed * Time.deltaTime);
    }

    public void openTheDoor()
    {
        openDoor = true;
        GetComponent<SphereCollider>().enabled = false;
    }
}
