using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public EventSystem ES;
    private GameObject storeSelected;

    private void Start() {
        storeSelected = ES.firstSelectedGameObject;
    }

    private void Update() {
        if(ES.currentSelectedGameObject != storeSelected) {
            if (ES.currentSelectedGameObject == null)
                ES.SetSelectedGameObject(storeSelected);

            else
                storeSelected = ES.currentSelectedGameObject;
        }
    }
    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
