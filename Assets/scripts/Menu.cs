using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button playGameButton;
    [SerializeField] private Button quitGameButton;
    
    
    public void playGame(){
        Debug.Log("A button is working");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void quitGame(){
        Debug.Log("User has quit game");
        Application.Quit();
    }

    void Update(){
        if (Input.GetKey(KeyCode.JoystickButton0)){
            playGameButton.onClick.Invoke();
        }else if(Input.GetKey(KeyCode.JoystickButton1)){
            quitGameButton.onClick.Invoke();
        }
    }
}