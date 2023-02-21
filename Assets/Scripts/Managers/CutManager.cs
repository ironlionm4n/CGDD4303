/*
CUT MANAGER
Handles cutting down materials of default sizes into the size specified by the player
Inherits from UIManagerParent
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutManager : UIManagerParent
{
    [Header("Available QTY Text")]
    public TMP_Text qtyPlyText;
    public TMP_Text qty2x4Text;
    public TMP_Text qty2x6Text;
    public TMP_Text qty4x4Text;

    //These are dropdowns to prevent the player from cutting the material to longer than it is currently
    //4x4 includes a decimal input, but the ones place is still a dropdown for the same reason
    [Header("Size Inputs")]
    public TMP_Dropdown lengthPlyDropdown;
    public TMP_Dropdown widthPlyDropdown;
    public TMP_Dropdown length2x4Dropdown;
    public TMP_Dropdown length2x6Dropdown;
    public TMP_Dropdown length4x4Dropdown;
    public PositiveInputField length4x4Decimal;

    [Header("QTY Dropdowns")]
    public TMP_Dropdown amtPlyDropdown;
    public TMP_Dropdown amt2x4Dropdown;
    public TMP_Dropdown amt2x6Dropdown;
    public TMP_Dropdown amt4x4Dropdown;

    private TMP_Dropdown[] dd;
    private bool initialized = false;

    private int qtyPly;
    private int qty2x4;
    private int qty2x6;
    private int qty4x4;

    private Entry ePly, e2x4, e2x6, e4x4;

    private void Start()
    {
        Setup();
        UpdateQTYText();
        PopulateDDArray();
        ResetDropdowns();
        SetSizeDropdowns();
        SetAllQTYDropdowns();
        initialized = true;
    }

    private void OnEnable()
    {
        if (initialized)
        {
            UpdateQTYText();
            SetSizeDropdowns();
            SetAllQTYDropdowns();

            foreach (TMP_Dropdown d in dd)
            {
                d.value = 0;
            }
        }
    }

    /// <summary>
    /// Updates the quantity text with the correct amounts from the inventory
    /// </summary>
    private void UpdateQTYText()
    { 
        ePly = im.Contains(ConstructionMaterial.Type.Plywood, defaultPly);
        e2x4 = im.Contains(ConstructionMaterial.Type.Lumber2x4, default2x4);
        e2x6 = im.Contains(ConstructionMaterial.Type.Lumber2x6, default2x6);
        e4x4 = im.Contains(ConstructionMaterial.Type.Lumber4x4, default4x4);

        qtyPly = ePly != null ? ePly.Qty : 0;
        qty2x4 = e2x4 != null ? e2x4.Qty : 0;
        qty2x6 = e2x6 != null ? e2x6.Qty : 0;
        qty4x4 = e4x4 != null ? e4x4.Qty : 0;

        qtyPlyText.text = "x" + qtyPly;
        qty2x4Text.text = "x" + qty2x4;
        qty2x6Text.text = "x" + qty2x6;
        qty4x4Text.text = "x" + qty4x4;
    }

    /// <summary>
    /// Creates an array for all the dropdowns for easy iteration through them
    /// </summary>
    private void PopulateDDArray()
    {
        dd = new TMP_Dropdown[9];
        dd[0] = lengthPlyDropdown;
        dd[1] = widthPlyDropdown;
        dd[2] = length2x4Dropdown;
        dd[3] = length2x6Dropdown;
        dd[4] = length4x4Dropdown;
        dd[5] = amtPlyDropdown;
        dd[6] = amt2x4Dropdown;
        dd[7] = amt2x6Dropdown;
        dd[8] = amt4x4Dropdown;
    }

    /// <summary>
    /// Sets all the possible sizes to cut in the dropdowns
    /// </summary>
    private void SetSizeDropdowns()
    {
        //For plywood, we let them go up to the default size because they may only want to cut it on one side
        //For the others, we go one less, because otherwise they'd be cutting to the size it is already
        SetDropdownValues((int)defaultPly.x, lengthPlyDropdown, false, 1);
        SetDropdownValues((int)defaultPly.z, widthPlyDropdown, false, 1);
        SetDropdownValues((int)default2x4.z - 1, length2x4Dropdown, false, 1);
        SetDropdownValues((int)default2x6.z - 1, length2x6Dropdown, false, 1);
        SetDropdownValues((int)default4x4.z - 1, length4x4Dropdown, false, 1);
    }

    /// <summary>
    /// Sets the quantity dropdowns for all the material types
    /// </summary>
    private void SetAllQTYDropdowns()
    {
        SetDropdownValues(qtyPly, amtPlyDropdown, true);
        SetDropdownValues(qty2x4, amt2x4Dropdown, true);
        SetDropdownValues(qty2x6, amt2x6Dropdown, true);
        SetDropdownValues(qty4x4, amt4x4Dropdown, true);
    }

    /// <summary>
    /// Sets a dropdown with numerical options
    /// </summary>
    /// <param name="value">The highest value to reach</param>
    /// <param name="d">The dropdown to set</param>
    /// <param name="ascending">If the options are going from low to high or not</param>
    /// <param name="min">The minimum value</param>
    private void SetDropdownValues(int value, TMP_Dropdown d, bool ascending, int min = 0)
    {
        d.ClearOptions();

        if (ascending)
        {
            for(int i = min; i <= value; i++)
            {
                d.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
            }
        }
        else
        {
            for(int i = value; i >= min; i--)
            {
                d.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
            }
        }

        d.value = 0;
    }

    /// <summary>
    /// Calculates the new sizes and quantities of items and adds them to the inventory
    /// </summary>
    public void CutItems()
    {
        //Plywood
        Vector3 sizePly = new Vector3(DropdownToNum(lengthPlyDropdown, (int)defaultPly.x, 1, false), defaultPly.y, DropdownToNum(widthPlyDropdown, (int)defaultPly.z, 1, false));
        int numPly = DropdownToNum(amtPlyDropdown, qtyPly, 0, true);
        ChangeInventory(ePly, sizePly, numPly);
        //Waste is in square feet
        float wastePly = ((defaultPly.x * defaultPly.z) - (sizePly.x * sizePly.z)) * numPly;

        //2x4
        Vector3 size2x4 = new Vector3(default2x4.x, default2x4.y, DropdownToNum(length2x4Dropdown, (int)default2x4.z - 1, 1, false));
        int num2x4 = DropdownToNum(amt2x4Dropdown, qty2x4, 0, true);
        ChangeInventory(e2x4, size2x4, num2x4);
        float waste2x4 = (default2x4.z - size2x4.z) * num2x4;

        //2x6
        Vector3 size2x6 = new Vector3(default2x6.x, default2x6.y, DropdownToNum(length2x6Dropdown, (int)default2x6.z - 1, 1, false));
        int num2x6 = DropdownToNum(amt2x6Dropdown, qty2x6, 0, true);
        ChangeInventory(e2x6, size2x6, num2x6);
        float waste2x6 = (default2x6.z - size2x6.z) * num2x6;

        //4x4
        Vector3 size4x4 = new Vector3(default4x4.x, default4x4.y, float.Parse(DropdownToNum(length4x4Dropdown, (int)default4x4.z - 1, 1, false) + "." + length4x4Decimal.text));
        int num4x4 = DropdownToNum(amt4x4Dropdown, qty4x4, 0, true);
        ChangeInventory(e4x4, size4x4, num4x4);
        float waste4x4 = (default4x4.z - size4x4.z) * num4x4;

        gm.CutCheckout(wastePly, waste2x4, waste2x6, waste4x4);
        LeaveCutting();
    }

    /// <summary>
    /// Changes the inventory quantities after cutting
    /// </summary>
    /// <param name="use">The entry to use a quantity of</param>
    /// <param name="size">The size of the material after cutting</param>
    /// <param name="amt">How many to cut</param>
    private void ChangeInventory(Entry use, Vector3 size, int amt)
    {
        if(amt > 0)
        {
            im.UseItem(use, amt);
            Entry add = new Entry(new ConstructionMaterial(use.Material.MaterialType, size), amt);
            im.AddEntry(add);
        }
    }

    /// <summary>
    /// Converts a dropdown's index value to a number
    /// </summary>
    /// <param name="d">Dropdown</param>
    /// <param name="max">Largest number</param>
    /// <param name="min">Smallest number</param>
    /// <param name="ascending">If the dropdown is low to high or not</param>
    /// <returns>Number corresponding to dropdown's value</returns>
    private int DropdownToNum(TMP_Dropdown d, int max, int min, bool ascending)
    {
        if (ascending)
        {
            return (d.value + min);
        }
        else
        {
            return (max - d.value);
        }
    }

    /// <summary>
    /// Resets all dropdowns and returns to the main screen
    /// </summary>
    public void LeaveCutting()
    {
        ResetDropdowns();

        mm.SwitchState(MenuManager.State.Main);
    }

    /// <summary>
    /// Resets all the dropdowns and inputs to their default states
    /// </summary>
    public void ResetDropdowns()
    {
        //The label is only updated when the value changes, so we set it to 1 here then 0 when the cutting appears again
        //Adding in the "FILLER" label lets you change the value to 1, because if there's only 1 option the index won't change from 0
        //Without this, the dropdown shows up as blank instead of its 0 label
        foreach (TMP_Dropdown d in dd)
        {
            if (d.options.Count == 1)
            {
                d.options.Add(new TMP_Dropdown.OptionData("FILLER"));
            }
            d.value = 1;
        }

        length4x4Decimal.ClearText();
    }
}
