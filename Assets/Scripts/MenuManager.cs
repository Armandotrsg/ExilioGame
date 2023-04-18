using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private FirebaseManager firebaseManager;

    void Start() {
        firebaseManager = FirebaseManager.Instance;
        Assert.IsNotNull(firebaseManager, "FirebaseManager is null");
    }
    public void playGame() {
        SceneManager.LoadScene(2);
    }

    public void viewGlobalScoreboard() {
        SceneManager.LoadScene(3);
    }

    public void viewMyScoreboard() {
        SceneManager.LoadScene(5);
    }

    public void quitGame() {
        firebaseManager.Signout();
        Application.Quit();
    }

    public void backToMenu() {
        SceneManager.LoadScene(1);
    }
}
