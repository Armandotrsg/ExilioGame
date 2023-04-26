using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;

public class FirebaseManager : MonoBehaviour
{

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    [Header("Login/Register")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;

    [Header("UI")]
    public TMP_Text statusText;

    [Header("Score")]
    public int previousScore = 0;

    [Header("Instance")]
    private static FirebaseManager instance = null;
    public static FirebaseManager Instance {
        get {
            return instance;
        }
    }

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
        
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
 
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void LoginButton() {
        StartCoroutine(Login(emailField.text, passwordField.text));
    }

    public void RegisterButton() {
        StartCoroutine(Register(emailField.text, passwordField.text));
    }

    public void Signout() {
        auth.SignOut();
        Debug.Log("Signed out");
        SceneManager.LoadScene(0);
    }

    // Save score to the user's account
    public IEnumerator SaveScore(int score) {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("score").SetValueAsync(score);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.IsCompleted) {
            Debug.Log("Score saved successfully");
        }
    }

    // Save user's kills
    public IEnumerator SaveKills(int kills) {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("kills").SetValueAsync(kills);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.IsCompleted) {
            Debug.Log("Kills saved successfully");
        }
    }

    // Save the username to the user's account
    public IEnumerator SaveUsername(string username) {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(username);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.IsCompleted) {
            Debug.Log("Username saved successfully");
        }
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
            yield return new WaitForSeconds(2f);
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
                //Create a user profile and set the username
                //The username will be the email without the domain
                UserProfile profile = new UserProfile { DisplayName = email.Split('@')[0] };
                var ProfileTask = User.UpdateUserProfileAsync(profile);
                yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);
                if (ProfileTask.Exception != null) {
                    Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                    FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    statusText.text = "Registration failed";
                    //Set text color to red
                    statusText.color = new Color32(255, 0, 0, 255);
                }
                else {
                    Debug.LogFormat("User created successfully: {0} ({1})", User.DisplayName, User.Email);
                    statusText.text = "Registration Successful";
                    //Set text color to green
                    statusText.color = new Color32(0, 255, 0, 255);

                }
            }
        }
    }

    


    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            //sign out
            auth.SignOut();
            Application.Quit();
        }
        // If the scene is the main menu, then
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            //When enter is pressed, login
            if (Input.GetKeyDown(KeyCode.Return)) {
                LoginButton();
            }
        }
    }

    public IEnumerator GetPreviousScore() {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("score").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.IsCompleted) {
            DataSnapshot snapshot = DBTask.Result;
            previousScore = int.Parse(snapshot.Value.ToString());
            Debug.Log("Previous score retrieved successfully");
        }
    }
    

}