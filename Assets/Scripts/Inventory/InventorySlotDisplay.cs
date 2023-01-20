/*
INVENTORY SLOT DISPLAY
Sets the correct text for the type, quantity, and size of the material in an inventoyr slot
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotDisplay : MonoBehaviour
{
    public Text typeText;
    public Text sizeText;
    public Text qtyText;
    
    private Entry entry;
    private BuildManager bm;
    private int position;

    private void Start()
    {
        bm = GameObject.FindWithTag("BuildManager").GetComponent<BuildManager>();
    }

    /// <summary>
    /// Changes the displayed text for type
    /// </summary>
    /// <param name="text">Type to display</param>
    public void SetTypeText(string text)
    {
        typeText.text = text;
    }

    /// <summary>
    /// Changes the displayed text for size
    /// </summary>
    /// <param name="text">Size to display</param>
    public void SetSizeText(string text)
    {
        sizeText.text = text;
    }

    /// <summary>
    /// Changes the displayed text for quantity
    /// </summary>
    /// <param name="qty">Quantity to display</param>
    public void SetQtyText(int qty)
    {
        qtyText.text = "QTY: " + qty;
    }

    /// <summary>
    /// Sets the entry associated with the InventorySlotDisplay
    /// </summary>
    /// <param name="e">Entry to associate</param>
    public void SetEntry(Entry e)
    {
        entry = e;
        SetTypeText(e.Material.TypeText);
        SetSizeText(e.Material.SizeText);
        SetQtyText(e.Qty);
    }

    /// <summary>
    /// Creates an object matching the associated entry
    /// </summary>
    public void Build()
    {
        bm.BuildGivenItem(entry.Material, position);
    }

    /// <summary>
    /// Stores the position within the inventory
    /// </summary>
    /// <param name="i">Inventory position</param>
    public void SetPosition(int i)
    {
        position = i;
    }
    
}
