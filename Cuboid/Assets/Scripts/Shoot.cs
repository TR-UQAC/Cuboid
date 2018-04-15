using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {
    public GameObject bulletPrefab;

    public float fireRate = 0.2f;
    private float attaqueCooldown;

    public bool CanAttack {
        get {
            return attaqueCooldown <= 0f;
        }
    }

    // Update is called once per frame
    void Update () {

        if (attaqueCooldown > 0)
            attaqueCooldown -= Time.deltaTime;

        if (Input.anyKey && CanAttack) {
            Instantiate(bulletPrefab, transform.position, transform.rotation);

            attaqueCooldown = fireRate;
        }
	}
}
