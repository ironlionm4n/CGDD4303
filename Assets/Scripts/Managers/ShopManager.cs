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
    public InputField amtStrut;
    public InputField amtStud;
    public InputField amtClamp;
    
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
        int qty2x6 = 0;
        if (amt2x6 != null)
        {
           qty2x6 = amt2x6.text == "" ? 0 : int.Parse(amt2x6.text);
            if (qty2x6 > 0)
            {
                Entry new2x6 = new Entry(new2x6Mat, qty2x6);
                im.AddEntry(new2x6);
            }
        }
        
        //4x4
        ConstructionMaterial new4x4Mat = new ConstructionMaterial(lumber4x4, BuildSystem.GetDefaultSize(lumber4x4));
        int qty4x4 = amt4x4.text == "" ? 0 : int.Parse(amt4x4.text);
        if(qty4x4 > 0)
        {
            Entry new4x4 = new Entry(new4x4Mat, qty4x4);
            im.AddEntry(new4x4);
        }

        //Tie
        int qtyTie = 0;
        if (amtTie != null)
        {
            ConstructionMaterial newTieMat = new ConstructionMaterial(tie, BuildSystem.GetDefaultSize(tie));
            qtyTie = amtTie.text == "" ? 0 : int.Parse(amtTie.text);
            if (qtyTie > 0)
            {
                Entry newTie = new Entry(newTieMat, qtyTie);
                im.AddEntry(newTie);
            }
        }

        //Strut
        int qtyStrut = 0;
        if (amtStrut != null)
        {
            ConstructionMaterial newStrutMat = new ConstructionMaterial(strut, BuildSystem.GetDefaultSize(strut));
            qtyStrut = amtStrut.text == "" ? 0 : int.Parse(amtStrut.text);
            if (qtyStrut > 0)
            {
                Entry newStrut = new Entry(newStrutMat, qtyStrut);
                im.AddEntry(newStrut);
            }
        }

        //Clamp
        int qtyClamp = 0;
        if (amtClamp != null)
        {
            ConstructionMaterial newClampMaterial = new ConstructionMaterial(clamp, BuildSystem.GetDefaultSize(clamp));
            qtyClamp = amtClamp.text == "" ? 0 : int.Parse(amtClamp.text);
            if(qtyClamp > 0)
            {
                Entry newClamp = new Entry(newClampMaterial, qtyClamp);
                im.AddEntry(newClamp);
            }
        }

        gm.StoreCheckout(qtyPly, qty2x4, qty2x6, qty4x4, qtyTie, qtyStrut, qtyClamp);
        LeaveShop();
    }

    /// <summary>
    /// Sets all the input fields to 0 and returns to the menu
    /// </summary>
    public void LeaveShop()
    {
        if (amtPly != null)
        {
            amtPly.text = "0";
        }

        if (amt2x4 != null)
        {
            amt2x4.text = "0";
        }

        if (amt2x6 != null)
        {
            amt2x6.text = "0";
        }

        if (amt4x4 != null)
        {
            amt4x4.text = "0";
        }

        if (amtTie != null)
        {
            amtTie.text = "0";
        }

        if(amtStrut != null)
        {
            amtStrut.text = "0";
        }

        if(amtClamp != null)
        {
            amtClamp.text = "0";
        }

        mm.SwitchState(MenuManager.State.Main);
    }

}
