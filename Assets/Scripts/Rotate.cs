using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private void OnEnable()
    {
        transform.Rotate(45, 0, 0);
    }
}
