using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Canon : MonoBehaviour {

    //public Vector2 direction;
    public int dmgAttaque;
    public float fireRate = 15f;
    public bool facingRight = false;
    public LayerMask noHit;
    public LayerMask dommageHit;

    public GameObject bulletPrefab;
    public Transform effetAttaquePrefab;

    public bool SonDefaut = true;

    private Vector2 directionTir = new Vector2(0, 0);
    private Transform firePoint;

    private float attaqueCooldown;


    private bool estActif = false;


    public bool CanAttack {
        get {
            return attaqueCooldown <= 0f;
        }
    }
    // Use this for initialization
    void Start () {
        directionTir.x = (facingRight) ? 1 : -1;

        firePoint = transform.Find("FirePoint");
        if (firePoint == null) {
            Debug.LogError("FirePoint not found!");
        }
    }

    void Update() {
        if (!estActif)
            return;

        if (attaqueCooldown > 0)
            attaqueCooldown -= Time.deltaTime;
        else
            Tirer();
    }

    private void Tirer() {
        if (CanAttack && estActif) {
            //TODO: Création d'un effet tirer
            if (effetAttaquePrefab != null) {
                Transform clone = Instantiate(effetAttaquePrefab, firePoint.position, firePoint.rotation) as Transform;
                Destroy(clone.gameObject, 3f);
            }

            if (bulletPrefab == null) {
                Debug.LogWarning("Il n'y a aucun prefab de balle dans " + name);
                return;
            }

            attaqueCooldown = fireRate;

            Bullet bul = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>() as Bullet;
            bul.direction = directionTir;
            bul.noHit = noHit;
            bul.dommageHit = dommageHit;
            bul.dmg = dmgAttaque;

            //bul.statAttaque = statAttaque;

            if (FindObjectOfType<AudioManager>() != null && SonDefaut == true) {
                FindObjectOfType<AudioManager>().Play("Shoot");
            }
        }
    }

    public void Detruire() {
        GameMaster gm = GameObject.Find("_GM").GetComponent<GameMaster>();
        GameObject ex = Instantiate(gm.m_explosionEnnemis, transform.position, transform.rotation);
        FindObjectOfType<AudioManager>().Play("SmallExplosion");

        Sequence explose = DOTween.Sequence();
        explose.SetDelay(1.0f);
        explose.AppendCallback(() =>
        {
            Destroy(ex);
        });
        explose.Play();

        GameMaster.KillCanon(this);
    }
    public void Entrer() {
        attaqueCooldown = fireRate;
        estActif = true;
    }
    public void Sortir() {
        attaqueCooldown = fireRate;
        estActif = false;
    }
}
