/*
POSITIVE INPUT FIELD
A child of InputField that only allows positive numbers
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositiveInputField : InputField
{
    public string emptyText = "0";

    protected override void Append(char input)
    {
        if(input != '-')
        {
            base.Append(input);
        }
    }

    private void Update()
    {
        if(text == "")
        {
            ClearText();
        }
    }

    public void ClearText()
    {
        text = emptyText;
    }
}
