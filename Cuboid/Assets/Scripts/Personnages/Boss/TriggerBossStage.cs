using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBossStage : MonoBehaviour {

    public GameObject m_boss;

    private bool m_bossActive = false;

    // Use this for initialization
    void Start () {
        if (!m_boss)
            enabled = false;

        m_boss.SetActive(false);

        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && m_bossActive == false)
        {
            m_boss.SetActive(true);
            m_bossActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Boss")
        {
            //le boss est mort
            Debug.Log("LE BOSS EST MOURU");
        }
    }
}
