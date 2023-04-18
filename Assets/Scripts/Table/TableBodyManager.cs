using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class TableBodyManager : MonoBehaviour
{
    public TableRow tableRowPrefab;

    public FirebaseManager firebaseManager;

    public void AddRow(string username, int score, int kills, int position)
    {
        TableRow tableRow = Instantiate(tableRowPrefab, transform);
        tableRow.type = "Body";
        tableRow.username.text.text = username;
        tableRow.score.text.text = score.ToString();
        tableRow.kills.text.text = kills.ToString();
        //Adjust position of the row to be 1 unit below the previous row
        tableRow.transform.position = new Vector3(tableRow.transform.position.x, tableRow.transform.position.y - position * 0.65f, tableRow.transform.position.z);
    }

    void Awake()
    {
        firebaseManager = FirebaseManager.Instance;
    }

    void Start()
    {
        // if current scene is the leaderboard scene
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            // Get the top 10 scores from the database and display them in the table
            StartCoroutine(GetTopScores());
        } else {
            // Get the current user's score and display it in the table
            StartCoroutine(GetCurrentUserScore());
        }
    }

    // Get the top 10 scores from the database and display them in the table
    public IEnumerator GetTopScores()
    {
        //Get the top 10 scores from the database
        Debug.Log("Getting top scores");
        var DBTask = firebaseManager.DBreference.Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.IsCompleted)
        {
            Debug.Log("Got top scores successfully");
            DataSnapshot snapshot = DBTask.Result;
            Debug.Log("Snapshot: " + snapshot.ToString());
            // JSON format:
                /* "users": {
                    "YyGVhUAn3MYLNrlHcxcho0xoNWS2": {
                    "kills": 6,
                    "score": 60
                    }
                } */
            // Snapshot: DataSnapshot { key = users, value = System.Collections.Generic.Dictionary`2[System.String,System.Object] }
            // Get the users dictionary
            Dictionary<string, object> users = (Dictionary<string, object>)snapshot.Value;
            Debug.Log("Users: " + users.ToString());
            // Users: System.Collections.Generic.Dictionary`2[System.String,System.Object]
            // Get the keys of the users dictionary
            List<string> keys = new List<string>(users.Keys);
            Debug.Log("Keys: " + keys.ToString());
            // Keys: System.Collections.Generic.List`1[System.String]
            // Get the values of the users dictionary
            List<object> values = new List<object>(users.Values);
            Debug.Log("Values: " + values.ToString());
            // Values: System.Collections.Generic.List`1[System.Object]
            // Add the top 10 scores to the table
            
            //Order all 10 scores by score
            values.Sort(delegate (object a, object b)
            {
                Dictionary<string, object> userA = (Dictionary<string, object>)a;
                Dictionary<string, object> userB = (Dictionary<string, object>)b;
                int scoreA = int.Parse(userA["score"].ToString());
                int scoreB = int.Parse(userB["score"].ToString());
                return scoreB.CompareTo(scoreA);
            });

            
            int position = 1;
            foreach (object value in values)
            {

                Dictionary<string, object> user = (Dictionary<string, object>)value;

                // Get the username of the user
                string username = user["username"].ToString();

                // Get the score of the user
                int score = int.Parse(user["score"].ToString());

                // Get the kills of the user
                int kills = int.Parse(user["kills"].ToString());

                // Add the row to the table
                AddRow(username, score, kills, position);
                position++;
            }
        }
    }

    // Get the last 10 scores of the current user and display them in the table
    public IEnumerator GetCurrentUserScore()
    {
        //Get the last 10 scores of the current user
        Debug.Log("Getting current user score");
        var DBTask = firebaseManager.DBreference.Child("users").Child(firebaseManager.User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.IsCompleted)
        {
            Debug.Log("Got current user score successfully");
            DataSnapshot snapshot = DBTask.Result;
            Debug.Log("Snapshot: " + snapshot.ToString());
            // DataSnapshot { key = 4W7GVTZ3FcPJrFTBk5ScuFiSHkB3, value = System.Collections.Generic.Dictionary`2[System.String,System.Object] }

            // Get the values of the users dictionary
            Dictionary<string, object> user = (Dictionary<string, object>)snapshot.Value;
            Debug.Log("User: " + user.ToString());

            // Get the username of the user
            string username = user["username"].ToString();
            Debug.Log("Username: " + username);

            // Get the score of the user
            int score = int.Parse(user["score"].ToString());
            Debug.Log("Score: " + score);

            // Get the kills of the user
            int kills = int.Parse(user["kills"].ToString());

            // Add the row to the table
            AddRow(username, score, kills, 1);
        }
    }



}
