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

    // Use this for initialization
    void Start () {
        if (!m_boss)
            enabled = false;

        m_boss.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(m_boss == null && m_bossFin == false)
        {
            //le boss est mort
            Debug.Log("LE BOSS EST MOURU");

            m_Porte.GetComponent<BoxCollider2D>().enabled = false;
            //  animation d'ouverture de porte

            //  faire pop l'upgrade
            Sequence loot = DOTween.Sequence();

            loot.Append(m_upgrade.transform.DOMove(m_boss.transform.position, 0.01f));
            loot.Append(m_upgrade.transform.DOShakePosition(5.0f, new Vector3(0.0f, -0.05f, 0.0f), 2, 40.0f, false, false).SetLoops(1000));
            loot.Play();

            m_bossFin = true;
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
        if(/*collision.tag == "Player" &&*/ m_bossActive == false)
        {
            m_boss.SetActive(true);
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
    }
}
