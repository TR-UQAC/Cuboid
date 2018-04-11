using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class WeaponEnnemi : MonoBehaviour {

    public GameObject bulletPrefab;

    public LayerMask noHit;
    public LayerMask dommageHit;

    private Transform firePoint;
    public  Transform effetAttaquePrefab;

    private float attaqueCooldown;

    public bool SonDefaut = true;

    //Les parametres pour les explosion
    public DegatAttaque statAttaque;

    private Transform my_transform;
    // Use this for initialization
    void Start () {
        attaqueCooldown = 0.1f;

        my_transform = transform;
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

        //Debug.DrawRay(my_transform.position, CibleDirection(), Color.green);
    }

    public void Tirer(Vector2 dir, int dmg, float fireRate, bool cibler = false) {
        if (CanAttack) {
            //TODO: Création d'un effet tirer
            if (effetAttaquePrefab != null) {
                Transform clone = Instantiate(effetAttaquePrefab, firePoint.position, firePoint.rotation) as Transform;
                Destroy(clone.gameObject, 3f);
            }

            if (bulletPrefab == null) {
                Debug.LogWarning("Il n'y a aucun prefab de balle dans " + name);
                return;
            }

            if (cibler) {
                //le ~noHit veut dire qu'il prend en compte tout ce qui n'est pas dans le LayerMask noHit
                RaycastHit2D hit = Physics2D.Raycast(my_transform.position, CibleDirection(), 20f, ~noHit);

                if (hit.collider == null || hit.collider.tag != "Player")
                    return;
            }

            attaqueCooldown = fireRate;

            Bullet bul = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>() as Bullet;
            bul.direction = (cibler) ? CibleDirection() : dir;
            bul.noHit = noHit;
            bul.dommageHit = dommageHit;
            bul.dmg = dmg;

            bul.statAttaque = statAttaque;

            if (FindObjectOfType<AudioManager>() != null && SonDefaut == true) {
                FindObjectOfType<AudioManager>().Play("Shoot");
            }
        }
    }

    public void Explosion(int dmg, PlayerCharacter2D pl, float fireRate) {
        if (CanAttack) {
            if (effetAttaquePrefab != null) {
                Transform clone = Instantiate(effetAttaquePrefab, firePoint.position, firePoint.rotation) as Transform;
                ShockWaveForce wave = clone.GetComponent<ShockWaveForce>();
                wave.radius = statAttaque.eRadius*1.3f;

                Destroy(clone.gameObject, 1f);
            }

            attaqueCooldown = fireRate;

            if (Rigidbody2DExt.AddExplosionForce(pl.GetComponent<Rigidbody2D>(), statAttaque.ePower, firePoint.position, statAttaque.eRadius, statAttaque.upwardsModifier))
                pl.DommagePerso(dmg);
        }
    }

    public void Contact(int dmg, PlayerCharacter2D pl, float f) {
        //Rigidbody2DExt.AddExplosionForce(pl.GetComponent<Rigidbody2D>(), f, firePoint.position, r, upM, ForceMode2D.Force);
        var dir = my_transform.position - pl.transform.position;
        //Debug.Log(dir);
        Vector3 baseForce = -dir.normalized*f;

        baseForce.x *= 2f;
        //baseForce.x += Mathf.Sign(baseForce.x)*10f;

        if (dir.y > -0.9f && dir.y < 0.9f) {
            Vector2 upp = Vector2.up * 10f;
            pl.GetComponent<Rigidbody2D>().AddForce(upp, ForceMode2D.Impulse);
            baseForce.y = 0;
        }

        //Debug.Log(baseForce);
        pl.GetComponent<Rigidbody2D>().AddForce(baseForce);
        pl.DommagePerso(dmg);
    }

    private Vector2 CibleDirection() {

        if(GameObject.FindGameObjectWithTag("Player") == null)
            return Vector2.zero;

        Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;

         return playerPos.position - my_transform.position;
    }

    public bool CanAttack {
        get {
            return attaqueCooldown <= 0f;
        }
    }
}
