/*
CLICK LISTENER
Calls its InventorySlotDisplay to build on click
Necessary because of the way ScrollViews show their objects
This covers the entire UI, while the object holding the InventorySlotDisplay cannot
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickListener : MonoBehaviour
{
    public InventorySlotDisplay isd;

    /// <summary>
    /// Calls the parent InventorySlotDisplay to build
    /// </summary>
    public void Build()
    {
        isd.Build();
    }
}
