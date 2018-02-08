using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnnemi : MonoBehaviour {

    public GameObject attaquePrefab;

    public LayerMask noHit;
    public LayerMask dommageHit;

    public Transform attackPrefab;
    private Transform firePoint;

    public float attaqueingRate = 0.25f;
    private float attaqueCooldown;


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

    public void Tirer(bool facingRight, int dmg) {
        if (CanAttack) {
            //TODO: Création d'un effet tirer
            if (attackPrefab != null) {
                Transform clone = Instantiate(attackPrefab, firePoint.position, firePoint.rotation) as Transform;
                Destroy(clone.gameObject, 3f);
            }

            attaqueCooldown = attaqueingRate;
            Vector3 newPosition = firePoint.position;
            if(facingRight)
                newPosition.x += this.transform.localScale.x;
            else
                newPosition.x -= this.transform.localScale.x;

            Bullet bul = Instantiate(attaquePrefab, newPosition, firePoint.rotation).GetComponent<Bullet>();
            bul.noHit = noHit;
            bul.dommageHit = dommageHit;
            bul.facingRight = facingRight;
            bul.dmg = dmg;

            FindObjectOfType<AudioManager>().Play("Shoot");
        }
    }

    public void Kamikaze(int dmg, PlayerCharacter2D pl) {
        //TODO: Création d'un effet EXPLOSION
        if (attackPrefab != null) {
            Transform clone = Instantiate(attackPrefab, firePoint.position, firePoint.rotation) as Transform;
            Destroy(clone.gameObject, 3f);
        }
        pl.DommagePerso(dmg);
        //TODO: ajouter un caméra shake
    }


    public bool CanAttack {
        get {
            return attaqueCooldown <= 0f;
        }
    }
}
