/*
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
    public enum Layer {Joist, Sheathing, Shore, Stringer, None}

    public Layer currentLayer = Layer.None;
    public int layerNum = 4;

    public Dropdown startSelection;
    public Dropdown assemblySelection;

    [Header("Example Components")]
    public HideExamples exampleShores;
    public HideExamples exampleJoists;
    public HideExamples exampleStringers;
    public HideExamples exampleSheathing;

    private Layer previousLayer;
    private HideExamples[] exampleBuilds;
    private GradeManager gm;
    private BuildSystem bs;

    void Start()
    {
        gm = GetComponent<GradeManager>();
        bs = GetComponent<BuildSystem>();

        //Creates an array for easy iteration
        exampleBuilds = new HideExamples[layerNum];
        exampleBuilds[(int)Layer.Shore] = exampleShores;
        exampleBuilds[(int)Layer.Joist] = exampleJoists;
        exampleBuilds[(int)Layer.Stringer] = exampleStringers;
        exampleBuilds[(int)Layer.Sheathing] = exampleSheathing;

        //Adds the layers in order to the two dropdowns
        startSelection.options.Clear();
        assemblySelection.options.Clear();
        for(int i = 0; i < layerNum; i++)
        {
            startSelection.options.Add(new Dropdown.OptionData(((Layer)i).ToString()));
            assemblySelection.options.Add(new Dropdown.OptionData(((Layer)i).ToString()));
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
            h.Hide();
        }
        if (currentLayer == Layer.None)
        {
            foreach(HideExamples h in exampleBuilds)
            {
                h.Show();
            }
        }
        else
        {
            for(int i = 0; i < exampleBuilds.Length; i++)
            {
                if(i == (int)currentLayer)
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
        currentLayer = (Layer)d.value;
        ChangeLayer();
    }

}
