/*
UI MANAGER PARENT
A class for other scripts that deal heavily with the UI to inherit from
Sets text labels to show default sizes for each material
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerParent : MonoBehaviour
{
    [Header("Size Labels")]
    public Text textPly;
    public Text text2x4;
    public Text text2x6;
    public Text text4x4;
    
    protected InventoryManager im;
    protected GradeManager gm;
    protected MenuManager mm;

    //Purely for readability so the variable names are shorter
    protected ConstructionMaterial.Type ply = ConstructionMaterial.Type.Plywood;
    protected ConstructionMaterial.Type lumber2x4 = ConstructionMaterial.Type.Lumber2x4;
    protected ConstructionMaterial.Type lumber2x6 = ConstructionMaterial.Type.Lumber2x6;
    protected ConstructionMaterial.Type lumber4x4 = ConstructionMaterial.Type.Lumber4x4;
    protected ConstructionMaterial.Type tie = ConstructionMaterial.Type.Tie;
    protected ConstructionMaterial.Type strut = ConstructionMaterial.Type.Strut;
    protected Vector3 defaultPly, default2x4, default2x6, default4x4, defaultStrut;

    /// <summary>
    /// Initializes the important values for the children
    /// </summary>
    protected void Setup()
    {
        GameObject manager = GameObject.FindWithTag("BuildManager");
        im = manager.GetComponent<InventoryManager>();
        gm = manager.GetComponent<GradeManager>();
        mm = manager.GetComponent<MenuManager>();
        SetDefaultSizes();
        SetLabels();
    }

    /// <summary>
    /// Stores the default sizes for each construction material
    /// </summary>
    protected void SetDefaultSizes()
    {
        defaultPly = BuildSystem.GetDefaultSize(ply);
        default2x4 = BuildSystem.GetDefaultSize(lumber2x4);
        default2x6 = BuildSystem.GetDefaultSize(lumber2x6);
        default4x4 = BuildSystem.GetDefaultSize(lumber4x4);
        defaultPly = BuildSystem.GetDefaultSize(strut);
    }

    /// <summary>
    /// Sets the labels to reflect the current default sizes
    /// </summary>
    protected void SetLabels()
    {
        if (defaultPly == null || defaultPly.Equals(Vector3.zero))
        {
            SetDefaultSizes();
        }
        textPly.text = ConstructionMaterial.SizeToText(ply, defaultPly, true);
        text2x4.text = ConstructionMaterial.SizeToText(lumber2x4, default2x4, true);
        text2x6.text = ConstructionMaterial.SizeToText(lumber2x6, default2x6, true);
        text4x4.text = ConstructionMaterial.SizeToText(lumber4x4, default4x4, true);
    }
}
