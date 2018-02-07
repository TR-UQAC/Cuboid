using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnnemi : MonoBehaviour {

    public GameObject attaquePrefab;

    public LayerMask noHit;
    public LayerMask dommageHit;

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


    public bool CanAttack {
        get {
            return attaqueCooldown <= 0f;
        }
    }
}
