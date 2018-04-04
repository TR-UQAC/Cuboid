using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSpawners : MonoBehaviour {

    private GameObject[] ennemiSpawners;
    // Use this for initialization
    void Start () {
        ennemiSpawners = GameObject.FindGameObjectsWithTag("Spawner_Ennemi");
	}

    //Le racourci clavier pour cette fonction est "R"
    public void SpawnAllEnnemis () {
        if(ennemiSpawners != null)
        foreach (GameObject ennemiSpawner in ennemiSpawners) {
            ennemiSpawner.GetComponent<EnnemiSpawner>().SpawnEnnemi();
        }
    }
}
