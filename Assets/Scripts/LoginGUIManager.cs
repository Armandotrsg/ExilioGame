using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginGUIManager : MonoBehaviour
{
    public TMP_Text statusText;
    public TMP_InputField emailField;
    public TMP_InputField passwordField;

    // Start is called before the first frame update
    void Start()
    {
        //Clear the status text
        statusText.text = "";
        emailField.text = "";
        passwordField.text = "";
    }
}
