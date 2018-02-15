using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Add the TextMesh Pro namespace to access the various functions.

public class LevelLoader : MonoBehaviour {

    public Slider loadingSlider;
    public TextMeshProUGUI progressText;

    public void Zone1() {
        LoadLevel(1);
        //SceneManager.LoadScene("Zone1Level");
    }

    public void DemoEnnemi() {
        Debug.Log("DemoEnnemi");
        //SceneManager.LoadScene(2);
    }

    public void LoadLevel (int sceneIndex) {

        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex) {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingSlider.gameObject.SetActive(true);

        while (!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / .9f);

            loadingSlider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }

    }
}
