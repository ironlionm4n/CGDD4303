using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class CameraFly : MonoBehaviour
{
    [SerializeField] private float sensitivity = 10;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float sprintSpeed = 20f;
    [SerializeField] private float rotationX;
    [SerializeField] private float rotationY;

    private void Start()
    {
        rotationX = transform.eulerAngles.y;
        rotationY = transform.eulerAngles.x;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.RightShift))
        {
            transform.position += transform.right * horizontal * sprintSpeed * Time.deltaTime;
            transform.position += transform.forward * vertical * sprintSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.right * horizontal * speed * Time.deltaTime;
            transform.position += transform.forward * vertical * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Backslash))
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Return))
        {
            transform.position -= transform.up * speed * Time.deltaTime;
        }


        /*
        transform.position =
            new Vector3(Mathf.Max(-30f, transform.position.x), Mathf.Max(2f, transform.position.y),
                transform.position.z);
                */
        var currentPosition = transform.position;
        transform.position = new Vector3(Mathf.Clamp(currentPosition.x, -30, 30),
            Mathf.Clamp(currentPosition.y, 2f, 30f), Mathf.Clamp(currentPosition.z, -40, 40));

        if (Input.GetKey(KeyCode.Mouse1))
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivity;
            rotationY += Input.GetAxis("Mouse Y") * -1 * sensitivity;
            transform.eulerAngles = new Vector3(rotationY, rotationX, 0f);
        }
    }
}