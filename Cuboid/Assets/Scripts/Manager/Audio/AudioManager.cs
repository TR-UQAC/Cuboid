﻿using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

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
        Play("Musique_Jeu");
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

    public void ChangeMusique(string nameStop, string namePlay) {

        Stop(nameStop);

        Play(namePlay);
    }
}
