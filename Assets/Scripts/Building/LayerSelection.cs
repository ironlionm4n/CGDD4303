﻿/*
LAYER SELECTION
Handles switching which layer to build in
Sends each layer to the GradeManager for grading
Sets non-selected layers to invisible and inactive
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerSelection : MonoBehaviour
{
    public enum Layer
    {
        Joist,
        Sheathing,
        Shore,
        Stringer,
        None
    }

    public Layer currentLayer = Layer.None;
    public int wallLayerNum = 5;
    public int slabLayerNum = 4;

    public Dropdown startSelection;
    public Dropdown assemblySelection;

    [SerializeField] private BuildManager buildManager;

    [Header("Slab Example Components")] public HideExamples slab_exampleShores;
    public HideExamples slab_exampleJoists;
    public HideExamples slab_exampleStringers;
    public HideExamples slab_exampleSheathing;

    [Header("Wall Example Components")] public HideExamples wall_exampleShores;
    public HideExamples wall_exampleJoists;
    public HideExamples wall_exampleStringers;
    public HideExamples wall_exampleSheathing;

    [Header("Column Example Components")] public HideExamples column_exampleShores;
    public HideExamples column_exampleJoists;
    public HideExamples column_exampleStringers;
    public HideExamples column_exampleSheathing;

    private Layer previousLayer;
    private HideExamples[] exampleBuilds;
    private GradeManager gm;
    private BuildSystem bs;

    void Start()
    {
        gm = GetComponent<GradeManager>();
        bs = GetComponent<BuildSystem>();

        if (buildManager.FormworkType == FormWorkType.Slab)
        {
            //Creates an array for easy iteration
            exampleBuilds = new HideExamples[slabLayerNum];
            exampleBuilds[(int) Layer.Shore] = slab_exampleShores;
            exampleBuilds[(int) Layer.Joist] = slab_exampleJoists;
            exampleBuilds[(int) Layer.Stringer] = slab_exampleStringers;
            exampleBuilds[(int) Layer.Sheathing] = slab_exampleSheathing;
        }
        if (buildManager.FormworkType == FormWorkType.Wall)
        {
            //Creates an array for easy iteration
            exampleBuilds = new HideExamples[wallLayerNum];
            exampleBuilds[(int) Layer.Shore] = wall_exampleShores;
            exampleBuilds[(int) Layer.Joist] = wall_exampleJoists;
            exampleBuilds[(int) Layer.Stringer] = wall_exampleStringers;
            exampleBuilds[(int) Layer.Sheathing] = wall_exampleSheathing;
        }

        //Adds the layers in order to the two dropdowns
        startSelection.options.Clear();
        assemblySelection.options.Clear();
        for (int i = 0; i < wallLayerNum; i++)
        {
            startSelection.options.Add(new Dropdown.OptionData(((Layer) i).ToString()));
            assemblySelection.options.Add(new Dropdown.OptionData(((Layer) i).ToString()));
        }
    }

    /// <summary>
    /// Changes the visibility of the example build according to the selected layer
    /// </summary>
    public void ChangeLayer()
    {
        gm.AddLayer(currentLayer);
        bs.CancelBuild();
        foreach (HideExamples h in exampleBuilds)
        {
            if(h != null)
                h.Hide();
        }

        if (currentLayer == Layer.None)
        {
            foreach (HideExamples h in exampleBuilds)
            {
                h.Show();
            }
        }
        else
        {
            for (int i = 0; i < exampleBuilds.Length; i++)
            {
                if (i == (int) currentLayer)
                {
                    exampleBuilds[i].Transparent();
                }
            }
        }
    }

    /// <summary>
    /// Changes the visibility of the example build according to the selected layer
    /// </summary>
    /// <param name="l">Layer to change to</param>
    public void ChangeLayer(Layer l)
    {
        currentLayer = l;
        ChangeLayer();
    }

    /// <summary>
    /// Sets the current layer to the dropdown's value
    /// </summary>
    /// <param name="d">Dropdown that changed</param>
    public void DropdownChanged(Dropdown d)
    {
        currentLayer = (Layer) d.value;
        ChangeLayer();
    }
}