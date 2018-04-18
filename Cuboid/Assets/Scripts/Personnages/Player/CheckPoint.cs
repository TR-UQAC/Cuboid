using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    private Animator m_anim;
    private bool m_currentState = false;

	// Use this for initialization
	void Start () {
        m_anim = GetComponent<Animator>() as Animator;
        m_anim.GetComponent<Animator>().SetBool("CurrentSpawner", false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (m_currentState == false)
            {
                m_anim.GetComponent<Animator>().SetBool("CurrentSpawner", true);
                m_currentState = true;
            }
        }
    }
}
