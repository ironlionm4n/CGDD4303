using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartLVLOne()
    {
        SceneManager.LoadScene("SlabScene");
    }

    public void StartLVLTwo()
    {
        SceneManager.LoadScene("BestWallScene");
    }

    public void StartLVLThree()
    {
        SceneManager.LoadScene("ColumnScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}