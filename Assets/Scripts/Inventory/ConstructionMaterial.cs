/*
CONSTRUCTION MATERIAL
A class representing either plywood or a specific size of lumber
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConstructionMaterial : IComparable
{
    public enum Type { Plywood, Lumber2x4, Lumber4x4, Lumber2x6, Tie, Strut, Stud, Clamp, None}

    private const float TOLERANCE = 0.01f;
    private const float TWO_INCHES = 2f / 12f;
    private const float FOUR_INCHES = 4f / 12f;
    private const float SIX_INCHES = 6f / 12f;
    private const float THREE_QUARTERS_INCH = .75f / 12f;

    private Type type;
    private Vector3 size;

    private BuildManager bm;

    /// <summary>
    /// Creates a new ConstructionMaterial object
    /// </summary>
    /// <param name="t">Type of material</param>
    /// <param name="s">Size of material</param>
    public ConstructionMaterial(Type t, Vector3 s)
    {
        bm = GameObject.Find("ConstructionManager").GetComponent<BuildManager>();

        //We don't just set size for a couple of reasons
        //1. We want plywood to always have the longer side first
        //2. If this is, say, a 2x4, we need to enforce that it is actually 2 inches by 4 inches
        type = t;
        switch (type)
        {
            case (Type.Plywood):
                size = new Vector3(Mathf.Max(s.x, s.z), THREE_QUARTERS_INCH, Mathf.Min(s.x, s.z));
                break;
            case (Type.Lumber2x4):
                size = new Vector3(TWO_INCHES, FOUR_INCHES, s.z);
                break;
            case (Type.Lumber2x6):
                size = new Vector3(TWO_INCHES, SIX_INCHES, s.z);
                break;
            case (Type.Lumber4x4):
                size = new Vector3(FOUR_INCHES, FOUR_INCHES, s.z);
                break;
            case (Type.Tie):
                size = new Vector3(41.5f, 41.5f, 4.15f); //Tie cannot change size
                break;
            case(Type.Stud):
                size = new Vector3(TWO_INCHES, FOUR_INCHES, 15.0f);
                break;
            case(Type.Strut):
                 if (bm.FormworkType == FormWorkType.Wall)
                 {
                    size = new Vector3(FOUR_INCHES, FOUR_INCHES, s.z);
                }
                else
                {
                    size = new Vector3(FOUR_INCHES, FOUR_INCHES, 12);
                }
                break;
            case (Type.Clamp):
                size = new Vector3(-0.0192f, 0.0017f, 0.0005f);
                break;
            default:
                size = new Vector3();
                break;
        }
    }

    public override string ToString()
    {
        return type.ToString() + " " + SizeText;
    }

    public override bool Equals(object obj)
    {
        ConstructionMaterial compare = (ConstructionMaterial)obj;
        if(type != compare.type)
        {
            return false;
        }
        else
        {
            if(Mathf.Abs(size.x - compare.size.x) >= TOLERANCE)
            {
                return false;
            }
            else
            {
                if(Mathf.Abs(size.y - compare.size.y) >= TOLERANCE)
                {
                    return false;
                }
                else
                {
                    if(Mathf.Abs(size.z - compare.size.z) >= TOLERANCE)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Size of the ConstructionMaterial
    /// </summary>
    public Vector3 Size
    {
        get { return size; }
    }

    /// <summary>
    /// Type of the ConstructionMaterial
    /// </summary>
    public Type MaterialType
    {
        get { return type; }
    }

    /// <summary>
    /// Returns X'xZ'xY" for plywood and X"xY"xZ' for lumber
    /// </summary>
    public string SizeText
    {
        get
        {
            return SizeToText(type, size, false);
        }
    }

    /// <summary>
    /// Returns the material type as a string
    /// </summary>
    public string TypeText
    {
        get
        {
            switch (type)
            {
                case (Type.Plywood):
                    return "Plywood";
                case (Type.Lumber2x4):
                    return "2x4 Lumber";
                case (Type.Lumber2x6):
                    return "2x6 Lumber";
                case (Type.Lumber4x4):
                    return "4x4 Lumber";
                case (Type.Tie):
                    return "Tie";
                case (Type.Strut):
                    return "Strut";
                case (Type.Clamp):
                    return "Clamp";
                default:
                    return "None";
            }
        }
    }

    /// <summary>
    /// Sorts in order Plywood before Lumber, then by size
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int CompareTo(object obj)
    {
        ConstructionMaterial compare = (ConstructionMaterial)obj;

        //If the type is none, something went wrong so stick it at the end
        if (type == Type.None)
        {
            return 1;
        }
        //Plywood comes first
        else if (type == Type.Plywood)
        {
            if (compare.type != Type.Plywood)
            {
                return -1;
            }
            else
            {
                //Sort by x size, then z - all plywood is the same thickness
                float xDiff = size.x - compare.size.x;

                if (Mathf.Approximately(xDiff, 0))
                {
                    float zDiff = size.z - compare.size.z;

                    if (Mathf.Approximately(zDiff, 0))
                    {
                        return 0;
                    }
                    else
                    {
                        return (int)(zDiff * 1000);
                    }
                }
                else
                {
                    return (int)(xDiff * 1000);
                }
            }
        }
        else
        {
            //If this is lumber, it goes after plywood
            if (compare.type == Type.Plywood)
            {
                return 1;
            }
            //Order is 2x4, 2x6, 4x4
            else if (type == Type.Lumber2x4)
            {
                if (type != compare.type)
                {
                    return -1;
                }
                else
                {
                    return (int)(size.z - compare.size.z);
                }
            }
            else if (type == Type.Lumber2x6)
            {
                if (compare.type == Type.Lumber2x4)
                {
                    return 1;
                }
                else if (compare.type == Type.Lumber4x4)
                {
                    return -1;
                }
                else if (compare.type == Type.Strut)
                {
                    return -1;
                }
                else if (compare.type == Type.Tie)
                {
                    return -1;
                }
                else if(compare.type == Type.Clamp)
                {
                    return -1;
                }
                else
                {
                    return (int)(size.z - compare.size.z);
                }
            }
            else if (type == Type.Lumber4x4)//4x4
            {
                if (type != compare.type && compare.type != Type.Strut && compare.type != Type.Tie)
                {
                    return 1;
                }
                else if(compare.type == Type.Strut)
                {
                    return -1;
                }
                else if(compare.type == Type.Tie)
                {
                    return -1;
                }
                else if (compare.type == Type.Clamp)
                {
                    return -1;
                }
                else
                {
                    return (int)(size.z - compare.size.z);
                }
            }
            else if(type == Type.Strut) //Strut
            {
                if (compare.type == Type.Lumber2x4)
                {
                    return 1;
                }
                else if (compare.type == Type.Lumber2x6)
                {
                    return 1;
                }
                else if (compare.type == Type.Lumber4x4)
                {
                    return 1;
                }
                else if(compare.type == Type.Tie)
                {
                    return -1;
                }
                else if (compare.type == Type.Clamp)
                {
                    return -1;
                }
                else
                {
                    return (int)(size.z - compare.size.z);
                }
            }
            else if (type == Type.Tie)//Tie
            {
                if (compare.type == Type.Lumber2x4)
                {
                    return 1;
                }
                else if (compare.type == Type.Lumber2x6)
                {
                    return 1;
                }
                else if (compare.type == Type.Lumber4x4)
                {
                    return 1;
                }
                else if (compare.type == Type.Strut)
                {
                    return 1;
                }
                else if(compare.type == Type.Clamp)
                {
                    return -1;
                }
                else
                {
                    return (int)(size.z - compare.size.z);
                }
            }
            else if(type == Type.Clamp)
            {
                if (compare.type == Type.Lumber2x4)
                {
                    return 1;
                }
                else if (compare.type == Type.Lumber2x6)
                {
                    return 1;
                }
                else if (compare.type == Type.Lumber4x4)
                {
                    return 1;
                }
                else if (compare.type == Type.Strut)
                {
                    return 1;
                }
                else if(compare.type == Type.Tie)
                {
                    return 1;
                }
                else
                {
                    return (int)(size.z - compare.size.z);
                }
            }
            else
            {
                return 1;
            }
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Converts any Vector3 into an appropriately formatted size string
    /// </summary>
    /// <param name="t">Type of material to format for</param>
    /// <param name="s">Size</param>
    /// <param name="useSpaces">Whether or not to add spaces</param>
    /// <returns>Formatted string</returns>
    public static string SizeToText(Type t, Vector3 s, bool useSpaces)
    {
        string space = useSpaces ? " x " : "x";

        string xNum;
        string yNum;
        string zNum;

        //Symbols - ' for feet and " for inches
        string xMod = "\"";
        string yMod = "\"";
        string zMod = "'";

        //We're not just returning the size because it's not nice to read
        //It's much easier to understand 2"x4" than 0.167x.333
        //And because plywood's thickness doesn't really matter, we put that last

        switch (t)
        {
            case (Type.Plywood):
                xNum = s.x.ToString();
                yNum = s.z.ToString();
                zNum = "3/4";

                xMod = "'";
                yMod = "'";
                zMod = "\"";

                break;
            case (Type.Lumber2x4):
                xNum = "2";
                yNum = "4";
                zNum = s.z.ToString();

                break;
            case (Type.Lumber2x6):
                xNum = "2";
                yNum = "6";
                zNum = s.z.ToString();

                break;
            case (Type.Lumber4x4):
                xNum = "4";
                yNum = "4";
                zNum = s.z.ToString();

                break;
            case (Type.Strut):
                xNum = "4";
                yNum = "4";
                zNum = s.z.ToString();

                break;

            case (Type.Tie):
                xNum = "5";
                yNum = "5";
                zNum = s.z.ToString();

                break;

            case (Type.Clamp):
                xNum = "3.875";
                yNum = "3.875";
                zNum = "2";

                break;
            default:
                return "0'x0'x0'";
        }

        return xNum + xMod + space + yNum + yMod + space + zNum + zMod;
    }
}
