using System;
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

    private bool ia = false;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")
            DommagePerso(50);
    }

    public override void DommagePerso(int dommage) {
        ennemiStats.vie -= dommage;
        if (ennemiStats.vie <= 0) {
            GameMaster.KillEnnemi(this);
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (this.GetComponent<EnnemiAI>() != null)
            ia = true;
    }

    private void FixedUpdate() {
        if(!ia)
        if (comp.deplacement == typeDeplac.Glisser) {
            Deplacement(new Vector3(-1,0,0));
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
    
}