/*
MENU MANAGER
Handles switching between canvases for the different parts of the game
Also contains most of the functions that will be called by buttons
*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public enum State { Main, Shop, Cut, Results, Menu}

    public Canvas[] uiCanvases;

    public State current;
    public GameObject restartPopup;
    public Text instructionsText;

    private GradeManager gm;

    private void Start()
    {
        SwitchState(current);
        gm = GetComponent<GradeManager>();
        restartPopup.SetActive(false);
        SetInstructionsText();
    }

    private void SetInstructionsText()
    {
        BuildSystem bs = GetComponent<BuildSystem>();
        CameraControl cc = Camera.main.GetComponentInParent<CameraControl>();
        //Manually inputting a few because their enum names are ugly
        //You'd have to set up a huge keycode dictionary to give "nice" names, and that's not worth it
        //If you want to, though: https://answers.unity.com/questions/1182703/how-to-get-the-nice-name-of-a-keycode.html
        string instructions = $"Build: Left Click\n";
        instructions += $"Rotate: {bs.rotateKey}\n";
        instructions += $"Cancel Current Material: {bs.cancelKey}\n";
        instructions += $"Delete: {bs.deleteKey}\n";
        instructions += $"Camera: Shift to rotate, Control to zoom";
        instructionsText.text = instructions;
    }

    /// <summary>
    /// Changes the visible UI based on the state
    /// </summary>
    /// <param name="newState">State to change to</param>
    public void SwitchState(State newState)
    {
        current = newState;
        SwitchCanvas();
    }

    /// <summary>
    /// Changes the visible canvas to match the new state
    /// </summary>
    private void SwitchCanvas()
    {
        for(int i = 0; i < uiCanvases.Length; i++)
        {
            if(i == (int)current)
            {
                uiCanvases[i].gameObject.SetActive(true);
            }
            else
            {
                uiCanvases[i].gameObject.SetActive(false);
            }
        }
    }

    #region Button Functions
    /// <summary>
    /// Opens the shop UI
    /// </summary>
    public void GoToShop()
    {
        if (GetComponent<BuildManager>().IsAssembling)
        {
            gm.VisitShop();
        }
        SwitchState(State.Shop);
    }

    /// <summary>
    /// Opens the cut UI
    /// </summary>
    public void GoToCut()
    {
        SwitchState(State.Cut);
    }

    /// <summary>
    /// Closes the main menu and starts the game
    /// </summary>
    public void StartGame()
    {
        SwitchState(State.Main);
    }

    /// <summary>
    /// Reloads the scene
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Closes the application
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Opens the popup about returning to the main menu
    /// </summary>
    public void OpenRestartPopup()
    {
        restartPopup.SetActive(true);
        GetComponent<BuildManager>().PauseNoUI();
    }

    /// <summary>
    /// Closes the popup about returning to the main menu
    /// </summary>
    public void CloseRestartPopup()
    {
        restartPopup.SetActive(false);
        GetComponent<BuildManager>().Unpause();
    }

    #endregion

}
