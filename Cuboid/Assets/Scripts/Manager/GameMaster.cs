﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster instance;

    public int spawnDelay = 2;

    public Transform spawnPoint;
    public Transform spawnPrefab;
    public Transform playerPrefab;
    public GameObject itemPickupPrefab;


    void Awake() {
        Cursor.visible = false;
        
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        Physics2D.IgnoreLayerCollision(11, 11, true);

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator RespawnPlayer() {
        //TODO: Ajout d'un son pour l'attente
        yield return new WaitForSeconds(spawnDelay);
       
        if(playerPrefab != null)
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        if (spawnPrefab != null) {
            Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
            Destroy(clone.gameObject, 3f);
        }
    }

    public void ItemDrop(Ennemis perso)
    {
        float result = Random.Range(1, 100);
        if (result/100 > 0.5f)
        {
            GameObject clone = Instantiate(itemPickupPrefab, perso.transform.position, perso.transform.rotation);
            clone.GetComponent<PickupItem>().PickupType = "Health";
            clone.GetComponent<PickupItem>().Valeur = 10;
        }  
    }

    public static void KillJoueur(PlayerCharacter2D perso) {
        Destroy(perso.gameObject);
        instance.StartCoroutine(instance.RespawnPlayer());
    }

    public static void SetSpawnPlayer(Transform spawn) {
        instance.spawnPoint = spawn;
    }

    public static void KillEnnemi(Ennemis perso)
    {
        instance.ItemDrop(perso);

        //TODO: particule à la mort des ennemis
        Destroy(perso.gameObject);
    }

    public static void KillBoss(boss perso)
    {
        Destroy(perso.gameObject);
    }
}
