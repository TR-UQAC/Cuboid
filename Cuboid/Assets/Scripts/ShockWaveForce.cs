using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveForce : MonoBehaviour {

    public float radius = 10f;


    private ParticleSystem ps;
    private ParticleSystem postFx;

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
        postFx = this.transform.Find("PostFx").GetComponent<ParticleSystem>();

        var main = ps.main;
        main.startSizeMultiplier = radius;

        var main2 = postFx.main;
        main2.startSizeMultiplier = radius*2f;
    }

	// Update is called once per frame
	void Update () {
        
    }
}
