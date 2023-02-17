using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float xRotateAmount = 0;
    [SerializeField] private float yRotateAmount = 0;
    [SerializeField] private float zRotateAmount = 0;
    private void OnEnable()
    {
        transform.Rotate(xRotateAmount, yRotateAmount, zRotateAmount);
    }
}
