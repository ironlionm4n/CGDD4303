/*
BUILD SYSTEM
Creates previews, moves them, then builds them as the actual objects
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    [Header("Controls")]
    public KeyCode rotateKey = KeyCode.R;
    public KeyCode cancelKey = KeyCode.G;
    public KeyCode buildKey = KeyCode.Mouse0;
    public KeyCode deleteKey = KeyCode.D;

    [Header("Building Modifiers")]
    public float rotateAmount = 90f;
    public float snapTolerance = 1.5f;

    [Header("Raycasting")]
    public LayerMask deletionLayers;
    public Camera cam;

    [Header("Material Default Sizes")]
    public Vector2 defaultPlywoodSize = new Vector2(8, 4);
    public float default2x4Size = 15;
    public float default2x6Size = 16;
    public float default4x4Size = 8.482f;

    private GameObject previewGameObject = null;
    private Quaternion previewRot = Quaternion.identity;
    private Preview previewScript = null;

    private const float TWO_INCHES = 2f / 12f;
    private const float FOUR_INCHES = 4f / 12f;
    private const float SIX_INCHES = 6f / 12f;
    private const float THREE_QUARTERS_INCH = .75f / 12f;

    private static Vector3 defaultPly;
    private static Vector3 default2x4;
    private static Vector3 default2x6;
    private static Vector3 default4x4;
    private static Vector3 defaultStrut;

    private bool isBuilding = false;
    private bool moveWithMouse = true;
    private List<BuiltObject> allBuilds;
    private BuiltObject deletionObject;

    private InventoryManager im;

    /// <summary>
    /// Sets the static default sizes and initializes variables
    /// </summary>
    private void Start()
    {
        im = GetComponent<InventoryManager>();
        allBuilds = new List<BuiltObject>();

        defaultPly = new Vector3(Mathf.Max(defaultPlywoodSize.x, defaultPlywoodSize.y), THREE_QUARTERS_INCH, Mathf.Min(defaultPlywoodSize.x, defaultPlywoodSize.y));
        default2x4 = new Vector3(TWO_INCHES, FOUR_INCHES, default2x4Size);
        default2x6 = new Vector3(TWO_INCHES, SIX_INCHES, default2x6Size);
        default4x4 = new Vector3(FOUR_INCHES, FOUR_INCHES, default4x4Size);
        defaultStrut = new Vector3(FOUR_INCHES, FOUR_INCHES, 20);
    }

    private void Update()
    {
        //Rotate
        if (Input.GetKeyDown(rotateKey))
        {
            if(previewGameObject != null)
            {
                previewGameObject.transform.Rotate(0, rotateAmount, 0);
                previewRot = previewGameObject.transform.rotation;
            }
        }

        //Cancel
        if (Input.GetKeyDown(cancelKey))
        {
            CancelBuild();
        }

        //Change appearance to delete
        if (Input.GetKeyDown(deleteKey))
        {
            CancelBuild();
            foreach(BuiltObject b in allBuilds)
            {
                b.ChangeMaterial(true);
            }
        }

        //Select potential delete object
        if (Input.GetKey(deleteKey))
        {
            //If there's already a selected object, deselect it
            deletionObject?.ChangeMaterialSelection(false);

            //Raycast to find what the mouse is pointing at
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, deletionLayers))
            {
                deletionObject = hit.collider.GetComponentInParent<BuiltObject>();
                deletionObject?.ChangeMaterialSelection(true);
            }
            else
            {
                deletionObject = null;
            }

            //Do the deletion if possible
            if (deletionObject != null && Input.GetKeyDown(buildKey))
            {
                allBuilds.Remove(deletionObject);
                deletionObject.Delete();
                deletionObject = null;
            }
            
        }

        //Change appearance to not-delete
        if (Input.GetKeyUp(deleteKey))
        {
            foreach(BuiltObject b in allBuilds)
            {
                b.ChangeMaterial(false);
            }
        }

        //Check for building
        if (isBuilding)
        {
            //Build if there is a preview
            if (Input.GetKeyDown(buildKey))
            {
                if(previewScript != null)
                {
                    BuildObjectInWorld();
                }
            }

            //If you just built, there won't be anything to check
            if(previewGameObject != null)
            {
                if (!moveWithMouse)
                {
                    //Cast these to the same space so they can be compared
                    Vector3 screenPoint = cam.WorldToScreenPoint(previewGameObject.transform.position);
                    Vector3 mousePoint = Input.mousePosition;

                    if (Vector3.Distance(screenPoint, mousePoint) >= snapTolerance)
                    {
                        previewScript.UnsnapFromPoint();
                        //Move once here so the preview doesn't immediately snap itself back into place
                        MoveWithRaycast();
                        ToggleMoveWithMouse(true);
                    }
                }
                else
                {
                    MoveWithRaycast();
                }
            }
        }

    }


    /// <summary>
    /// Creates a preview object for placement
    /// </summary>
    public void CreatePreview(Preview preview, ConstructionMaterial t, int position)
    {
        //If there's already a preview object, destroy it
        //That way you can switch without having to press cancel each time
        if(previewGameObject != null)
        {
            Destroy(previewGameObject);

            //Only reflect the previous rotation if this is the same type of game object
            //There are some edge cases where this won't work but close enough
            previewRot = new Quaternion(preview.transform.rotation.x, preview.transform.rotation.y, preview.transform.rotation.z, preview.transform.rotation.w);
        }

        //Puts the new preview at the mouse's position
        Vector3 startPos = new Vector3();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.Log(preview);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, preview.raycastLayers))
        {
            startPos = hit.point;
        }

        //Sets all the necessary variables
        previewScript = Instantiate(preview, startPos, previewRot);
        previewGameObject = previewScript.gameObject;
        previewScript.Mat = t;
        previewScript.SetPosition(position);

        if (t.MaterialType != ConstructionMaterial.Type.Tie)
        {
            previewScript.Resize(t.Size);
        }
        isBuilding = true;
    }
    public void CreatePreview(Preview preview, ConstructionMaterial t, int position, Quaternion previewRotation)
    {
        //If there's already a preview object, destroy it
        //That way you can switch without having to press cancel each time
        if(previewGameObject != null)
        {
            Destroy(previewGameObject);

            //Only reflect the previous rotation if this is the same type of game object
            //There are some edge cases where this won't work but close enough
            //previewRot = new Quaternion(preview.transform.rotation.x, preview.transform.rotation.y, preview.transform.rotation.z, preview.transform.rotation.w);
        }
        previewRot = previewRotation;

        //Puts the new preview at the mouse's position
        Vector3 startPos = new Vector3();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.Log(preview);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, preview.raycastLayers))
        {
            startPos = hit.point;
        }

        //Sets all the necessary variables
        previewScript = Instantiate(preview, startPos, previewRot);
        previewGameObject = previewScript.gameObject;
        previewScript.Mat = t;
        previewScript.SetPosition(position);

        if (t.MaterialType != ConstructionMaterial.Type.Tie)
        {
            previewScript.Resize(t.Size);
        }
        isBuilding = true;
    }

    /// <summary>
    /// Cancels the current build and removes the preview object
    /// </summary>
    public void CancelBuild()
    {
        Destroy(previewGameObject);
        previewGameObject = null;
        previewRot = Quaternion.identity;
        previewScript = null;
        isBuilding = false;
    }

    /// <summary>
    /// Places a built object at the same position as the preview
    /// </summary>
    private void BuildObjectInWorld()
    {
        bool success = previewScript.Place();
        if (success)
        {
            previewGameObject = null;
            previewScript = null;
            isBuilding = false;
            ToggleMoveWithMouse(true);
        }
        
    }

    /// <summary>
    /// Allows changing of whether or not the preview object should move with the mouse's movement
    /// </summary>
    /// <param name="moveWithMouse">Whether or not to move with the mouse</param>
    public void ToggleMoveWithMouse(bool moveWithMouse)
    {
        this.moveWithMouse = moveWithMouse;
    }

    /// <summary>
    /// Casts a ray to move the preview object properly to the right position
    /// </summary>
    private void MoveWithRaycast()
    {
        if(previewGameObject != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, previewScript.raycastLayers))
            {
                previewGameObject.transform.position = hit.point;
            }
        }
        
    }

    /// <summary>
    /// If there is currently a preview
    /// </summary>
    public bool IsBuilding
    {
        get { return isBuilding; }
    }

    /// <summary>
    /// Returns the default, purchasable size of the material
    /// </summary>
    /// <param name="t">The type of material to check</param>
    /// <returns>Material default size</returns>
    public static Vector3 GetDefaultSize(ConstructionMaterial.Type t)
    {
        switch (t)
        {
            case (ConstructionMaterial.Type.Plywood):
                return defaultPly;
            case (ConstructionMaterial.Type.Lumber2x4):
                return default2x4;
            case (ConstructionMaterial.Type.Lumber2x6):
                return default2x6;
            case (ConstructionMaterial.Type.Lumber4x4):
                return default4x4;
            case (ConstructionMaterial.Type.Strut):
                return defaultStrut;
            default:
                return Vector3.zero;
        }
    }

    /// <summary>
    /// Adds a built object to the list
    /// </summary>
    /// <param name="b">The built object to add</param>
    public void AddBuiltObject(BuiltObject b)
    {
        allBuilds.Add(b);
    }

}
