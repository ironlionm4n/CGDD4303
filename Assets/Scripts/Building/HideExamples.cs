/*
HIDE EXAMPLES
Attached to the parents of each layer in the example build
Handles hiding and changing the appearance of objects
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideExamples : MonoBehaviour
{
    public MeshRenderer[] rends;
    public Material baseMat;
    public Material hiddenMat;
    public GameObject[] disable;

    /// <summary>
    /// Sets all children to the opaque material
    /// </summary>
    public void Show()
    {
        foreach(MeshRenderer r in rends)
        {
            r.enabled = true;
            r.material = baseMat;
        }
        foreach(GameObject g in disable)
        {
            g.SetActive(true);
        }
    }

    /// <summary>
    /// Sets all children to the transparent material
    /// </summary>
    public void Transparent()
    {
        foreach(MeshRenderer r in rends)
        {
            r.enabled = true;
            r.material = hiddenMat;
        }
        foreach(GameObject g in disable)
        {
            g.SetActive(true);
        }
    }

    /// <summary>
    /// Sets all children to invisible and turns off the colliders
    /// </summary>
    public void Hide()
    {
        foreach(MeshRenderer r in rends)
        {
            r.enabled = false;
        }
        foreach (GameObject g in disable)
        {
            g.SetActive(false);
        }
    }
}
