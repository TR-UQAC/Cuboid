﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Ennemis : Personnages {
    
    public enum typeAttaque { Rien = 0, Tirer = 1, Kamikaze = 2}
    public enum typeDeplac { Immobile = 0, Voler = 1, Glisser = 2 }

    public PersoStats ennemiStats = new PersoStats();

    private Rigidbody2D rb;
    public Comportement comp;

    public int fireRate;
    public LayerMask noHit;

    public GameObject bulletPref;
    private Transform firePoint;

    private bool ia = false;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")
            DommagePerso(100);
    }

    public override void DommagePerso(int dommage) {
        ennemiStats.vie -= dommage;
        if (ennemiStats.vie <= 0) {
            GameMaster.KillEnnemi(this);
            TimeManager.DoSlowMotion();
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (this.GetComponent<EnnemiAI>() != null)
            ia = true;
        /*
        switch (comp.attaque) {
            case typeAttaque.Rien:
                Debug.Log("Rien");
                break;
                
            case typeAttaque.Tirer:
                var go = Instantiate(weaponPrefab) as GameObject;
                go.transform.parent = this.transform;
                break;
                
            default:
                break;
        }
    */
    }

    void Awake() {
        switch (comp.attaque) {
            case typeAttaque.Rien:
                Debug.Log("Rien");
                break;

            case typeAttaque.Tirer:
                firePoint = transform.Find("FirePoint");
                if (firePoint == null) {
                    Debug.LogWarning("FirePoint not found!");
                }
                break;

            default:
                break;
        }
    
    }

    private void FixedUpdate() {
        if(!ia)
        if (comp.deplacement == typeDeplac.Glisser) {
            Deplacement(new Vector3(-1,0,0));
        }

        if(comp.attaque == typeAttaque.Tirer) {
            
        }
    }
    public void Attaque() {
        switch (comp.attaque) {
            case typeAttaque.Rien:
                Debug.Log("Rien");
                break;

            case typeAttaque.Tirer:
                Debug.Log("Tirer");
                break;

            default:
                break;
        }
    }

    public void Deplacement(Vector3 dir) {
        switch (comp.deplacement) {
            case typeDeplac.Immobile:
                Debug.Log("Immobile");
                break;

            case typeDeplac.Glisser:
                Debug.Log("Glisser");
                dir *= ennemiStats.speed * Time.fixedDeltaTime;
                rb.AddForce(dir, ennemiStats.fMode);
                break;

            case typeDeplac.Voler:
                Debug.Log("Voler");
                dir *= ennemiStats.speed * Time.fixedDeltaTime;

                rb.AddForce(dir, ennemiStats.fMode);
                break;

            default:
                break;
        }
    }

    [System.Serializable]
    public class Comportement {

        public bool contact;
        public int dmgContact;

        public typeAttaque attaque;
        public int dmgAttaque;
        public typeDeplac deplacement;
    }

    /*
    #if UNITY_EDITOR
        //Put gui elements for custom inspector here
        public bool showVie = true;
    #endif
   */
}