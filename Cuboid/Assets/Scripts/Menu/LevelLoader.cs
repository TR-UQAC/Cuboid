using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Add the TextMesh Pro namespace to access the various functions.
using UnityStandardAssets.CrossPlatformInput;

public class LevelLoader : MonoBehaviour {

    public Slider loadingSlider;
    public Image imageTuto;
    public TextMeshProUGUI progressText;

    public void Zone1() {
        LoadLevel(1);
        //SceneManager.LoadScene("Zone1Level");
    }

    public void DemoEnnemi() {
        LoadLevel(2);
        //SceneManager.LoadScene(2);
    }

    public void DemoUpgrade() {
        LoadLevel(3);
        //SceneManager.LoadScene(3);
    }

    public void LoadLevel (int sceneIndex) {

        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex) {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        loadingSlider.gameObject.SetActive(true);
        imageTuto.gameObject.SetActive(true);
        while (!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / .9f);

            loadingSlider.value = progress;
            progressText.text = progress * 100f + "%";


            // Check if the load has finished
            if (operation.progress >= 0.9f) {
                //Change the Text to show the Scene is ready
                progressText.text = "Appuyer sur une touche";
                //Wait to you press the space key to activate the Scene
                if (Input.anyKey)
                    //Activate the Scene
                    operation.allowSceneActivation = true;
            }

            yield return null;
        }

    }
}
