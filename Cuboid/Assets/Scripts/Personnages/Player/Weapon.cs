using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float fireCooldown = 0;
    public int dmg = 100;
    public LayerMask noHit;
    public LayerMask dommageHit;

    public GameObject bulletPref;
    private Transform firePoint;

    //Les parametres pour les explosion
    public DegatAttaque statAttaque;
    /*
    [Header("Paramètre de force explosive")]
    public float eForce;
    public float eRadius;
    public float upwardsModifier;
    */
    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("FirePoint not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Shoot(bool facingRight)
    {
        //TODO: Effet particule de tir
        Bullet bul = Instantiate(bulletPref, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        bul.direction.x = (facingRight) ? 1 : -1;

        bul.noHit = noHit;
        bul.dommageHit = dommageHit;
        //bul.facingRight = facingRight;
        bul.dmg = dmg;
        bul.statAttaque = statAttaque;
        /*
        bul.eForce = eForce;
        bul.eRadius = eRadius;
        bul.upwardsModifier = upwardsModifier;
        */
        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().Play("Shoot");
        }   
    }
}



