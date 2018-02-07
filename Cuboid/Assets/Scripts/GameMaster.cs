using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster instance;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;
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

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        //TODO: Effet particule de spawn
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
