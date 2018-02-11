using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(WeaponEnnemi))]
public class Ennemis : Personnages {
    
    public enum typeAttaque { Rien = 0, Tirer = 1, Kamikaze = 2, Explosion = 3}
    public enum typeDeplac { Immobile = 0, Voler = 1, Glisser = 2 }

    private PlayerCharacter2D en;
    private bool searchingForPlayer = false;

    public PersoStats ennemiStats = new PersoStats();

    private Rigidbody2D rb;
    private WeaponEnnemi weapon;
    private PatrolControl control;
    public Comportement comp;

    public Vector2 direction;
    public bool facingRight = false;

    private bool ia = false;

    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject go = collision.gameObject;
        if (go.tag == "Player") {

            if(comp.contact)
                en.DommagePerso(comp.dmgContact);
            
            if (comp.attaque == typeAttaque.Kamikaze) {
                weapon.Explosion(comp.dmgAttaque, en);
                GameMaster.KillEnnemi(this);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject go = collision.gameObject;
        if (go.tag == "KillZone")
            GameMaster.KillEnnemi(this);
    }

    public override void DommagePerso(int dommage) {
        ennemiStats.vie -= dommage;
        if (ennemiStats.vie <= 0) {
            GameMaster.KillEnnemi(this);
            //TimeManager.DoSlowMotion();
        }
    }

    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        if (this.GetComponent<EnnemiAI>() != null) {
            ia = true;
        } else
            direction = new Vector3(-1f, 0f, 0f);

        if (this.GetComponent<WeaponEnnemi>() != null)
            weapon = this.GetComponent<WeaponEnnemi>();

        if (this.GetComponent<PatrolControl>() != null)
            control = this.GetComponent<PatrolControl>();

        rb.gravityScale = (comp.deplacement == typeDeplac.Voler) ? 0 : rb.gravityScale;

        if (en == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        enabled = false;
    }

    private void FixedUpdate() {

        if (!ia) {
            direction = control.checkDirection();
        } else
            GetComponent<EnnemiAI>().iaUpdate();

        if (en == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        if (comp.deplacement != typeDeplac.Immobile)
            Deplacement(direction);

            Attaque();
    }

    public void Attaque() {
        switch (comp.attaque) {
            case typeAttaque.Tirer:
                weapon.Tirer(facingRight, comp.dmgAttaque);
                break;

            case typeAttaque.Explosion:
                weapon.Explosion(comp.dmgAttaque, en);
                break;

            default:
                break;
        }
    }

    public void Deplacement(Vector2 dir) {

        switch (comp.deplacement) {
            case typeDeplac.Glisser:
                dir *= ennemiStats.speed * Time.fixedDeltaTime;
                rb.AddRelativeForce(dir, ennemiStats.fMode);

                if (rb.velocity.magnitude > ennemiStats.maxSpeed)
                    rb.velocity = rb.velocity.normalized * ennemiStats.maxSpeed;
                break;

            case typeDeplac.Voler:
                dir *= ennemiStats.speed * Time.fixedDeltaTime;
                rb.AddForce(dir, ennemiStats.fMode);
                break;

            default:
                break;
        }

        if (!ia)
            facingRight = (dir.x > 0) ? true : false;
        else {
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
        public float fireRate = 0.5f;

        public float ePower;
        public float eRadius;
        public float upwardsModifier;

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
    IEnumerator SearchForPlayer() {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null) {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        } else {
            en = (PlayerCharacter2D)sResult.GetComponent(typeof(PlayerCharacter2D));
            searchingForPlayer = false;
            yield break;
        }
    }
}