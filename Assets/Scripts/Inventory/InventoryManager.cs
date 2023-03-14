/*
INVENTORY MANAGER
Holds the inventory and allows for addition and removal of entries
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("UI")]
    public RectTransform inventoryHolder;
    public InventorySlotDisplay inventorySlot;
    public TMP_Text outOfMaterialsText;
    public float textVisibleTime = 1;
    public float textFadeSpeed = .5f;

    [Header("Testing")]
    public int testing2x4Amt = 22;
    public int testing2x6Amt = 12;
    public int testing4x4Amt = 60;
    public int testingSheathingAmt = 20;
    public bool testInventory = false;
    public bool testRandoms = false;

    private List<Entry> inventory = new List<Entry>();
    private Color baseTextColor;
    private Color clearTextColor;

    void Start()
    {
        //Adds default sized materials to the inventory if you need them for testing
        if (testInventory)
        {
            AddEntry(new Entry(new ConstructionMaterial(ConstructionMaterial.Type.Plywood, BuildSystem.GetDefaultSize(ConstructionMaterial.Type.Plywood)), testing2x4Amt));
            AddEntry(new Entry(new ConstructionMaterial(ConstructionMaterial.Type.Lumber2x4, BuildSystem.GetDefaultSize(ConstructionMaterial.Type.Lumber2x4)), testing2x6Amt));
            AddEntry(new Entry(new ConstructionMaterial(ConstructionMaterial.Type.Lumber2x6, BuildSystem.GetDefaultSize(ConstructionMaterial.Type.Lumber2x6)), testing4x4Amt));
            AddEntry(new Entry(new ConstructionMaterial(ConstructionMaterial.Type.Lumber4x4, BuildSystem.GetDefaultSize(ConstructionMaterial.Type.Lumber4x4)), testingSheathingAmt));
        }

        baseTextColor = outOfMaterialsText.color;
        clearTextColor = new Color(baseTextColor.r, baseTextColor.g, baseTextColor.b, 0);
        outOfMaterialsText.color = clearTextColor;
        DisplayInventory();
    }

    private void Update()
    {
        //Creates a random entry if you need it for testing
        if (testRandoms && Input.GetKeyDown(KeyCode.Space))
        {
            CreateRandomEntry();
        }
    }

    /// <summary>
    /// Adds a new entry to the inventory or increases quantity if that material is already in the inventory
    /// </summary>
    /// <param name="e">The entry to add</param>
    public void AddEntry(Entry e)
    {
        //Check if there's already an entry for that material
        Entry match = inventory.Find(x => x.Material.Equals(e.Material));
        if(match != null)
        {
            match.ChangeQty(e.Qty);
        }
        else
        {
            inventory.Add(e);
        }

        inventory.Sort();
        DisplayInventory();
    }

    /// <summary>
    /// Uses up an item from the inventory
    /// </summary>
    /// <param name="i">The index of the used item</param>
    /// <param name="amtUsed">How much to use</param>
    public bool UseItem(int i, int amtUsed = 1)
    {
        bool remaining = true;
        inventory[i].ChangeQty(-1 * amtUsed);
        if(inventory[i].Qty <= 0)
        {
            inventory.RemoveAt(i);
            remaining = false;
        }
        inventory.Sort();
        DisplayInventory();
        return remaining;
    }

    /// <summary>
    /// Uses up an item from the inventory
    /// </summary>
    /// <param name="e">The entry to change</param>
    /// <param name="amtUsed">How much to use</param>
    public bool UseItem(Entry e, int amtUsed = 1)
    {
        int index = inventory.FindIndex(0, inventory.Count, (x => x.Equals(e)));
        if(index >= 0)
        {
            return UseItem(index, amtUsed);
        }
        else
        {
            Debug.Log("Trying to use an item that isn't in the inventory!");
            return false;
        }
    }

    /// <summary>
    /// Displays the inventory
    /// </summary>
    private void DisplayInventory()
    {
        //Clear existing inventory
        InventorySlotDisplay[] currentDisplay = inventoryHolder.GetComponentsInChildren<InventorySlotDisplay>();
        foreach(InventorySlotDisplay isd in currentDisplay)
        {
            Destroy(isd.gameObject);
        }
        //Displays the new inventory
        for(int i = 0; i < inventory.Count; i++)
        {
            InventorySlotDisplay newSlot = Instantiate(inventorySlot);
            newSlot.SetEntry(inventory[i]);
            newSlot.SetPosition(i);
            newSlot.transform.SetParent(inventoryHolder, false);
        }
    }

    /// <summary>
    /// A debug method to create a random material and add it to the inventory
    /// </summary>
    private void CreateRandomEntry()
    {
        int typeI = Random.Range(0, 4);
        ConstructionMaterial.Type t = (ConstructionMaterial.Type)typeI;
        Vector3 s = new Vector3(Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10));
        int q = Random.Range(1, 30);
        AddEntry(new Entry(new ConstructionMaterial(t, s), q));
    }

    /// <summary>
    /// Checks if a matching ConstructionMaterial is in the inventory
    /// </summary>
    /// <param name="t">Type to match</param>
    /// <param name="s">Size to match</param>
    /// <returns>The matching entry, or null if there is none</returns>
    public Entry Contains(ConstructionMaterial.Type t, Vector3 s)
    {
        ConstructionMaterial checkAgainst = new ConstructionMaterial(t, s);
        Entry e = inventory.Find(x => x.Material.Equals(checkAgainst));
        return e;
    }

    /// <summary>
    /// Shows and fades the warning text when all of a given material is used
    /// </summary>
    public void OutOfEntry()
    {
        outOfMaterialsText.color = baseTextColor;
        StartCoroutine(FadeText());
    }

    /// <summary>
    /// Gradually fades text from full color to clear
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeText()
    {
        yield return new WaitForSeconds(textVisibleTime);
        for(float t = 0; t < 1; t += Time.deltaTime * textFadeSpeed)
        {
            outOfMaterialsText.color = Color.Lerp(baseTextColor, clearTextColor, t);
            yield return null;
        }
    }

    /// <summary>
    /// The list of entries the player currently has (read-only)
    /// </summary>
    public List<Entry> Inventory
    {
        get { return inventory; }
    }
}
