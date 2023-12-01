using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignOverlay : MonoBehaviour
{
    public static SignOverlay instance;
    public TextMeshProUGUI textField;
    void Awake()
    {
        instance = this;
    }

    void Start() {
        Undisplay();
    }

    public void Display(string s)
    {
        textField.enabled = true;
        textField.text = s;
    }
    
    public void Undisplay()
    {
        textField.enabled = false;
    }
}
