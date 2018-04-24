using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusique : MonoBehaviour {
    //public string musique;
    public float speed = 0.2f;
    public float volume = 0.25f;
    private AudioManager audioManager;

    private bool dispoChange = true;

    private string currentMusique = "Musique_Jeu";

    void Start() {
        audioManager = FindObjectOfType<AudioManager>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        GameObject go = other.gameObject;
        if (go.tag == "Player" && audioManager != null && dispoChange) {
            //dispoChange = false;
            StartCoroutine(TimeChange());

            if (currentMusique == "Musique_Jeu")
                currentMusique = "MusiqueJeu2";
            else
                currentMusique = "Musique_Jeu";

            audioManager.ChangeMusique(currentMusique, speed, volume);

        }

    }

    IEnumerator TimeChange() {
        dispoChange = false;

        yield return new WaitForSeconds(0.2f);

        dispoChange = true;
    }

}
