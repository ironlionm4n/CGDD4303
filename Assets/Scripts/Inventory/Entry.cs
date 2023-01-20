/*
ENTRY
An inventory entry that holds a ConstructionMaterial and the quantity
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entry : IComparable
{
    private ConstructionMaterial mat;
    private int qty;

    /// <summary>
    /// Creates a new Entry object
    /// </summary>
    /// <param name="type">ConstructionMaterial</param>
    /// <param name="q">Quantity</param>
    public Entry(ConstructionMaterial type, int q)
    {
        mat = type;
        qty = q;
    }

    /// <summary>
    /// Creates a new Entry object (with 0 quantity)
    /// </summary>
    /// <param name="type">ConstructionMaterial</param>
    public Entry(ConstructionMaterial type)
    {
        mat = type;
        qty = 0;
    }

    /// <summary>
    /// Creates a new Entry object
    /// </summary>
    /// <param name="t">Type of material</param>
    /// <param name="s">Size of material</param>
    /// <param name="q">Quantity</param>
    public Entry(ConstructionMaterial.Type t, Vector3 s, int q)
    {
        mat = new ConstructionMaterial(t, s);
        qty = q;
    }

    /// <summary>
    /// Changes the quantity held in the entry
    /// </summary>
    /// <param name="change">Change amount</param>
    public void ChangeQty(int change)
    {
        qty += change;
    }

    public override string ToString()
    {
        return mat.ToString() + " QTY: " + qty;
    }

    public int CompareTo(object obj)
    {
        Entry compare = (Entry)obj;

        if(mat.CompareTo(compare.mat) != 0)
        {
            return mat.CompareTo(compare.mat);
        }
        else
        {
            return qty - compare.qty;
        }
    }

    /// <summary>
    /// ConstructionMaterial of entry
    /// </summary>
    public ConstructionMaterial Material
    {
        get { return mat; }
    }

    /// <summary>
    /// Quantity of entry
    /// </summary>
    public int Qty
    {
        get { return qty; }
    }
}
