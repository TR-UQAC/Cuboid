using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Ennemis : Personnages {
#region Variable
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
    public Vector2 directionTir = new Vector2(0,0);
    public bool facingRight = false;

    private Transform myTransform;

    private bool ia = false;
    public bool tirerSurJoueur = false;
    #endregion

#region Collision
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject go = collision.gameObject;

        if (go.tag == "Player") {
            if (comp.contact)
                weapon.Contact(comp.dmgContact, en, comp.statAttaque.ePower, comp.statAttaque.eRadius, comp.statAttaque.upwardsModifier);

            
            if (comp.attaque == typeAttaque.Kamikaze) {
                weapon.Explosion(comp.dmgAttaque, en, comp.fireRate);
                GameMaster.KillEnnemi(this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject go = collision.gameObject;
        if (go.tag == "KillZone")
            GameMaster.KillEnnemi(this);
    }
#endregion
    
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

        if (GetComponent<PatrolControl>() != null) {
            control = GetComponent<PatrolControl>() as PatrolControl;
            direction.x = control.direction.x;
            myTransform.localScale = new Vector2(myTransform.localScale.x * -direction.x, myTransform.localScale.y);
        }


        rb.gravityScale = (comp.deplacement == typeDeplac.Voler) ? 0 : rb.gravityScale;
        directionTir.x = (facingRight) ? 1 : -1;

        if (en == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
        }

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

        Attaque();
    }

    public void Attaque() {
        switch (comp.attaque) {
            case typeAttaque.Tirer:
                weapon.Tirer(directionTir, comp.dmgAttaque, comp.fireRate, tirerSurJoueur);
                break;

            case typeAttaque.Explosion:
                weapon.Explosion(comp.dmgAttaque, en, comp.fireRate);
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
                /*
                if (rb.velocity.x > ennemiStats.maxSpeed)
                    rb.velocity = rb.velocity.normalized * ennemiStats.maxSpeed;*/
                break;

            default:
                break;
        }

        if (ia && !tirerSurJoueur)
            DirectionTarget();
        else {
            direction = dir;
            directionTir = dir;
        }

    }

    void DirectionTarget() {
        if (artIntel.target.transform.position.x < myTransform.position.x)
            directionTir.x = -1;
        else
            directionTir.x = 1;
    }

    [System.Serializable]
    public class Comportement {

        public bool contact;
        public int dmgContact = 0;

        [SerializeField] public DegatAttaque statAttaque;
        public typeAttaque attaque;

        public int dmgAttaque;
        public float fireRate = 0.5f;

        public typeDeplac deplacement;
    }

#region Activation
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
#endregion
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

#if UNITY_EDITOR
    //Put gui elements for custom inspector here

    [HideInInspector] public bool showVie;
    [HideInInspector] public bool showDommage;
    [HideInInspector] public bool showContactDommage;
    [HideInInspector] public bool showMouvement;
    [HideInInspector] public bool showElements;
#endif

}