using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableCell : MonoBehaviour
{
    public Image image;
    public TMP_Text text;

    public string textValue;

    void Awake() {
        text.text = textValue;
    }
}
