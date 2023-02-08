/* 
SHOP MANAGER
Gets the player input in the shop to add new material to the inventory
Inherits from UIManagerParent
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : UIManagerParent
{
    [Header("Input Fields")]
    public InputField amtPly;
    public InputField amt2x4;
    public InputField amt2x6;
    public InputField amt4x4;
    public InputField amtTie;

    private void Start()
    {
        Setup();
    }

    /// <summary>
    /// Adds new entries to the inventory according to the given quantities
    /// </summary>
    public void BuyItems()
    {
        //Plywood
        ConstructionMaterial newPlyMat = new ConstructionMaterial(ply, BuildSystem.GetDefaultSize(ply));
        int qtyPly = amtPly.text == "" ? 0 : int.Parse(amtPly.text);
        if(qtyPly > 0)
        {
            Entry newPly = new Entry(newPlyMat, qtyPly);
            im.AddEntry(newPly);
        }

        //2x4
        ConstructionMaterial new2x4Mat = new ConstructionMaterial(lumber2x4, BuildSystem.GetDefaultSize(lumber2x4));
        int qty2x4 = amt2x4.text == "" ? 0 : int.Parse(amt2x4.text);
        if(qty2x4 > 0)
        {
            Entry new2x4 = new Entry(new2x4Mat, qty2x4);
            im.AddEntry(new2x4);
        }
        
        //2x6
        ConstructionMaterial new2x6Mat = new ConstructionMaterial(lumber2x6, BuildSystem.GetDefaultSize(lumber2x6));
        int qty2x6 = amt2x6.text == "" ? 0 : int.Parse(amt2x6.text);
        if(qty2x6 > 0)
        {
            Entry new2x6 = new Entry(new2x6Mat, qty2x6);
            im.AddEntry(new2x6);
        }
        
        //4x4
        ConstructionMaterial new4x4Mat = new ConstructionMaterial(lumber4x4, BuildSystem.GetDefaultSize(lumber4x4));
        int qty4x4 = amt4x4.text == "" ? 0 : int.Parse(amt4x4.text);
        if(qty4x4 > 0)
        {
            Entry new4x4 = new Entry(new4x4Mat, qty4x4);
            im.AddEntry(new4x4);
        }

        //4x4
        ConstructionMaterial newTieMat = new ConstructionMaterial(tie, BuildSystem.GetDefaultSize(tie));
        int qtyTie = amtTie.text == "" ? 0 : int.Parse(amtTie.text);
        if (qtyTie > 0)
        {
            Entry newTie = new Entry(newTieMat, qtyTie);
            im.AddEntry(newTie);
        }

        gm.StoreCheckout(qtyPly, qty2x4, qty2x6, qty4x4, qtyTie);
        LeaveShop();
    }

    /// <summary>
    /// Sets all the input fields to 0 and returns to the menu
    /// </summary>
    public void LeaveShop()
    {
        amtPly.text = "0";
        amt2x4.text = "0";
        amt2x6.text = "0";
        amt4x4.text = "0";
        amtTie.text = "0";

        mm.SwitchState(MenuManager.State.Main);
    }

}
