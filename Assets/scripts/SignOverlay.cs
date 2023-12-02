using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignOverlay : MonoBehaviour
{
    public static SignOverlay instance;
    public TextMeshProUGUI textField;
    public TextMeshProUGUI startOfLevelTitle;
    public TextMeshProUGUI startOfLevelSubtitle;
    void Awake()
    {
        instance = this;
    }

    void Start() {
        Undisplay();
        startOfLevelTitle.enabled = false;
        startOfLevelSubtitle.enabled = false;
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

    IEnumerator DisplayStartOfLevelCoroutine(string title, string subtitle)
    {
        startOfLevelTitle.enabled = true;
        startOfLevelSubtitle.enabled = true;
        startOfLevelTitle.text = title;
        startOfLevelSubtitle.text = subtitle;
        yield return new WaitForSeconds(2);
        startOfLevelTitle.enabled = false;
        startOfLevelSubtitle.enabled = false;
    }
    
    public void DisplayStartOfLevel(string title, string subtitle)
    {
        StartCoroutine(DisplayStartOfLevelCoroutine(title, subtitle));
    }
}
