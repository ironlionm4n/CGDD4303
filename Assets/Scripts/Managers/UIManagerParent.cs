/*
UI MANAGER PARENT
A class for other scripts that deal heavily with the UI to inherit from
Sets text labels to show default sizes for each material
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerParent : MonoBehaviour
{
    [Header("Size Labels")]
    public TMP_Text textPly;
    public TMP_Text text2x4;
    public TMP_Text text2x6;
    public TMP_Text text4x4;
    public TMP_Text textStrut;
    public TMP_Text textTie;
    public TMP_Text textClamp;

    private static BuildManager buildManager;
    
    protected InventoryManager im;
    protected GradeManager gm;
    protected MenuManager mm;

    private const float FOUR_INCHES = 4f / 12f;

    //Purely for readability so the variable names are shorter
    protected ConstructionMaterial.Type ply = ConstructionMaterial.Type.Plywood;
    protected ConstructionMaterial.Type lumber2x4 = ConstructionMaterial.Type.Lumber2x4;
    protected ConstructionMaterial.Type lumber2x6 = ConstructionMaterial.Type.Lumber2x6;
    protected ConstructionMaterial.Type lumber4x4 = ConstructionMaterial.Type.Lumber4x4;
    protected ConstructionMaterial.Type tie = ConstructionMaterial.Type.Tie;
    protected ConstructionMaterial.Type strut = ConstructionMaterial.Type.Strut;
    protected ConstructionMaterial.Type stud = ConstructionMaterial.Type.Stud;
    protected ConstructionMaterial.Type clamp = ConstructionMaterial.Type.Clamp;
    protected Vector3 defaultPly, default2x4, default2x6, default4x4, defaultStrut, defaultTie, defaultClamp;

    /// <summary>
    /// Initializes the important values for the children
    /// </summary>
    protected void Setup()
    {
        GameObject manager = GameObject.FindWithTag("BuildManager");
        buildManager = manager.GetComponent<BuildManager>();
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

        if (buildManager.FormworkType == FormWorkType.Wall)
        {
            defaultStrut = BuildSystem.GetDefaultSize(strut);
        }
        else
        {
            defaultStrut = new Vector3(FOUR_INCHES, FOUR_INCHES, 12f);
        }

        defaultTie = BuildSystem.GetDefaultSize(tie);
        defaultClamp = BuildSystem.GetDefaultSize(clamp);
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

        if (textPly!= null)
        {
            textPly.text = ConstructionMaterial.SizeToText(ply, defaultPly, true);
        }

        if (text2x4 != null)
        {
            text2x4.text = ConstructionMaterial.SizeToText(lumber2x4, default2x4, true);
        }

        if (text2x6!= null)
        {
            text2x6.text = ConstructionMaterial.SizeToText(lumber2x6, default2x6, true);
        }

        if (text4x4 != null)
        {
            text4x4.text = ConstructionMaterial.SizeToText(lumber4x4, default4x4, true);
        }

        if(textStrut != null)
        {
            textStrut.text = ConstructionMaterial.SizeToText(strut, buildManager.FormworkType == FormWorkType.Column ? new Vector3(defaultStrut.x, defaultStrut.y, 12f) : defaultStrut, true);
        }

        if(textTie != null)
        {
            textTie.text = ConstructionMaterial.SizeToText(tie, defaultTie, true);
        }

        if(textClamp != null)
        {
            textClamp.text = ConstructionMaterial.SizeToText(clamp, defaultClamp, true);
        }
    }
}
