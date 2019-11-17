using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform player;
    public GameObject intentedPivotPoint;

    float heading = 0;
    float tilt = 15;
    float camDist = 15;
    float playerHeight = 3;

    private Camera camera;

    private void Start()
    {
        camera = gameObject.GetComponent<Camera>();
        Screen.lockCursor = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                player.gameObject.GetComponent<MovementBehavior>().SetIntendedPivotPointPosition(hit.point);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        heading += Input.GetAxis("Mouse X") * Time.deltaTime * 180;
        tilt += Input.GetAxis("Mouse Y") * Time.deltaTime * 180;
        tilt = Mathf.Clamp(tilt, -60, 60);
        transform.rotation = Quaternion.Euler(tilt, heading, 0);

        RaycastHit hit;
        bool cameraWallHit = Physics.Raycast(player.position + new Vector3(0.0f,2.5f,0.0f), transform.position - (player.position + new Vector3(0.0f, 2.5f, 0.0f)), out hit, 10.0f);
        //Debug.DrawRay(player.position + new Vector3(0.0f, 1.5f, 0.0f), transform.position - (player.position + new Vector3(0.0f, 1.5f, 0.0f)), Color.green);
        if (cameraWallHit)
        {
            camDist = Mathf.MoveTowards(camDist, Vector3.Distance(player.position, hit.point), 8.0f * Time.deltaTime);
            camDist = Mathf.Clamp(camDist, 1.3f, 10.0f);
        }
        else
        {
            camDist = Mathf.MoveTowards(camDist, 10.0f, 8.0f * Time.deltaTime);
        }
        transform.position = player.position - transform.forward * camDist + Vector3.up * playerHeight;
    }
}
