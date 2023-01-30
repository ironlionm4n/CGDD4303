/*
CAMERA CONTROLLER
Attached to an object with the camera as a child
Handles rotation and zoom of the camera
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform lookPos;
    [Header("Rotation")]
    public KeyCode rotKey = KeyCode.LeftShift;
    public KeyCode rotKey2 = KeyCode.Mouse1;
    public float rotSpeed;
    public Vector2 rotClamp;

    [Header("Zoom")]
    public KeyCode zoomKey = KeyCode.LeftControl;
  
    public float zoomSpeed;
    public Vector2 zoomClamp;


    private Transform cam;
    private Vector3 startPoint;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>().transform;
        cam.LookAt(lookPos);
    }

    void Update()
    {
        //Rotation
        if (Input.GetKey(rotKey) || Input.GetKey(rotKey2))
        {
            float sideways = Input.GetAxis("Mouse X") * rotSpeed;
            float vertical = Input.GetAxisRaw("Mouse Y") * rotSpeed;

            transform.Rotate(vertical, sideways, 0f);
            Vector3 degrees = transform.rotation.eulerAngles;
            degrees = new Vector3(Mathf.Clamp(degrees.x > 180 ? degrees.x - 360 : degrees.x, rotClamp.x, rotClamp.y), degrees.y, degrees.z);
            transform.rotation = Quaternion.Euler(degrees);

            cam.LookAt(lookPos);
        }
        //Zoom
        else if (Input.GetKey(zoomKey))
        {
            float move = Input.GetAxisRaw("Mouse Y") * zoomSpeed;

            //Zoom is done by moving the camera forward/backwards
            cam.localPosition += new Vector3(0f, 0f, move);

            if (Mathf.Abs(cam.localPosition.z) > Mathf.Abs(zoomClamp.y))
            {
                cam.localPosition += new Vector3(0f, 0f, -cam.localPosition.z + zoomClamp.y);
            }

            if (Mathf.Abs(cam.localPosition.z) < Mathf.Abs(zoomClamp.x))
            {
                cam.localPosition += new Vector3(0f, 0f, -cam.localPosition.z + zoomClamp.x);
            }
        }
    }

}
