using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropDown;

    public Slider volSlider;

    Resolution[] resolutions;

    void Start() {
        if (audioMixer != null) {
            float vol;
            if(audioMixer.GetFloat("volume", out vol))
                volSlider.value = vol;
        }
            

        if (resolutionDropDown == null)
            return;

        resolutions = Screen.resolutions;

        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

       
    }


    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);

        if (FindObjectOfType<AudioManager>() != null)
            FindObjectOfType<AudioManager>().Play("Shoot");
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
