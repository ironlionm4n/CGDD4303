/*
INPUT FIELD VARIABLE SET
Sets the empty text for a PositiveInputField
Necessary because if the PositiveInputField takes numbers, you can't have leading zeroes normally
Without this script, you could set the empty text to 0 or 20 but not to 000 or 020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldVariableSet : MonoBehaviour
{
    public string emptyText = "000";

    void Start()
    {
        PositiveInputField pif = GetComponent<PositiveInputField>();
        pif.emptyText = emptyText;
        pif.ClearText();
    }
}
