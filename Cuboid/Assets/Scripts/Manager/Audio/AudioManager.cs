using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

    private static bool keepFadingIn;
    private static bool keepFadingOut;

    private string currentMusic;

    private bool valideChange = true;
    // Use this for initialization
    void Awake () {

        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

		foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = s.output;
        }
	}

    void Start() {
        //currentMusic = "Musique_Jeu";
        //Play("Musique_Jeu");
        ChangeMusique("Musique_Jeu", .05f, .25f);
    }

    public void Play(string name) {

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null) {
            Debug.LogWarning("Le son " + name + " n'a pas été trouvé!");
            return;
        }

        if(s.source != null)
        {
            //if(s.source.outputAudioMixerGroup.name == "LaserEffect")
                //s.source.outputAudioMixerGroup.audioMixer.SetFloat("laserVolume", 0.0f);
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Le son " + name + " n'a pas été trouvé!");
            return;
        }
        else
        {
            s.Stop();
        }    
    }

    public void ChangeMusique(string name, float speed = 0.2f, float maxVolume = 0.5f) {
        if (currentMusic == null) {
            instance.StartCoroutine(FadeIn(name, speed, maxVolume));
            currentMusic = name;
        }

        if (currentMusic != name && valideChange) {
            instance.StartCoroutine(TimeChange());
            instance.StartCoroutine(FadeOut(name, speed, maxVolume));
        }

    }

    IEnumerator FadeIn(string name, float speed, float maxVolume) {
        keepFadingIn = true;
        keepFadingOut = false;
        int nb = Array.FindIndex(sounds, sound => sound.name == name);
        if (nb < 0) {
            Debug.LogWarning("Le son " + name + " n'a pas été trouvé!");
            keepFadingIn = false;
            yield break;
        }

        sounds[nb].source.volume = 0;
        float audioVolume = sounds[nb].source.volume;

        if (keepFadingIn)
            Play(name);

        while (sounds[nb].source.volume < maxVolume && keepFadingIn) {
            audioVolume += speed;
            sounds[nb].source.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }

        keepFadingIn = false;
    }

    IEnumerator FadeOut(string name, float speed, float maxVolume) {
        keepFadingIn = false;
        keepFadingOut = true;

        int nb = Array.FindIndex(sounds, sound => sound.name == currentMusic);
        if (nb < 0) {
            Debug.LogWarning("Le son " + currentMusic + " n'a pas été trouvé!");
            keepFadingOut = false;
            yield break;
        }

        float audioVolume = sounds[nb].source.volume;

        while (sounds[nb].source.volume >= speed && keepFadingOut) {
            audioVolume -= speed;
            sounds[nb].source.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }

        sounds[nb].source.Stop();
        keepFadingOut = false;

        instance.StartCoroutine(FadeIn(name, speed, maxVolume));
        currentMusic = name;
    }

    IEnumerator TimeChange() {
        valideChange = false;

        yield return new WaitForSeconds(0.5f);

        valideChange = true;
    }



}
