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
    public int score;

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
    public IEnumerator SetScore(int score) {
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
    public IEnumerator SetKills(int kills) {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("kills").SetValueAsync(kills);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.IsCompleted) {
            Debug.Log("Kills saved successfully");
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

    // Set the username to be the email


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

    //Load the global scoreboard of all users
    /* public void LoadScoreboard() {
        //Get the reference to the scoreboard
        var DBTask = DBreference.Child("users").OrderByChild("score").GetValueAsync();
        //Wait until the task is completed
        StartCoroutine(WaitForDBTask(DBTask));
    } */

    //Wait for the database task to complete
    /* private IEnumerator WaitForDBTask(Task<DataSnapshot> task) {
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if (task.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {task.Exception}");
        }
        else if (task.IsCompleted) {
            DataSnapshot snapshot = task.Result;
            //Get the number of users
            int numUsers = (int)snapshot.ChildrenCount;
            //Create an array of users
            User[] users = new User[numUsers];
            //Get the enumerator for the snapshot
            var enumerator = snapshot.Children.GetEnumerator();
            //Loop through the enumerator
            for (int i = 0; i < numUsers; i++) {
                enumerator.MoveNext();
                //Get the current user
                DataSnapshot userSnapshot = enumerator.Current;
                //Get the user's data
                string email = userSnapshot.Child("email").Value.ToString();
                int score = int.Parse(userSnapshot.Child("score").Value.ToString());
                int kills = int.Parse(userSnapshot.Child("kills").Value.ToString());
                //Create a new user
                User user = new User(email, score, kills);
                //Add the user to the array
                users[i] = user;
            }
            //Sort the users by score
            Array.Sort(users);
            //Loop through the users
            for (int i = 0; i < numUsers; i++) {
                //Get the current user
                User user = users[i];
                //Create a new text object
                GameObject textObject = Instantiate(textPrefab, scoreboardContent.transform);
                //Get the text component
                Text text = textObject.GetComponent<Text>();
                //Set the text
                text.text = $"{i + 1}. {user.email} - {user.score} - {user.kills}";
            }
        }
    }  */

}