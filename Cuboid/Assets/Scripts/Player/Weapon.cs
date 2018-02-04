using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public int fireRate = 0;
    public LayerMask noHit;

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

    public void Shoot()
    {
        Debug.Log("Pew");
    }
}



