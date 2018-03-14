using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private EventSystem ES;
    public GameObject storeSelected;

    // Update is called once per frame
    void Update () {
        if (CrossPlatformInputManager.GetButtonDown("Cancel")) {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
	}

    public void Resume() {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Pause() {
        ES = FindObjectOfType<EventSystem>();
        GameIsPaused = true;
        ES.SetSelectedGameObject(storeSelected);

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OptionMenu() {
        Debug.Log("OptionMenu");
    }

    public void MainMenu() {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
