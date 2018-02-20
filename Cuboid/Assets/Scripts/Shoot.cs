using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {
    public GameObject bulletPrefab;
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey) {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
	}
}
