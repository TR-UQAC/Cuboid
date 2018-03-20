using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{

    public float fireCooldown = 0;
    public int dmg = 100;
    public LayerMask noHit;
    public LayerMask dommageHit;

    public GameObject bulletPref;
    public GameObject missilePref;
    private GameObject activeBullet;
    private Transform firePoint;
    private GameObject missileUI;

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
        activeBullet = bulletPref;

        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("FirePoint not found!");
        }

        if (GameObject.FindGameObjectWithTag("MissileUI"))
        {
            missileUI = GameObject.FindGameObjectWithTag("MissileUI");
            missileUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateGUI(bool on)
    {
        if (!missileUI.activeSelf)
        {
            missileUI.SetActive(true);
        }

        if (on)
        {
            missileUI.GetComponent<Image>().color = new Vector4(1, 1, 0, 1);
        }
        else
        {
            missileUI.GetComponent<Image>().color = new Vector4(1, 1, 0, 0.098f);
        }
    }

    public void UseMissile(bool on)
    {
        if (on)
        {
            activeBullet = missilePref;
        }
        else
        {
            activeBullet = bulletPref;
        }
    }

    public void Shoot(bool facingRight)
    {
        //TODO: Effet particule de tir
        Bullet bul = Instantiate(activeBullet, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
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



