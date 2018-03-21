using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class MainMenu : MonoBehaviour {

    public EventSystem ES;
    public GameObject mainMenu;
    public GameObject settingMenuUI;
    public GameObject levelSelecUI;
    private GameObject storeSelected;


    private void Start() {
        storeSelected = ES.firstSelectedGameObject;
    }

    private void Update() {
        /*
        if (ES.currentSelectedGameObject != storeSelected) {
            if (ES.currentSelectedGameObject == null)
                ES.SetSelectedGameObject(storeSelected);

            else
                storeSelected = ES.currentSelectedGameObject;
        }
        */
        if (CrossPlatformInputManager.GetButtonDown("Fire2")) {
            if (settingMenuUI.activeSelf) {
                if (GameObject.Find("Dropdown List")) {
                    GameObject obj = GameObject.Find("Dropdown List");

                    GameObject parent = obj.transform.parent.gameObject;
                    ES.SetSelectedGameObject(parent);

                    Destroy(obj);

                    if (GameObject.Find("Blocker")) 
                        Destroy(GameObject.Find("Blocker"));

                } else 
                    ChangeMenu(settingMenuUI);
                
            } else if (levelSelecUI.activeSelf)
                ChangeMenu(levelSelecUI);
        }
    }
    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void ChangeMenu(GameObject menu) {
        mainMenu.SetActive(true);
        menu.SetActive(false);

        ES = FindObjectOfType<EventSystem>();
        ES.SetSelectedGameObject(storeSelected);
    }
}
