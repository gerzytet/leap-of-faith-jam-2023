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

    public void Display(string s)
    {
        gameObject.SetActive(true);
        textField.text = s;
    }
    
    public void Undisplay()
    {
        gameObject.SetActive(false);
    }
}
