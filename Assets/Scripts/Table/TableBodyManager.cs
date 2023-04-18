using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class TableBodyManager : MonoBehaviour
{
    public TableRow tableRowPrefab;

    public FirebaseManager firebaseManager;

    public void AddRow(string username, int score, int kills, int position) {
        TableRow tableRow = Instantiate(tableRowPrefab, transform);
        tableRow.type = "Body";
        tableRow.username.textValue = username;
        tableRow.score.textValue = score.ToString();
        tableRow.kills.textValue = kills.ToString();
        //Adjust position of the row to be 1 unit below the previous row
        tableRow.transform.position = new Vector3(tableRow.transform.position.x, tableRow.transform.position.y - position* 0.65f, tableRow.transform.position.z);


    }

    void Awake() {
        firebaseManager = FirebaseManager.Instance;
    }

    void Start() {
        StartCoroutine(GetTopScores());
        print("HERE");
    }

    // Get the top 10 scores from the database and display them in the table
    public IEnumerator GetTopScores() {
        //Get the top 10 scores from the database
        Debug.Log("Getting top scores");
        var DBTask = firebaseManager.DBreference.Child("scores").OrderByChild("score").LimitToLast(10).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.IsCompleted) {
            DataSnapshot snapshot = DBTask.Result;
            //Loop through the children
            int position = 1;
            foreach (DataSnapshot childSnapshot in snapshot.Children) {
                //Get the username
                string username = childSnapshot.Child("username").Value.ToString();
                //Get the score
                string score = childSnapshot.Child("score").Value.ToString();
                //Get the kills
                string kills = childSnapshot.Child("kills").Value.ToString();
                //Add the username and score to the list
                AddRow(username, int.Parse(score), int.Parse(kills), position);
                position++;
                Debug.Log("CREATED");
            }
        }
    }
    
}
