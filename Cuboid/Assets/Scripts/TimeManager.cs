using UnityEngine;

public class TimeManager : MonoBehaviour {

    public float slowdownFactor = 0.05f;
    public static float slowdownFactore;
    public float slowdownLength = 2f;

    private void Start() {
        slowdownFactore = slowdownFactor;
    }
    // Update is called once per frame
    void Update () {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
	}

    public static void DoSlowMotion(){
        Time.timeScale = slowdownFactore;
        //Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}