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

    private GameObject tmpPlayer;

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

    public IEnumerator RespawnPlayer(PlayerCharacter2D perso) {
        //TODO: Ajout d'un son pour l'attente
        yield return new WaitForSeconds(spawnDelay);

        perso.gameObject.transform.position = spawnPoint.position;
        perso.gameObject.transform.rotation = spawnPoint.rotation;
        perso.SoinPerso(999999);
        perso.gameObject.SetActive(true);

        if (playerPrefab != null)
            //Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
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

    private void DestroyPlayer(PlayerCharacter2D perso)
    {
        Destroy(perso.gameObject);
    }

    public static void KillJoueur(PlayerCharacter2D perso)
    {
        //instance.DestroyPlayer(perso);
        //Destroy(perso.gameObject);
        perso.gameObject.SetActive(false);
        //instance.RespawnPlayer(perso);
        instance.StartCoroutine(instance.RespawnPlayer(perso));
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
