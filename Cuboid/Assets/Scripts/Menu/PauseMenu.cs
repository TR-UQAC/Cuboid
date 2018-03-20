using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingMenuUI;

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

        if (CrossPlatformInputManager.GetButtonDown("Fire2")) {
            if (pauseMenuUI.activeSelf) 
                Resume();
             else if (settingMenuUI.activeSelf) {

                if (GameObject.Find("Dropdown List")) {
                    GameObject obj = GameObject.Find("Dropdown List");

                    GameObject parent = obj.transform.parent.gameObject;
                    ES.SetSelectedGameObject(parent);

                    Destroy(obj);

                    if (GameObject.Find("Blocker"))
                        Destroy(GameObject.Find("Blocker"));

                } else {
                    pauseMenuUI.SetActive(true);
                    settingMenuUI.SetActive(false);

                    ES = FindObjectOfType<EventSystem>();
                    ES.SetSelectedGameObject(storeSelected);
                }
            }

        }

    }

    public void Resume() {
        Cursor.visible = false;
        GameIsPaused = false;

        if (GameObject.Find("Dropdown List"))
            Destroy(GameObject.Find("Dropdown List"));
        if (GameObject.Find("Blocker"))
            Destroy(GameObject.Find("Blocker"));

        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(false);


        Time.timeScale = 1f;
    }

    void Pause() {
        Cursor.visible = true;
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
