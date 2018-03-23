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

    public GameObject firstMenuButton;
    public GameObject firtsButtonSetting;


    private void Start() {
        ES = FindObjectOfType<EventSystem>();

        //storeSelected = ES.firstSelectedGameObject;
    }
    // Update is called once per frame
    void Update () {

        if (ES.currentSelectedGameObject != storeSelected) {
            if (ES.currentSelectedGameObject == null)
                ES.SetSelectedGameObject(storeSelected);

            else
                storeSelected = ES.currentSelectedGameObject;
        }
        

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

                } else 
                    ChangeMenu(settingMenuUI);
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
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        ES.SetSelectedGameObject(firstMenuButton);
    }

    public void OptionMenu() {
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(true);

        ES.SetSelectedGameObject(firtsButtonSetting);
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

    public void ChangeMenu(GameObject menuToClose) {
        pauseMenuUI.SetActive(true);
        menuToClose.SetActive(false);
        
        ES = FindObjectOfType<EventSystem>();
        ES.SetSelectedGameObject(firstMenuButton);
        
    }
}
