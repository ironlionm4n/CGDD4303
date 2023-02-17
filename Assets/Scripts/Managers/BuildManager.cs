/*
BUILD MANAGER
Starts the building process
Handles the UI for starting and finishing the assembly phase
Increments the timer
Works very closely with BuildSystem
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public KeyCode escape = KeyCode.Escape;
    public Canvas pauseScreen;

    [Header("Assembly UI")] public GameObject[] assemblyUI;
    public GameObject assemblyPopup;
    public Button assemblyButton;
    public Button finalizeButton;
    public Text timerText;
    public GameObject finalizePopup;

    public FormWorkType FormworkType;

    [Header("Wall Previews")] public Preview wall_plywoodPreview;
    public Preview wall_lumber2x4Preview;
    public Preview wall_lumber2x6Preview;
    public Preview wall_lumber4x4Preview;
    public Preview wall_TiePreview;

    [Header("Slab Previews")] public Preview slab_plywoodPreview;
    public Preview slab_lumber2x4Preview;
    public Preview slab_lumber2x6Preview;
    public Preview slab_lumber4x4Preview;

    [Header("Column Previews")] public Preview column_plywoodPreview;
    public Preview column_lumber2x4Preview;
    public Preview column_lumber2x6Preview;
    public Preview column_lumber4x4Preview;

    private BuildSystem buildSystem;
    private bool isAssembling = false;
    private float timer = 0.0f;

    private void Start()
    {
        buildSystem = GetComponent<BuildSystem>();
        assemblyPopup.SetActive(false);
        finalizePopup.SetActive(false);
        finalizeButton.interactable = false;
        ToggleAssemblyUI(false);
        pauseScreen.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(escape))
        {
            Screen.fullScreen = false;
        }

        if (isAssembling)
        {
            UpdateTimer();
        }
    }

    #region Building

    /// <summary>
    /// Creates a preview object of the associated ConstructionMaterial
    /// </summary>
    /// <param name="t">ConstructionMaterial to preview</param>
    /// <param name="position">ConstructionMaterial's position in the inventory</param>
    public void BuildGivenItem(ConstructionMaterial t, int position)
    {
        if (isAssembling)
        {
            Preview preview = null;
            if (FormworkType == FormWorkType.Slab)
            {
                switch (t.MaterialType)
                {
                    case (ConstructionMaterial.Type.Plywood):
                        preview = slab_plywoodPreview;
                        break;
                    case (ConstructionMaterial.Type.Lumber2x4):
                        preview = slab_lumber2x4Preview;
                        break;
                    case (ConstructionMaterial.Type.Lumber2x6):
                        preview = slab_lumber2x6Preview;
                        break;
                    case (ConstructionMaterial.Type.Lumber4x4):
                        preview = slab_lumber4x4Preview;
                        break;
                    default:
                        //Do nothing, it's borked
                        break;
                }
            }

            if (FormworkType == FormWorkType.Wall)
            {
                switch (t.MaterialType)
                {
                    case (ConstructionMaterial.Type.Plywood):
                        preview = wall_plywoodPreview;
                        break;
                    case (ConstructionMaterial.Type.Lumber2x4):
                        preview = wall_lumber2x4Preview;
                        break;
                    case (ConstructionMaterial.Type.Lumber2x6):
                        preview = wall_lumber2x6Preview;
                        break;
                    case (ConstructionMaterial.Type.Tie):
                        preview = wall_TiePreview;
                        break;
                    case (ConstructionMaterial.Type.Strut):
                        preview = wall_lumber4x4Preview;
                        break;
                    default:
                        //Do nothing, it's borked
                        break;
                }
            }
            if (FormworkType == FormWorkType.Column)
            {
                switch (t.MaterialType)
                {
                    case (ConstructionMaterial.Type.Plywood):
                        preview = column_plywoodPreview;
                        break;
                    case (ConstructionMaterial.Type.Lumber2x4):
                        preview = column_lumber2x4Preview;
                        break;
                    case (ConstructionMaterial.Type.Lumber2x6):
                        preview = column_lumber2x6Preview;
                        break;
                    case (ConstructionMaterial.Type.Lumber4x4):
                        preview = column_lumber4x4Preview;
                        break;
                    default:
                        //Do nothing, it's borked
                        break;
                }
            }

            buildSystem.CreatePreview(preview, t, position);
        }
    }

    /// <summary>
    /// Waits one frame, then builds a preview
    /// </summary>
    /// <param name="t">ConstructionMaterial to preview</param>
    /// <param name="position">ConstructionMaterial's position in the inventory</param>
    public void BuildGivenItemAfterFrame(ConstructionMaterial t, int position)
    {
        StartCoroutine(PauseAndBuild(t, position));
    }

    /// <summary>
    /// Waits one frame, then builds a preview
    /// </summary>
    /// <param name="t">ConstructionMaterial to preview</param>
    /// <param name="position">ConstructionMaterial's position in the inventory</param>
    public IEnumerator PauseAndBuild(ConstructionMaterial t, int position)
    {
        yield return null;
        BuildGivenItem(t, position);
    }

    #endregion

    #region UI

    /// <summary>
    /// Increments the timer and updates the timer text
    /// </summary>
    private void UpdateTimer()
    {
        timer += Time.deltaTime;
        int minutes = (int) (timer / 60);
        if (minutes < 1)
        {
            minutes = 0;
        }

        int seconds = Mathf.CeilToInt(timer % 60);
        //Have to check this in case it was like 59.8 and ceiling brought it up to 60
        if (seconds == 60)
        {
            minutes++;
            seconds = 0;
        }

        string secondText = "";
        if (seconds < 10)
        {
            secondText = "0" + seconds;
        }
        else
        {
            secondText = seconds + "";
        }

        timerText.text = minutes + ":" + secondText;
    }

    /// <summary>
    /// Starts the assembly phase
    /// </summary>
    public void StartAssembly()
    {
        isAssembling = true;
        assemblyButton.interactable = false;
        finalizeButton.interactable = true;
        ToggleAssemblyUI(true);
        assemblyPopup.SetActive(false);
        LayerSelection ls = GetComponent<LayerSelection>();
        int initialValue = ls.assemblySelection.value;
        ls.assemblySelection.value = ls.startSelection.value;

        //If the value hasn't changed the DropdownValueChanged method won't run
        //Otherwise, it does run, so if we do this every time we end up doubling the layer in the grade
        if (ls.assemblySelection.value == initialValue)
        {
            ls.ChangeLayer((LayerSelection.Layer) ls.assemblySelection.value);
        }
    }

    /// <summary>
    /// Opens the popup to start the assembly phase
    /// </summary>
    public void OpenAssemblyPopup()
    {
        assemblyPopup.SetActive(true);
    }

    /// <summary>
    /// Closes the popup to start the assembly phase
    /// </summary>
    public void CloseAssemblyPopup()
    {
        assemblyPopup.SetActive(false);
    }

    /// <summary>
    /// Opens the popup to end the assembly phase
    /// </summary>
    public void OpenFinalizePopup()
    {
        isAssembling = false;
        finalizePopup.SetActive(true);
    }

    /// <summary>
    /// Closes the popup to end the assembly phase
    /// </summary>
    public void CloseFinalizePopup()
    {
        isAssembling = true;
        finalizePopup.SetActive(false);
    }

    /// <summary>
    /// Ends the game and calls the GradeManager to run its calculations
    /// </summary>
    public void FinalizeSubmission()
    {
        GradeManager gm = GetComponent<GradeManager>();
        gm.FinalTime = timer;
        gm.EndGame();
        finalizePopup.SetActive(false);
    }

    /// <summary>
    /// Changes the visibility of UI only shown during the assembly phase
    /// </summary>
    /// <param name="b">Visibility</param>
    private void ToggleAssemblyUI(bool b)
    {
        foreach (GameObject g in assemblyUI)
        {
            g.SetActive(b);
        }
    }

    /// <summary>
    /// Pauses the timer and displays the pause popup
    /// </summary>
    public void Pause()
    {
        isAssembling = false;
        pauseScreen.gameObject.SetActive(true);
    }

    /// <summary>
    /// Unpauses the timer and hides the pause popup
    /// </summary>
    public void Unpause()
    {
        isAssembling = true;
        pauseScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// Pauses the game without showing the pause menu
    /// </summary>
    public void PauseNoUI()
    {
        isAssembling = false;
    }

    #endregion

    /// <summary>
    /// If the game is in the assembly phase or not (read-only)
    /// </summary>
    public bool IsAssembling
    {
        get { return isAssembling; }
    }
}