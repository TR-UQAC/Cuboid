using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerBossStage : MonoBehaviour {

    public GameObject m_boss;
    public GameObject m_Porte;
    public GameObject m_upgrade;

    private bool m_bossActive = false;
    private bool m_bossFin = false;

    private Vector3 m_lastPosBoss;

    // Use this for initialization
    void Start () {
        if (!m_boss)
            enabled = false;

        m_boss.SetActive(false);
        m_boss.GetComponent<boss>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_boss == null && m_bossFin == false)
        {
            m_bossFin = true;
            m_bossActive = false;
            //le boss est mort
            Debug.Log("LE BOSS EST MOURU");

            m_Porte.GetComponent<BoxCollider2D>().enabled = false;
            //  animation d'ouverture de porte

            //  faire pop l'upgrade
            Sequence loot = DOTween.Sequence();

            loot.Append(m_upgrade.transform.DOMove(m_lastPosBoss, 0.01f));
            loot.Append(m_upgrade.transform.DOShakePosition(5.0f, new Vector3(0.0f, -0.05f, 0.0f), 2, 40.0f, false, false).SetLoops(1000));
            loot.Play();

            
        }

        if(m_bossActive == true)
        {
            m_lastPosBoss = m_boss.transform.position;
        }

        //  test de sequence pour l'apparition de l'upgrade

        /*
        if ( m_bossActive == false)
        {
            //  faire pop l'upgrade
            Sequence loot = DOTween.Sequence();
            GameObject trPl = GameObject.FindGameObjectWithTag("Player");
            loot.Append(m_upgrade.transform.DOMove(trPl.transform.position + new Vector3(3.0f, 0.0f), 0.01f));
            loot.Append(m_upgrade.transform.DOShakePosition(5.0f, new Vector3(0.0f, -0.05f, 0.0f), 2, 40.0f, false,false).SetLoops(1000));
            loot.Play();

            m_bossActive = true;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("dans trigger");
        if(collision.tag == "Player" && m_bossActive == false)
        {
            Debug.Log("dans trigger qui start");
            m_boss.SetActive(true);
            m_boss.GetComponent<boss>().enabled = true;
            m_bossActive = true;
            m_Porte.GetComponent<BoxCollider2D>().enabled = true;
            //m_Porte.PlayAnimation()
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Boss")
        {
            //le boss est mort
            Debug.Log("LE BOSS EST MOURU");
        }

        if(collision.tag == "Player")
        {
            //m_boss.SetActive(false);
            m_boss.GetComponent<boss>().enabled = false;
            m_bossActive = false;
            m_Porte.GetComponent<BoxCollider2D>().enabled = false;
            m_boss.GetComponent<boss>().resetPV();
        }
    }
}
