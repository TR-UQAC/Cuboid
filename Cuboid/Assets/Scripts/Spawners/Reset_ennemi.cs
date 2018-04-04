using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_ennemi : MonoBehaviour {
    private AllSpawners all;
    private bool disponible = true;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Spawn_Folder") != null)
            all = GameObject.Find("Spawn_Folder").GetComponent<AllSpawners>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!disponible)
            return;

        disponible = false;
        StartCoroutine(ChangeDisponile());

        GameObject go = other.gameObject;
        if (go.tag == "Player")
            all.SpawnAllEnnemis();
    }

    IEnumerator ChangeDisponile() {
        yield return new WaitForSeconds(1f);
        disponible = true;
    }
}
