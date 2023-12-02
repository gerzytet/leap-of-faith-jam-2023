using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject button0;
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    [SerializeField] private GameObject button3;
    [SerializeField] private GameObject button4;
    [SerializeField] private GameObject button5;
    [SerializeField] private GameObject button6;


    public void selectLevel(){
        //dumb way of converting name to int
        int buttonNum = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        Frog.instance.reachedFlag(buttonNum);
        Debug.Log("Button num: " + buttonNum);
    }
}
