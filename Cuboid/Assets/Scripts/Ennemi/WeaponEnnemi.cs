using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnnemi : MonoBehaviour {

    public GameObject attaquePrefab;

    public LayerMask noHit;
    public LayerMask dommageHit;

    private Transform firePoint;
    public  Transform attackPrefab;

    private float fireRate;
    private float attaqueCooldown;

    //Les parametres pour les explosion
    private float ePower;
    private float eRadius;
    private float upwardsModifier;

    // Use this for initialization
    void Start () {
        Ennemis.Comportement comp = GetComponent<Ennemis>().comp;
        attaqueCooldown = 0f;

        ePower          = comp.ePower;
        eRadius         = comp.eRadius;
        fireRate       = comp.fireRate;
        upwardsModifier = comp.upwardsModifier;

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

    public void Tirer(bool facingRight, int dmg) {
        if (CanAttack) {
            //TODO: Création d'un effet tirer
            if (attackPrefab != null) {
                Transform clone = Instantiate(attackPrefab, firePoint.position, firePoint.rotation) as Transform;
                Destroy(clone.gameObject, 3f);
            }

            attaqueCooldown = fireRate;

            Bullet bul = Instantiate(attaquePrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
            bul.noHit = noHit;
            bul.dommageHit = dommageHit;
            bul.facingRight = facingRight;
            bul.dmg = dmg;

            FindObjectOfType<AudioManager>().Play("Shoot");
        }
    }

    public void Explosion(int dmg, PlayerCharacter2D pl) {
        if (CanAttack) {
            //TODO: Répultion du joueur
            //TODO: Création d'un effet EXPLOSION
            if (attackPrefab != null) {
                Transform clone = Instantiate(attackPrefab, firePoint.position, firePoint.rotation) as Transform;
                Destroy(clone.gameObject, 3f);
            }

            attaqueCooldown = fireRate;

            if (pl.GetComponent<Rigidbody2D>().AddExplosionForce(ePower, firePoint.position, eRadius, upwardsModifier))
                pl.DommagePerso(dmg);

            Debug.Log("EXPLOSION");


            //TODO: ajouter un caméra shake
        }
    }


    public bool CanAttack {
        get {
            return attaqueCooldown <= 0f;
        }
    }
}
