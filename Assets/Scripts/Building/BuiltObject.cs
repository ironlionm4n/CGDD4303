/*
BUILT OBJECT
Attached to all "built" construction materials
Handles the appearance for deletion
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltObject : MonoBehaviour
{
    public PositionMesh mesh;
    public Material regularMat;
    public Material deletionMat;
    public Material selectedDeletionMat;

    private ConstructionMaterial mat;
    private MeshRenderer rend;
    private InventoryManager im;

    private void Start()
    {
        rend = mesh.GetComponent<MeshRenderer>();
        rend.material = regularMat;
        im = GameObject.FindWithTag("BuildManager").GetComponent<InventoryManager>();
    }

    /// <summary>
    /// Resizes the object to the given size
    /// </summary>
    /// <param name="s">Size</param>
    public void Resize(Vector3 s)
    {
        mesh.transform.localScale = s;
       mesh.Reposition();
    }

    /// <summary>
    /// Changes the appearance based on if the game is in deletion mode or not
    /// </summary>
    /// <param name="deleting">If the game is in deletion mode</param>
    public void ChangeMaterial(bool deleting)
    {
        if (deleting)
        {
            rend.material = deletionMat;
        }
        else
        {
            rend.material = regularMat;
        }
    }

    /// <summary>
    /// Changes the appearance based on if the object has been highlighted for deletion
    /// </summary>
    /// <param name="selected">If the object is highlighted or not</param>
    public void ChangeMaterialSelection(bool selected)
    {
        if (selected)
        {
            rend.material = selectedDeletionMat;
        }
        else
        {
            rend.material = deletionMat;
        }
    }

    /// <summary>
    /// The ConstructionMaterial associated with this object
    /// </summary>
    public ConstructionMaterial Mat
    {
        get { return mat; }
        set { mat = value; }
    }

    /// <summary>
    /// Deletes the object and returns quantity to the inventory
    /// </summary>
    public void Delete()
    {
        im.AddEntry(new Entry(mat, 1));
        Destroy(gameObject);
    }
}
