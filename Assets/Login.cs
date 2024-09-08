using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public InputField UNInput;
    public InputField PWinput;

    public void OnButtonClick() 
    {
        string username = UNInput.text;
        string password = PWinput.text;

        if (username == "1" && password == "2")
        {
            SceneManager.LoadScene("Game");
        }

    }

}
