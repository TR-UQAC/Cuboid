using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(WeaponEnnemi))]
public class Ennemis : Personnages {
    
    public enum typeAttaque { Rien = 0, Tirer = 1, Kamikaze = 2}
    public enum typeDeplac { Immobile = 0, Voler = 1, Glisser = 2 }

    public PersoStats ennemiStats = new PersoStats();

    private Rigidbody2D rb;
    private WeaponEnnemi weapon;
    public Comportement comp;

    public Vector3 direction;
    public bool facingRight;

    private bool ia = false;


    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject go = collision.gameObject;
        if (go.tag == "Player") {

            PlayerCharacter2D en = (PlayerCharacter2D)go.GetComponent(typeof(PlayerCharacter2D));

            if(comp.contact)
                en.DommagePerso(comp.dmgContact);

            if (comp.attaque == typeAttaque.Kamikaze) {
                weapon.Kamikaze(comp.dmgAttaque, en);
                GameMaster.KillEnnemi(this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        string _tag = collision.gameObject.tag;
        if (_tag == "KillZone")
            GameMaster.KillEnnemi(this);
    }

    public override void DommagePerso(int dommage) {
        ennemiStats.vie -= dommage;
        if (ennemiStats.vie <= 0) {
            GameMaster.KillEnnemi(this);
            TimeManager.DoSlowMotion();
        }
    }

    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        if (this.GetComponent<EnnemiAI>() != null) {
            ia = true;
        } else
            direction = new Vector3(-1, 0, 0);

        if (this.GetComponent<WeaponEnnemi>() != null)
            weapon = this.GetComponent<WeaponEnnemi>();

        enabled = false;
    }

    private void FixedUpdate() {

        Deplacement(direction);
        Attaque();
    }

    public void Attaque() {
        switch (comp.attaque) {
            case typeAttaque.Tirer:
                weapon.Tirer(facingRight, comp.dmgAttaque);

                break;

            default:
                break;
        }
    }

    public void Deplacement(Vector2 dir) {
        switch (comp.deplacement) {
            case typeDeplac.Immobile:
                break;

            case typeDeplac.Glisser:
                //TODO: !Améliorer Glisser pour que l'ennemi change de direction quand il rencontre un obstacle ou du vide.
                /*
                //dir = (dir + transform.position).normalized;
                //dir *=  ennemiStats.speed * Time.fixedDeltaTime;
                //dir -= transform.position;
                dir *= ennemiStats.speed;
                Debug.Log(-transform.right * ennemiStats.speed);
                //rb.AddForce(dir, ennemiStats.fMode);
                rb.velocity = -transform.right * ennemiStats.speed;
                */
                dir *= ennemiStats.speed;
                rb.AddForce(dir, ennemiStats.fMode);

                break;

            case typeDeplac.Voler:
                dir *= ennemiStats.speed * Time.fixedDeltaTime;
                rb.AddForce(dir, ennemiStats.fMode);
                break;

            default:
                break;
        }

        if (!ia) {
            if (comp.deplacement != typeDeplac.Immobile) {
                if (dir.x > 0)
                    facingRight = true;
                else
                    facingRight = false;
            }
        } else {
            if (this.GetComponent<EnnemiAI>().target.transform.position.x < this.transform.position.x)
                facingRight = false;
            else
                facingRight = true;
            }

    }

    [System.Serializable]
    public class Comportement {

        public bool contact;
        public int dmgContact = 0;

        public typeAttaque attaque;
        public int dmgAttaque;
        public typeDeplac deplacement;
    }

    
    //**** Désactivation des ennemis quand il ne sont pas vu ****//
    
    void OnBecameVisible() {
        enabled = true;

    }
    /*
    void OnBecameInvisible() {
        enabled = false;
    }
    */
}