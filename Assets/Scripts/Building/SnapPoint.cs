/*
SNAP POINT
A transform for Preview objects to attach to when they get close enough
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public float snapDistance = 2;
    //Parent prevents previews from trying to snap to themselves
    public GameObject parent;

    /// <summary>
    /// Checks if the given position is within range of the snap point
    /// </summary>
    /// <param name="pos">Position to check</param>
    /// <returns>If in range or not</returns>
    public bool InRange(Vector3 pos)
    {
        return (Vector3.Distance(pos, transform.position) <= snapDistance);
    }
}
