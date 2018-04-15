using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using System;


public class GameMaster : MonoBehaviour {

    public static GameMaster instance;

    public int spawnDelay = 2;

    public float escapeSeconds = 150;

    public Transform spawnPoint;
    public Transform spawnPrefab;
    public Transform playerPrefab;
    public GameObject itemPickupPrefab;
    public GameObject m_smokeEffect;
    public GameObject m_explosionEnnemis;

    private GameObject tmpPlayer;

    private GameObject escapeTimer;
    private bool timerRunning = false;
    private float currentTime = 0f;

    void Awake() {
        Cursor.visible = false;

        if (GameObject.FindGameObjectWithTag("EscapeTimerUI"))
        {
            escapeTimer = GameObject.FindGameObjectWithTag("EscapeTimerUI");
        }
        escapeTimer.SetActive(false);

        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        Physics2D.IgnoreLayerCollision(11, 11, true);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (timerRunning)
        {
            HandleTimer();
        }
        else
        {
            currentTime = escapeSeconds;
        }
    }

    public IEnumerator RespawnPlayer(PlayerCharacter2D perso) {
        //TODO: Ajout d'un son pour l'attente
        yield return new WaitForSeconds(spawnDelay);

        perso.gameObject.transform.position = spawnPoint.position;
        perso.gameObject.transform.rotation = spawnPoint.rotation;
        perso.SoinPerso(999999);

        //Detache le grappin si on meurt en etant attache
        if (perso.GetComponent<GrappleBeam>().isGrappleAttached)
        {
            perso.GetComponent<GrappleBeam>().UseGrapple();
        }
      
        //  si le joueur à été écrasé avant de mourir, réactive les controles et remet le scale comme il faut;
        if(perso.gameObject.transform.lossyScale.y < 1.5f)
        {
            Rigidbody2D rbp = perso.GetComponent<Rigidbody2D>() as Rigidbody2D;
            rbp.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            perso.setEnableInput(true);

            perso.gameObject.transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);
        }

        perso.gameObject.SetActive(true);

        if (playerPrefab != null)
            //Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        if (spawnPrefab != null) {
            Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
            Destroy(clone.gameObject, 3f);
        }

        perso.joueurStats.immortel = false;
    }

    public void ItemDrop(Ennemis perso)
    {
        float result = UnityEngine.Random.Range(1, 100);
        if (result/100 > 0.5f)
        {
            result = UnityEngine.Random.Range(1, 100);
            if (result/100 > 0.5f)          //1-50: Missile, 51-100: Health
            {
                PlayerCharacter2D pc = FindObjectOfType<PlayerCharacter2D>();
                if (pc.HasUpgrade("Missile"))
                {
                    GameObject clone = Instantiate(itemPickupPrefab, perso.transform.position, perso.transform.rotation);
                    clone.GetComponent<PickupItem>().PickupType = "Missile";
                    clone.GetComponent<Animator>().Play("Missile");
                    clone.GetComponent<PickupItem>().Valeur = 5;
                    //Debug.Log("missile drop");
                }
            }
            else
            {
                //Debug.Log("health drop");
                GameObject clone = Instantiate(itemPickupPrefab, perso.transform.position, perso.transform.rotation);
                clone.GetComponent<PickupItem>().PickupType = "Health";
                clone.GetComponent<Animator>().Play("Health");
                clone.GetComponent<PickupItem>().Valeur = 10;
            }
        }  
    }

    public void HandleTimer()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            escapeTimer.GetComponent<TextMeshProUGUI>().text = "00:00.00";
            timerRunning = false;
        }
        else
        {
            TimeSpan ts = TimeSpan.FromSeconds(currentTime);
            string txt = Math.Floor(ts.TotalMinutes).ToString("00") + ":" + (Math.Floor(ts.TotalSeconds) % 60).ToString("00") + "." + Math.Floor(ts.TotalMilliseconds) % 100;
            escapeTimer.GetComponent<TextMeshProUGUI>().text = txt;
        }
    }

    private void DestroyPlayer(PlayerCharacter2D perso)
    {
        Destroy(perso.gameObject);
    }

    public static void StartEscapeSequence()
    {
        instance.timerRunning = true;
        instance.escapeTimer.SetActive(true);
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

    public static void KillBossTP(bossTeleport perso)
    {
        Destroy(perso.gameObject);
    }
}
