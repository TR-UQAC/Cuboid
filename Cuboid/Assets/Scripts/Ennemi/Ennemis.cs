using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Ennemis : Personnages {
    
    public enum typeAttaque { Rien = 0, Tirer = 1, Kamikaze = 2, Explosion = 3}
    public enum typeDeplac { Immobile = 0, Voler = 1, Glisser = 2 }

    public float distActivation = 50;

    private PlayerCharacter2D en;
    private bool searchingForPlayer = false;

    public PersoStats ennemiStats = new PersoStats();
    public Comportement comp;

    private Rigidbody2D rb;
    private EnnemiAI artIntel;
    private WeaponEnnemi weapon;
    private PatrolControl control;

    public Vector2 direction;
    public bool facingRight = false;
    private Transform myTransform;

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
        if (!ennemiStats.immortel) {
            ennemiStats.vie -= dommage;

            if (ennemiStats.vie <= 0) {
                GameMaster.KillEnnemi(this);
                //TimeManager.DoSlowMotion();
            }
        }
    }

    private void Start() {
        myTransform = transform;
        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;

        if (GetComponent<EnnemiAI>() != null) {
            ia = true;
            artIntel = GetComponent<EnnemiAI>() as EnnemiAI;
        }

        if (GetComponent<WeaponEnnemi>() != null)
            weapon = GetComponent<WeaponEnnemi>() as WeaponEnnemi;

        if (GetComponent<PatrolControl>() != null)
            control = GetComponent<PatrolControl>() as PatrolControl;

        rb.gravityScale = (comp.deplacement == typeDeplac.Voler) ? 0 : rb.gravityScale;

        if (en == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        //enabled = false;
        StartCoroutine(CheckDistance());
    }

    private void FixedUpdate() {

        if (ia)
            artIntel.iaUpdate();
        else if (control != null)
            direction = control.checkDirection();

        if (en == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        if (comp.deplacement != typeDeplac.Immobile)
            Deplacement(direction);
        else if (ia)
            DirectionTarget();

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
            case typeDeplac.Voler:
            case typeDeplac.Glisser:
                dir *= ennemiStats.speed * Time.fixedDeltaTime;
                rb.AddRelativeForce(dir, ennemiStats.fMode);

                if (rb.velocity.magnitude > ennemiStats.maxSpeed)
                    rb.velocity = rb.velocity.normalized * ennemiStats.maxSpeed;
                break;

            default:
                break;
        }

        if (!ia)
            facingRight = (dir.x > 0) ? true : false;
        else DirectionTarget();


    }

    void DirectionTarget() {
        if (artIntel.target.transform.position.x < myTransform.position.x)
            facingRight = false;
        else
            facingRight = true;
    }

    [System.Serializable]
    public class Comportement {

        public bool contact;
        public int dmgContact = 0;

        public typeAttaque attaque;

        public int dmgAttaque;
        [Tooltip("Mise à jour dans le start de WeaponEnnemi")]
        public float fireRate = 0.5f;

        [Tooltip("Mise à jour dans le start de WeaponEnnemi")]
        public float ePower;
        [Tooltip("Mise à jour dans le start de WeaponEnnemi")]
        public float eRadius;
        [Tooltip("Mise à jour dans le start de WeaponEnnemi")]
        public float upwardsModifier;

        public typeDeplac deplacement;
    }

    //**** Désactivation des ennemis quand il ne sont pas vu ****//
    /*
    void OnBecameVisible() {
        enabled = true;
    }
    */
    /*
    void OnBecameInvisible() {
        enabled = false;
    }
    */

    //**** Activation/ Desactivation par rapport à la distance ****//
    IEnumerator CheckDistance() {
        //Debug.Log("Check dist : " + distActivation);
        if (en == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield break;
        }

        if ((en.transform.position - myTransform.position).sqrMagnitude < distActivation * distActivation)
            enabled = true;
        else
            enabled = false;

        yield return new WaitForSeconds(5f);
        StartCoroutine(CheckDistance());
    }

    IEnumerator SearchForPlayer() {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null) {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        } else {
            en = sResult.GetComponent<PlayerCharacter2D>() as PlayerCharacter2D;
            searchingForPlayer = false;
            StartCoroutine(CheckDistance());
            yield break;
        }
    }
}