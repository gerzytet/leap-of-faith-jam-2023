using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject PauseMenu_UI;

    public bool pauseButtonPressed()
    {
        return (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7));
    }

    public void enterPauseMenu(){
        PauseMenu_UI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void resumeGame(){
        PauseMenu_UI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void returnToMainMenu(){
        //Debug.Log("going to main menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void goToLevelSelect(){
        //Debug.Log("going to main menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseButtonPressed()){
            if (gameIsPaused){
                resumeGame();
            }else{
                enterPauseMenu();
            }
        }
    }
}