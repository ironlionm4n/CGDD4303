/*
PREVIEW
A preview object that shows the player what they will be placing
Handles snapping and creating the new built object
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{
    [Header("Building")]
    public BuiltObject builtVersion;
    public PositionMesh mesh;

    [Header("Movement and Snapping")]
    public LayerMask raycastLayers;
    public Material snappedMat;
    public Material unsnappedMat;
    public List<string> snapTags = new List<string>();

    private BuildSystem buildSys;
    private BuildManager buildM;
    private InventoryManager im;
    private int position;
    private MeshRenderer rend;
    private ConstructionMaterial mat;
    private bool isSnapped = false;

    private List<SnapPoint> availableSnaps = new List<SnapPoint>();
    
    private void Start()
    {
        GameObject buildManager = GameObject.FindGameObjectWithTag("BuildManager");
        buildSys = buildManager.GetComponent<BuildSystem>();
        buildM = buildManager.GetComponent<BuildManager>();
        im = buildManager.GetComponent<InventoryManager>();
        rend = GetComponent<MeshRenderer>();
        if(rend == null)
        {
            rend = GetComponentInChildren<MeshRenderer>();
        }
        ChangeColor();

        //Gets all the snap points in the scene that could be snapped to
        for(int i = 0; i < snapTags.Count; i++)
        {
            GameObject[] potentialSnaps = GameObject.FindGameObjectsWithTag(snapTags[i]);
            for(int j = 0; j < potentialSnaps.Length; j++)
            {
                SnapPoint newSnap = potentialSnaps[j].GetComponent<SnapPoint>();
                if(newSnap.parent == null)
                {
                    availableSnaps.Add(newSnap);
                }
                else if(newSnap.parent != gameObject)
                {
                    availableSnaps.Add(newSnap);
                }
            }
        }
    }

    private void Update()
    {
        if (!isSnapped)
        {
            foreach(SnapPoint sp in availableSnaps)
            {
                if (sp.InRange(transform.position))
                {
                    SnapToPoint(sp.transform.position, sp);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Builds the object in the world and removes the preview
    /// </summary>
    /// <returns>Whether or not the placement was successful</returns>
    public bool Place()
    {
        if(isSnapped)
        {
            BuiltObject built = Instantiate(builtVersion, transform.position, transform.rotation);
            built.Resize(mesh.transform.localScale);
            built.Mat = mat;
            
            buildSys.AddBuiltObject(built);
            Destroy(gameObject);

            //If the inventory still has some of this type left
            bool remaining = im.UseItem(position);
            if (remaining)
            {
                Entry e = im.Inventory[position];
                buildM.BuildGivenItemAfterFrame(e.Material, position);
            }
            else
            {
                im.OutOfEntry();
            }
            
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Changes between snappedMat and unsnappedMat according to if the object is snapped or not
    /// </summary>
    private void ChangeColor()
    {
        if (isSnapped)
        {
            rend.material = snappedMat;
        }
        else
        {
            rend.material = unsnappedMat;
        }
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
    /// "Snaps" the position onto the snap point
    /// </summary>
    /// <param name="snapPos">Position of the snap point</param>
    public void SnapToPoint(Vector3 snapPos, SnapPoint sp)
    {
        buildSys.ToggleMoveWithMouse(false);
        transform.position = snapPos;
        transform.rotation = sp.transform.rotation;
        isSnapped = true;
        ChangeColor();
    }

    /// <summary>
    /// "Unsnaps" the position from any snap points
    /// </summary>
    public void UnsnapFromPoint()
    {
        isSnapped = false;
        ChangeColor();
    }

    /// <summary>
    /// If the object is snapped to a point
    /// </summary>
    public bool IsSnapped
    {
        get { return isSnapped; }
    }

    /// <summary>
    /// Sets the position in the inventory
    /// Used for inventory management
    /// </summary>
    /// <param name="i">Inventory index</param>
    public void SetPosition(int i)
    {
        position = i;
    }

    /// <summary>
    /// The ConstructionMaterial associated with this object
    /// </summary>
    public ConstructionMaterial Mat
    {
        get { return mat; }
        set { mat = value; }
    }
}
