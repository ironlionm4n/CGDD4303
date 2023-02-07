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
        Cursor.lockState = CursorLockMode.Confined;
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

        transform.position =
            new Vector3(transform.position.x, Mathf.Max(2f, transform.position.y), transform.position.z);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivity;
            rotationY += Input.GetAxis("Mouse Y") * -1 * sensitivity;
            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0f);
        }
    }
}