using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartLVLOne()
    {
        SceneManager.LoadScene("TestScene");
    }
    public void StartLVLTwo()
    {
        //SceneManager.LoadScene("TestScene");
    }
    public void StartLVLThree()
    {
        //SceneManager.LoadScene("TestScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
