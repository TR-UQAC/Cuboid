using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    private Animator m_anim;
    private bool m_currentState = false;
    private bool m_endGameState = false;

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
        if (collision.tag == "Player" && m_endGameState == false)
        {
            if (m_currentState == false)
            {
                foreach (Transform check in transform.parent)
                {
                    if(check != transform)
                    {
                        Debug.Log(check.name);
                        check.GetComponent<Animator>().SetBool("CurrentSpawner", false);
                        check.GetComponent<CheckPoint>().m_currentState = false;
                    }
                }


                m_anim.GetComponent<Animator>().SetBool("CurrentSpawner", true);
                m_currentState = true;
                GameMaster.SetSpawnPlayer(transform);

            }
        }
    }

    public void EndGameState()
    {
        m_endGameState = true;
        m_currentState = false;
        m_anim.GetComponent<Animator>().SetBool("CurrentSpawner", false);
    }
}
