using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    [Header("Login/Register")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;

    [Header("UI")]
    public TMP_Text statusText;

    // Start is called before the first frame update
    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
        DontDestroyOnLoad(this.gameObject);
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
    }

    public void LoginButton() {
        StartCoroutine(Login(emailField.text, passwordField.text));
    }

    public void RegisterButton() {
        StartCoroutine(Register(emailField.text, passwordField.text));
    }

    private IEnumerator Login(string email, string password) {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login failed";
            switch (errorCode) {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "User Not Found";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email Already In Use";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak Password";
                    break;
            }
            statusText.text = message;
            //Set text color to red
            statusText.color = new Color32(255, 0, 0, 255);
        }
        else {
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            statusText.text = "Login Successful";
            //Set text color to green
            statusText.color = new Color32(0, 255, 0, 255);
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator Register(string email, string password) {
        var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Registration failed";
            switch (errorCode) {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email Already In Use";
                    break;
            }
            statusText.text = message;
            //Set text color to red
            statusText.color = new Color32(255, 0, 0, 255);
        }
        else {
            User = RegisterTask.Result;
            if (User != null) {
                Debug.LogFormat("Firebase user created successfully: {0} ({1})", User.DisplayName, User.Email);
                statusText.text = "Registration Successful";
                //Set text color to green
                statusText.color = new Color32(0, 255, 0, 255);
            }
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        // Login when click enter
        if (Input.GetKeyDown(KeyCode.Return)) {
            LoginButton();
        }
    }

}