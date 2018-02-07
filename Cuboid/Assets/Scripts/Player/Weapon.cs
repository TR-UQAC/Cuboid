using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public int fireRate = 0;
    public LayerMask noHit;
    public LayerMask dommageHit;

    public GameObject bulletPref;
    private Transform firePoint;

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
        Bullet bul = Instantiate(bulletPref, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        bul.noHit = noHit;
        bul.dommageHit = dommageHit;
        bul.facingRight = facingRight;

        FindObjectOfType<AudioManager>().Play("Shoot");
    }
}



