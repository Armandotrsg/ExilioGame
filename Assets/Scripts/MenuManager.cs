using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public void playGame() {
        SceneManager.LoadScene(2);
    }

    public void viewGlobalScoreboard() {
        SceneManager.LoadScene(1);
    }

    public void viewMyScoreboard() {
        SceneManager.LoadScene(0);
    }

    public void quitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
