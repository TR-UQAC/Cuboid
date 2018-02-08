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

    public bool facingRight;

    private bool ia = false;


    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject go = collision.gameObject;
        if (go.tag == "Player" && comp.contact) {
            PlayerCharacter2D en = (PlayerCharacter2D)go.GetComponent(typeof(PlayerCharacter2D));
            en.DommagePerso(comp.dmgContact);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "KillZone")
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
        rb = GetComponent<Rigidbody2D>();
        if (this.GetComponent<EnnemiAI>() != null)
            ia = true;

        if (this.GetComponent<WeaponEnnemi>() != null)
            weapon = GetComponent<WeaponEnnemi>();
    }

    private void FixedUpdate() {
        if(!ia)
        if (comp.deplacement == typeDeplac.Glisser) {
            Deplacement(new Vector3(-1,0,0));
        }

        Attaque();
    }

    public void Attaque() {
        switch (comp.attaque) {
            case typeAttaque.Rien:
                //Debug.Log("Rien");
                break;

            case typeAttaque.Tirer:
                weapon.Tirer(facingRight, comp.dmgAttaque);
                break;

            default:
                break;
        }
    }

    public void Deplacement(Vector3 dir) {
        switch (comp.deplacement) {
            case typeDeplac.Immobile:
                //Debug.Log("Immobile");
                break;

            case typeDeplac.Glisser:
                //Debug.Log("Glisser");
                dir *= ennemiStats.speed * Time.fixedDeltaTime;
                rb.AddForce(dir, ennemiStats.fMode);
                break;

            case typeDeplac.Voler:
                //Debug.Log("Voler");
                dir *= ennemiStats.speed * Time.fixedDeltaTime;

                rb.AddForce(dir, ennemiStats.fMode);
                break;

            default:
                break;
        }
        
        if (comp.deplacement != typeDeplac.Immobile) {
            if (dir.x > 0)
                facingRight = true;
            else
                facingRight = false;
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
}