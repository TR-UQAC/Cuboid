using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddForce : MonoBehaviour {

    private Rigidbody2D rb;
    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update() {
        rb.AddForce(transform.right * 10f);

    }
}
