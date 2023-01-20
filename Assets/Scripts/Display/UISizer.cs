/*
UI SIZER
Sizes certain UI elements based on the screen size
While using percentage anchors works in most cases, sometimes it isn't possible
For ScrollViews and Dropdowns, for example, the children won't resize with the screen
This forces them to be the same relative size
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISizer : MonoBehaviour
{
    public Vector2 baseScreenSize = new Vector2(1024f, 768f);
    public bool sizeX = true;
    public bool sizeY = true;
    public bool destroyAfterSetting = false;
    public RectTransform[] affected;

    void Awake()
    {
        float xScale = Screen.width / baseScreenSize.x;
        float yScale = Screen.height / baseScreenSize.y;
        
        foreach (RectTransform rt in affected)
        {
            float newX = sizeX ? rt.sizeDelta.x * xScale: rt.sizeDelta.x;
            float newY = sizeY ? rt.sizeDelta.y * yScale: rt.sizeDelta.y;
            rt.sizeDelta = new Vector2(newX, newY);
        }

        if (destroyAfterSetting)
        {
            Destroy(this);
        }
    }
}
