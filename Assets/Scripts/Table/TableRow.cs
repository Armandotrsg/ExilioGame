using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TableRow : MonoBehaviour
{
    [Header("Username")]
    public TableCell username;

    [Header("Score")]
    public TableCell score;

    [Header("Kills")]
    public TableCell kills;

    [Header("Type")]
    public string type;

    void Awake() {
        //Set image color based on type
        if (type == "Header") {
            username.image.color = new Color(0.5f, 0.5f, 0.5f);
            score.image.color = new Color(0.5f, 0.5f, 0.5f);
            kills.image.color = new Color(0.5f, 0.5f, 0.5f);
        } else {
            username.image.color = new Color(0.75f, 0.75f, 0.75f);
            score.image.color = new Color(0.75f, 0.75f, 0.75f);
            kills.image.color = new Color(0.75f, 0.75f, 0.75f);
        }
    }
}
