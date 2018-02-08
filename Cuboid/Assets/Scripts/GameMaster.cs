using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster instance;

    public int spawnDelay = 2;

    public Transform spawnPoint;
    public Transform spawnPrefab;
    public Transform playerPrefab;


    void Awake() {
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator RespawnPlayer() {
        //TODO: Ajout d'un son pour l'attente
        yield return new WaitForSeconds(spawnDelay);
       
        if(playerPrefab != null)
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        if (spawnPrefab != null) {
            Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
            Destroy(clone.gameObject, 3f);
        }
    }

    public static void KillJoueur(PlayerCharacter2D perso) {
        Destroy(perso.gameObject);
        instance.StartCoroutine(instance.RespawnPlayer());
    }

    public static void SetSpawnPlayer(Transform spawn) {
        instance.spawnPoint = spawn;
    }

    public static void KillEnnemi(Ennemis perso) {
        Destroy(perso.gameObject);
    }
}
