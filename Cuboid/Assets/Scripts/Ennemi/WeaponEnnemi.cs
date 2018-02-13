﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WeaponEnnemi : MonoBehaviour {

    public GameObject bulletPrefab;

    public LayerMask noHit;
    public LayerMask dommageHit;

    private Transform firePoint;
    public  Transform effetAttaquePrefab;

    //private float fireRate;
    private float attaqueCooldown;

    //Les parametres pour les explosion
    [Header("Paramètre de force explosive")]
    public float eForce;
    public float eRadius;
    public float upwardsModifier;
    
    // Use this for initialization
    void Start () {
        attaqueCooldown = 0f;
    }

    void Awake() {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null) {
            Debug.LogError("FirePoint not found!");
        }
    }

    void Update () {
        if (attaqueCooldown > 0) {
            attaqueCooldown -= Time.deltaTime;
        }
    }

    public void Tirer(Vector2 dir, int dmg, float fireRate, bool cibler = false) {
        //public void Tirer(Vector2 direction, int dmg, float fireRate) {
        if (CanAttack) {
            //TODO: Création d'un effet tirer
            if (effetAttaquePrefab != null) {
                Transform clone = Instantiate(effetAttaquePrefab, firePoint.position, firePoint.rotation) as Transform;
                Destroy(clone.gameObject, 3f);
            }

            attaqueCooldown = fireRate;

            if (bulletPrefab == null) {
                Debug.LogWarning("Il n'y a aucun prefab de balle dans " + name);
                return;
            }
            Bullet bul = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>() as Bullet;
            if (cibler) {
                bul.direction = cibleDirection();
            } else {
                bul.direction = dir;
            }

            bul.noHit = noHit;
            bul.dommageHit = dommageHit;
            //bul.direction = direction;
            //bul.facingRight = facingRight;
            bul.dmg = dmg;

            bul.eForce          = eForce;
            bul.eRadius         = eRadius;
            bul.upwardsModifier = upwardsModifier;

            if (FindObjectOfType<AudioManager>() != null) {
                FindObjectOfType<AudioManager>().Play("Shoot");
            }
        }
    }

    public void Explosion(int dmg, PlayerCharacter2D pl, float fireRate) {
        if (CanAttack) {
            //TODO: Création d'un effet EXPLOSION
            if (effetAttaquePrefab != null) {
                Transform clone = Instantiate(effetAttaquePrefab, firePoint.position, firePoint.rotation) as Transform;
                Destroy(clone.gameObject, 3f);
            }

            attaqueCooldown = fireRate;

            if (Rigidbody2DExt.AddExplosionForce(pl.GetComponent<Rigidbody2D>(), eForce, firePoint.position, eRadius, upwardsModifier))
                pl.DommagePerso(dmg);


            //TODO: ajouter un caméra shake
        }
    }

    public void Contact(int dmg, PlayerCharacter2D pl, float f, float r, float upM) {
        Rigidbody2DExt.AddExplosionForce(pl.GetComponent<Rigidbody2D>(), f, firePoint.position, r, upM);
        pl.DommagePerso(dmg);
        Debug.Log("Dommage contact");
    }

    private Vector2 cibleDirection() {
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        return playerPos.position - transform.position;
    }

    public bool CanAttack {
        get {
            return attaqueCooldown <= 0f;
        }
    }
}
