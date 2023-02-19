/*
POSITION MESH
Attached to the visible mesh children of each material - built, preview, etc
Resizes the mesh according to the ConstructionMaterial's size, and positions its center as well
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMesh : MonoBehaviour
{
    public enum CenterPosition { BottomLeftCenter, BottomCenterCenter, StrutCenter}
    public enum RotationAxis { X, Y, Z}

    public CenterPosition center;
    public RotationAxis axis;
    public bool longestOnZ = false;
    public bool useZToPosition = false;
    
    /// <summary>
    /// Positions a child relative to its parent for centering regardless of size
    /// </summary>
    public void Reposition()
    {
        float y = transform.localPosition.y;
        float x = 0f;
        float z = 0f;

        //Sets the y position according to how the mesh is oriented so its bottom rests at 0
        switch (axis)
        {
            case (RotationAxis.X):
                y = transform.localScale.z / 2;
                break;
            case (RotationAxis.Y):
                y = transform.localScale.y / 2;
                break;
            case (RotationAxis.Z):
                y = transform.localScale.x / 2;
                break;
        }

        //Sets the x and z according to if the mesh is centered in the bottom left or the bottom center
        switch (center)
        {
            case (CenterPosition.BottomLeftCenter):
                float modifier = useZToPosition ? transform.localScale.z / 2 : transform.localScale.x / 2;

                if (longestOnZ)
                {
                    z = modifier;
                }
                else
                {
                    x = modifier;
                }

                transform.localPosition = new Vector3(x, y, z);
                break;
            case (CenterPosition.BottomCenterCenter):
                transform.localPosition = new Vector3(0f, y, 0f);
                break;
            case (CenterPosition.StrutCenter):
                break;
        }

        
    }
}
